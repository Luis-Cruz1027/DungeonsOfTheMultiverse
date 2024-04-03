using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectRightClick : MonoBehaviour
{
    public GameObject Enemy;
    private Canvas Menu;

    //chatgpt
    private OpenAIController controller;

    void Start()
    {
        Menu = GetComponentInChildren<Canvas>();
        Menu.enabled = false;

        //reference chatgpt script
        controller = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<OpenAIController>();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Menu.enabled = true;
        }
    }

    public void KillButton()
    {
        //decrement counter
        controller.numEnemies -= 1;

        //If counter reaches 0 send ChatGPT request.
        if (controller.numEnemies == 0)
        {
            controller.GetResponse("All enemies have been slain, reward warriors with appropriate exp, with a 10% chance of enemy drops per enemy.");
        }

        //delete gameObject
        Destroy(Enemy);
    }

    public void CancelButton()
    {
        Menu.enabled = false;
    }
}
