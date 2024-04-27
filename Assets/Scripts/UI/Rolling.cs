using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Rolling : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roll;

    private void displayResult(int dice){
        int diceRoll = Random.Range(1, dice + 1);

        if(dice == 10){
            diceRoll *= 10;
        }
        
        roll.text = diceRoll.ToString();
    }

    public void rolld4(){
        displayResult(4);
    }

    public void rolld6(){
        displayResult(6);
    }

    public void rolld8(){
        displayResult(8);
    }

    public void rolld100(){
        displayResult(10);
    }

    public void rolld12(){
        displayResult(12);
    }

    public void rolld20(){
        displayResult(20);
    }
}
