using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using Random=UnityEngine.Random;
using NUnit.Framework.Constraints;
using TMPro;


public class enemySpawner : MonoBehaviour
{
    //Add List of sprites
    public Sprite[] listOfImages;

    [SerializeField] private Image image;
    
    [SerializeField] private Tilemap dungeon;
    
    public SpriteRenderer spriteRenderer;
    public GameObject Goblin;

    private OpenAIController controller;
    private imageGeneration imagegeneration;
    private string testURL = "https://lh3.googleusercontent.com/fife/ALs6j_G_cjI4zQOGEI9oTz9wv5Hn8vigeLVCBecvzz-eFcq06DnY6TzAU-ydcAQQgke3E2FGvoj14Gm5nwMZTsJJkoPckh0Zf_7NlYgn43XgLfLycaqujS4eJQHa_l9UG3g0T5em8w7yMAv33tJEItXxMdl11nRo8alYxCyOXLXEtS2wzELKin3HXvmw4bJJw1lx3QdKvITjAf704957JciugUSztNfCtQAaoYlfMd-fRMfSanNs9VqOAsl3cDW9Rh03RS8Z_RovK-v27Xk25MiM1rMdYvjsJLLmsBnAaZBc6UKQ13K5kq7wpEjE4kbB5xeeFlviCxFEIWpUwjHVqgkXIC_DTm44pFE10S0dBgcBSEvVqoQu6j1rDg9bH8vLlwYkm8DlmOxVH8QYkZZgCh6EUzNMKrs7WIAbNrWcdfLlLF011Hydn1ZUVsjVUwzdqjnZtcqVNABMwu7uJhkbU9eA27NuOQL7hcHlg869o1kED3mydIOIxXKajR4b7RnWkGh0-xXK8_dmKpKtBXv-3TRhFOEfNpvjHO_P_DBIvnesAhGvP8zv73x7zUGxfKjhYlBNmrcBxbxBshXERhrRrsLLyFzeGJYXyk8k7QLzlzbmHMln3XIdkDvsO4kI1LQdgfeQ6cW8KYSNpCXW7LT5_oINEawBXxq_YHRnhI_seibwNfowHiLtBXCkieb5OJ-p3z7siqCgL6D4zJo4RZty7SGodgnkJ4a-_aRyA0y3an18z_YEy5OX6u0omaV9i5LJSXmQVD7gdP7n7-U2LxHlfQtxVXJfg5VNtFO5lnsQ5_WowGY40CdruhMkFdhqiEg0mNMxJoWStm8cHdq0evefEsbMSx8dgxRFnpf_BkX-fu9ush2y6CtzxuJPX8ldXtyXU7z1pJhrm4VOgjo7voHFXaStKe2EOkLP7OuQLY799VSc_lCVxl4cbEP5BKX8r9tFJo1nSim-BH_XM0siO1Vkg_QXGFakwZz9y25R205fEgrQMHCPg5MJrYbGmZi3_Lp4xvnkM3V3C5ESuPsFFu7JzplS4fuPYoxil4I0xOkxz9k59nCBbirif7UUUJKsXhzgFOPrbWOoRjAR8x8gzziiuYgfKsONKIImlurbdb7tqcCKfFLmlxUNpcUlXc6meUVXXBF6Vwu50IEjv-udNyFI2-nOn9ituduFWRkAwq7PgAiaqr4Ahi2fmUZjLp5RO8psQcaWPmRv2qVwdhH8Rq5arvWcRlLQd4EFbLLWO_BzRZRc2rUwz6MMHod8VAa0kvj-Jxqn3k3u5XGjCqbseIt7BJ0uVmGNl20E_5oNalRmbwlcerGx8_Ets8PQ4v_WpFBw5wRi2Hqq-d7nJAE4ml_hitqQFpnPQ52-TzX_X7PPfjXJasUcFpUoglxmOFPfGt2eGCEmjJEt8vDlNMicXoO62UodOLJBSWUUCbaaEG8snm7eoeftDikjLsLJIoUU3nBlUt_A1OP4anqS754oa-qxiPmYNEdu9DcVOmMZBWqt7tYebTSydLmq8rIle4nOinhnegIqSklqF9QrHqklhQs=w1920-h953";
    private bool semaphore = false;

    private Dictionary<BoundsInt, Vector3Int> roomCenters;
    private List<BoundsInt> generatedRooms;
    private TileManager manager;
    private StartMenu startmenu;

