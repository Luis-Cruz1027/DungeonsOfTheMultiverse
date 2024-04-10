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
    public Slider LoadingBarFill;

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

    public void Update(){
        if(LoadingBarFill.value == 1){
            LoadMenu.SetActive(false);
            SceneManager.LoadScene("DungeonScene");
            LoadingBarFill.value = 0;
        }
    }

    public void SetCharacter(Character character)
    {
        currentCharacter = character;
    }
    public async void playGame()
    {
        charMenu.SetActive(false);
        LoadMenu.SetActive(true);
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

    public void changeLoadingBar(int i, int size = 5){
        LoadingBarFill.value = i/size;
    }
}
