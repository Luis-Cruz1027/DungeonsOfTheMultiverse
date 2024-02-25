using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Unity.VisualScripting;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] private Image image;
    
    public SpriteRenderer spriteRenderer;
    public GameObject Goblin;

    private imageGeneration imagegeneration;
    private string testURL = "https://lh3.googleusercontent.com/fife/ALs6j_G_cjI4zQOGEI9oTz9wv5Hn8vigeLVCBecvzz-eFcq06DnY6TzAU-ydcAQQgke3E2FGvoj14Gm5nwMZTsJJkoPckh0Zf_7NlYgn43XgLfLycaqujS4eJQHa_l9UG3g0T5em8w7yMAv33tJEItXxMdl11nRo8alYxCyOXLXEtS2wzELKin3HXvmw4bJJw1lx3QdKvITjAf704957JciugUSztNfCtQAaoYlfMd-fRMfSanNs9VqOAsl3cDW9Rh03RS8Z_RovK-v27Xk25MiM1rMdYvjsJLLmsBnAaZBc6UKQ13K5kq7wpEjE4kbB5xeeFlviCxFEIWpUwjHVqgkXIC_DTm44pFE10S0dBgcBSEvVqoQu6j1rDg9bH8vLlwYkm8DlmOxVH8QYkZZgCh6EUzNMKrs7WIAbNrWcdfLlLF011Hydn1ZUVsjVUwzdqjnZtcqVNABMwu7uJhkbU9eA27NuOQL7hcHlg869o1kED3mydIOIxXKajR4b7RnWkGh0-xXK8_dmKpKtBXv-3TRhFOEfNpvjHO_P_DBIvnesAhGvP8zv73x7zUGxfKjhYlBNmrcBxbxBshXERhrRrsLLyFzeGJYXyk8k7QLzlzbmHMln3XIdkDvsO4kI1LQdgfeQ6cW8KYSNpCXW7LT5_oINEawBXxq_YHRnhI_seibwNfowHiLtBXCkieb5OJ-p3z7siqCgL6D4zJo4RZty7SGodgnkJ4a-_aRyA0y3an18z_YEy5OX6u0omaV9i5LJSXmQVD7gdP7n7-U2LxHlfQtxVXJfg5VNtFO5lnsQ5_WowGY40CdruhMkFdhqiEg0mNMxJoWStm8cHdq0evefEsbMSx8dgxRFnpf_BkX-fu9ush2y6CtzxuJPX8ldXtyXU7z1pJhrm4VOgjo7voHFXaStKe2EOkLP7OuQLY799VSc_lCVxl4cbEP5BKX8r9tFJo1nSim-BH_XM0siO1Vkg_QXGFakwZz9y25R205fEgrQMHCPg5MJrYbGmZi3_Lp4xvnkM3V3C5ESuPsFFu7JzplS4fuPYoxil4I0xOkxz9k59nCBbirif7UUUJKsXhzgFOPrbWOoRjAR8x8gzziiuYgfKsONKIImlurbdb7tqcCKfFLmlxUNpcUlXc6meUVXXBF6Vwu50IEjv-udNyFI2-nOn9ituduFWRkAwq7PgAiaqr4Ahi2fmUZjLp5RO8psQcaWPmRv2qVwdhH8Rq5arvWcRlLQd4EFbLLWO_BzRZRc2rUwz6MMHod8VAa0kvj-Jxqn3k3u5XGjCqbseIt7BJ0uVmGNl20E_5oNalRmbwlcerGx8_Ets8PQ4v_WpFBw5wRi2Hqq-d7nJAE4ml_hitqQFpnPQ52-TzX_X7PPfjXJasUcFpUoglxmOFPfGt2eGCEmjJEt8vDlNMicXoO62UodOLJBSWUUCbaaEG8snm7eoeftDikjLsLJIoUU3nBlUt_A1OP4anqS754oa-qxiPmYNEdu9DcVOmMZBWqt7tYebTSydLmq8rIle4nOinhnegIqSklqF9QrHqklhQs=w1920-h953";
    private bool semaphore = false;
    

    private IEnumerator urlImageLoader(string link, Action callback)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(link);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            if (texture != null)
            {
                Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 250);

                image.sprite = newSprite;
            }
            else
            {
                Debug.Log("texture null");
            }
        }

        semaphore = true;
        callback.Invoke();
    }
    public void doorSpawnEnemy(int numEnemies)
    {
        StartCoroutine(urlImageLoader(testURL, () =>
            {
                if (semaphore)
                {
                    spriteRenderer.sprite = image.sprite;
                    spriteRenderer = Goblin.GetComponent<SpriteRenderer>();

                    for (int i = 0; i < numEnemies; i++)
                    {
                        Instantiate(Goblin, transform.position, Quaternion.identity);
                        Debug.Log("spawned enemy, " + i);
                    }
                }
                else
                {
                    Debug.Log("need to wait until testURL finishes");
                }
            }));
    }
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Unity.VisualScripting;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] private Image image;
    
    public SpriteRenderer spriteRenderer;
    public GameObject Goblin;

    private imageGeneration imagegeneration;
    private string testURL = "https://lh3.googleusercontent.com/fife/ALs6j_G_cjI4zQOGEI9oTz9wv5Hn8vigeLVCBecvzz-eFcq06DnY6TzAU-ydcAQQgke3E2FGvoj14Gm5nwMZTsJJkoPckh0Zf_7NlYgn43XgLfLycaqujS4eJQHa_l9UG3g0T5em8w7yMAv33tJEItXxMdl11nRo8alYxCyOXLXEtS2wzELKin3HXvmw4bJJw1lx3QdKvITjAf704957JciugUSztNfCtQAaoYlfMd-fRMfSanNs9VqOAsl3cDW9Rh03RS8Z_RovK-v27Xk25MiM1rMdYvjsJLLmsBnAaZBc6UKQ13K5kq7wpEjE4kbB5xeeFlviCxFEIWpUwjHVqgkXIC_DTm44pFE10S0dBgcBSEvVqoQu6j1rDg9bH8vLlwYkm8DlmOxVH8QYkZZgCh6EUzNMKrs7WIAbNrWcdfLlLF011Hydn1ZUVsjVUwzdqjnZtcqVNABMwu7uJhkbU9eA27NuOQL7hcHlg869o1kED3mydIOIxXKajR4b7RnWkGh0-xXK8_dmKpKtBXv-3TRhFOEfNpvjHO_P_DBIvnesAhGvP8zv73x7zUGxfKjhYlBNmrcBxbxBshXERhrRrsLLyFzeGJYXyk8k7QLzlzbmHMln3XIdkDvsO4kI1LQdgfeQ6cW8KYSNpCXW7LT5_oINEawBXxq_YHRnhI_seibwNfowHiLtBXCkieb5OJ-p3z7siqCgL6D4zJo4RZty7SGodgnkJ4a-_aRyA0y3an18z_YEy5OX6u0omaV9i5LJSXmQVD7gdP7n7-U2LxHlfQtxVXJfg5VNtFO5lnsQ5_WowGY40CdruhMkFdhqiEg0mNMxJoWStm8cHdq0evefEsbMSx8dgxRFnpf_BkX-fu9ush2y6CtzxuJPX8ldXtyXU7z1pJhrm4VOgjo7voHFXaStKe2EOkLP7OuQLY799VSc_lCVxl4cbEP5BKX8r9tFJo1nSim-BH_XM0siO1Vkg_QXGFakwZz9y25R205fEgrQMHCPg5MJrYbGmZi3_Lp4xvnkM3V3C5ESuPsFFu7JzplS4fuPYoxil4I0xOkxz9k59nCBbirif7UUUJKsXhzgFOPrbWOoRjAR8x8gzziiuYgfKsONKIImlurbdb7tqcCKfFLmlxUNpcUlXc6meUVXXBF6Vwu50IEjv-udNyFI2-nOn9ituduFWRkAwq7PgAiaqr4Ahi2fmUZjLp5RO8psQcaWPmRv2qVwdhH8Rq5arvWcRlLQd4EFbLLWO_BzRZRc2rUwz6MMHod8VAa0kvj-Jxqn3k3u5XGjCqbseIt7BJ0uVmGNl20E_5oNalRmbwlcerGx8_Ets8PQ4v_WpFBw5wRi2Hqq-d7nJAE4ml_hitqQFpnPQ52-TzX_X7PPfjXJasUcFpUoglxmOFPfGt2eGCEmjJEt8vDlNMicXoO62UodOLJBSWUUCbaaEG8snm7eoeftDikjLsLJIoUU3nBlUt_A1OP4anqS754oa-qxiPmYNEdu9DcVOmMZBWqt7tYebTSydLmq8rIle4nOinhnegIqSklqF9QrHqklhQs=w1920-h953";
    private bool semaphore = false;

    private void Start()
    {
        imagegeneration = GameObject.FindGameObjectWithTag("ImageMaker").GetComponent<imageGeneration>();
    }
    private IEnumerator urlImageLoader(string link, Action callback)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(link);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            if (texture != null)
            {
                Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 400);

                image.sprite = newSprite;
            }
            else
            {
                Debug.Log("texture null");
            }
        }

        semaphore = true;
        callback.Invoke();
    }
    public void doorSpawnEnemy(int numEnemies)
    {
        imagegeneration.makeURLImages("Goblin", () =>
        {
            StartCoroutine(urlImageLoader(imagegeneration.url, () =>
            {
                if (semaphore)
                {
                    spriteRenderer.sprite = image.sprite;
                    spriteRenderer = Goblin.GetComponent<SpriteRenderer>();

                    for (int i = 0; i < numEnemies; i++)
                    {
                        Instantiate(Goblin, transform.position, Quaternion.identity);
                        Debug.Log("spawned enemy, " + i);
                    }
                }
                else
                {
                    Debug.Log("need to wait until testURL finishes");
                }
            }));
        });
    }
}

 */