    void Start()
    {
        startmenu = GameObject.FindGameObjectWithTag("StartMenu").GetComponent<StartMenu>();
        tileSpawner myspawner = GameObject.FindGameObjectWithTag("TileManager").GetComponent<tileSpawner>();
        manager = GameObject.FindGameObjectWithTag("TileManager").GetComponent<TileManager>();
        roomCenters = myspawner.GetDictionary();
        generatedRooms = myspawner.GetList();
        imagegeneration = GameObject.FindGameObjectWithTag("ImageMaker").GetComponent<imageGeneration>();
        controller = GameObject.FindGameObjectWithTag("StartMenu").GetComponentInChildren<OpenAIController>();
    }

    public IEnumerator preGenSpawner()
    {
        Debug.Log("We in here preGenSpawner");

        //count couroutine
        int coroutineCount = 0;

        //instantiate list of images during runtime
        List<Sprite> tempImageList = new List<Sprite>();

        foreach (KeyValuePair<string, string> item in controller.monsters)
        {
            coroutineCount++;
            Debug.Log(item.Key + " " + item.Value);
            //create images and set them in sprite list and add each element to instantiated list
            StartCoroutine(imagegeneration.helperFunc(item.Value, () =>  //change contoller.description to be each monster description from dictionary
            {
                Debug.Log("Made url for image");

                StartCoroutine(urlImageLoader(imagegeneration.url, tempImageList, item.Key, () =>
                {
                    Debug.Log("created and added " + item.Key + " sprite to list");
                    coroutineCount--;
                }));
            }));

        }


        // Wait until all coroutines finish
        while (coroutineCount > 0)
        {
            yield return null;
        }

        // Once all coroutines finish do this
        listOfImages = tempImageList.ToArray();
        Debug.Log("all images generated in list");
        Debug.Log(listOfImages.Length);

        //reference textbox in main camera and attach textfield to it and activate canvas
        GameObject canvas = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>().GetChild(0).gameObject;
        canvas.SetActive(true);

        //set bool to false to get rid of load screen
        startmenu.LoadMenu.SetActive(false);
    }

    private IEnumerator urlImageLoader(string link, List<Sprite> tempList, string monsterName, Action callback)
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
                Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 800);

                //add sprite to list of images
                Debug.Log(monsterName + " added to array of sprites");
                newSprite.name = monsterName;
                tempList.Add(newSprite);
            }
            else
            {
                Debug.Log("texture null");
            }
        }

        semaphore = true;
        callback.Invoke();
    }

    public void doorSpawnEnemy(int numEnemies, string monsterType)
    {
        string tempMonster = monsterType.Substring(1);
        Debug.Log("spawning enemies in doorSpawn enemy type: " + tempMonster);

        Vector3Int doorpos = manager.getDoorPos();
        Vector3Int roomCenterActual = new Vector3Int(0,0,0);
        foreach(var room in roomCenters.Values){
            if(new Vector3Int(room.x - 4, room.y, 0) == doorpos || new Vector3Int(room.x + 4, room.y, 0 ) == doorpos || new Vector3Int(room.x, room.y - 4, 0 ) == doorpos || new Vector3Int(room.x, room.y + 4, 0 ) == doorpos){
                    roomCenterActual = room;
                    Debug.Log("We in here? " + room);
                    break;
            }
        }
      
        //search through dictionary and find and pop monster from dictionary. check to see if bad request.
        for (int i = 0; i < listOfImages.Length; i++)
        {
            Debug.Log("searching monster in loop, " + listOfImages[i].name);
            if (listOfImages[i].name == tempMonster)
            {
                Debug.Log("loading sprite");
                spriteRenderer.sprite = listOfImages[i];       //change to what enemy chatgpt wants to spawn from list
                spriteRenderer = Goblin.GetComponent<SpriteRenderer>();
                break;
            }
        }

        //spawn number of enemies based on what ChatGpt says
        for (int i = 0; i < numEnemies; i++)
        {
            Instantiate(Goblin, new Vector3(Random.Range(dungeon.CellToWorld(new Vector3Int(roomCenterActual.x - 3, roomCenterActual.y, 0)).x, dungeon.CellToWorld(new Vector3Int(roomCenterActual.x + 3, roomCenterActual.y, 0)).x), Random.Range(dungeon.CellToWorld(new Vector3Int(roomCenterActual.x, roomCenterActual.y - 3, 0)).y, dungeon.CellToWorld(new Vector3Int(roomCenterActual.x, roomCenterActual.y + 3, 0)).y), 0), Quaternion.identity);
            Debug.Log("spawned enemy, " + i);
        }

        manager.disableDoors(roomCenterActual);
    }
}
