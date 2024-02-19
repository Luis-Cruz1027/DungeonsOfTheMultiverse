using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartMenu : MonoBehaviour
{
    public GameObject TitleMenu;
    public GameObject charMenu;

    public static StartMenu instance;
    public Character[] characters;
    public Character currentCharacter;

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
    }

    public void SetCharacter(Character character)
    {
        currentCharacter = character;
    }
    public void playGame()
    {
        charMenu.SetActive(false);
        SceneManager.LoadScene("DungeonScene");
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
}
