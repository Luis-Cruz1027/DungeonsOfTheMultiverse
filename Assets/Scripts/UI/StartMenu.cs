using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System.Threading.Tasks;

public class StartMenu : MonoBehaviour
{
    public GameObject TitleMenu;
    public GameObject charMenu;
    public GameObject LoadMenu;

    public static StartMenu instance;
    public Character[] characters;
    public Character currentCharacter;

    private OpenAIAPI api;
    private OpenAIController apiController;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
       if (characters.Length > 0)
        {
            currentCharacter = characters[0];
        }
        apiController = GameObject.Find("AIController").GetComponent<OpenAIController>();
    }


    public void SetCharacter(Character character)
    {
        currentCharacter = character;
    }
    public void playGame()
    {
        charMenu.SetActive(false);
        LoadMenu.SetActive(true);
        SceneManager.LoadScene(1);
        StartCoroutine(WaitFor(1f));
        apiController.loadMonsters();
    }

    public void quitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void charScreen()
    {
        TitleMenu.SetActive(false);
        charMenu.SetActive(true);
    }

    private IEnumerator WaitFor(float delay){
        yield return new WaitForSeconds(delay);
    }
}
