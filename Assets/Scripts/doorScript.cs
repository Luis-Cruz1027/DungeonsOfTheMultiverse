using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScript : MonoBehaviour
{
    private string newRoomText;
    private OpenAIController controller;

    private void Start()
    {
        controller = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<OpenAIController>();
        newRoomText = "The party enters a new room.";
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            //call door method;
        }
    }

    private void doorTrigger()
    {
        Debug.Log("door triggered.");
        controller.GetResponse(newRoomText);
    }
}
