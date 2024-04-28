using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tabs : MonoBehaviour
{
    [SerializeField] private GameObject current;
    [SerializeField] private GameObject history;
    [SerializeField] private Image currButton;
    [SerializeField] private Image histButton;
    [SerializeField] private GameObject rollMenu;
    private bool toggle = false;

    // Start is called before the first frame update

    public void switchToCurrent(){
        currButton.color = Color.gray;
        histButton.color = Color.white;
        current.SetActive(true);
        history.SetActive(false);
    }

    public void switchToHistory(){
        currButton.color = Color.white;
        histButton.color = Color.gray;
        history.SetActive(true);
        current.SetActive(false);
    }

    public void toggleRollMenu(){
        switch(toggle){
            case true:
                rollMenu.SetActive(false);
                toggle = false;
                break;
            case false:
                rollMenu.SetActive(true);
                toggle = true;
                break;
        }
    }
}