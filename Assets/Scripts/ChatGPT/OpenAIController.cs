using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class OpenAIController : MonoBehaviour
{
    private OpenAIAPI api;
    private List<ChatMessage> messages;
    private string someText;
    private string temp;
    private string testURL = "https://drive.google.com/drive/folders/1l5ecWVskqufnRowYNbE3HUAM4FgMM76P";
    //spawner variables and GameObjects
    private enemySpawner enemySpawner;
    private string enemyType;
    private int numEnemies;

    public TMP_Text textField;
    //public TMP_InputField inputField;
    public Button okButton;

    // Start is called before the first frame update
    void Start()
    {
        someText = "The party enters a new room.";

        api = new OpenAIAPI("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");
        StartConversation();
        okButton.onClick.AddListener(() => GetResponse(someText));

        //initialize spawners
        enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<enemySpawner>();
    }

    private void StartConversation()
    {
        messages = new List<ChatMessage> {
            new ChatMessage(ChatMessageRole.System, "You are the Dungeon Master in a chat based game. You will decide what encounters we will face whenever we open a door.\r\n\r\nEnemies you can spawn\r\n1. Goblin\r\n\r\nWhenever the party enters a new room.\r\n80% of the time\r\n{\r\nYou will decide how many enemies will spawn once the user tells you when they open a door and what type of enemies spawn. you will start your sentence with \"[the type of enemy, amount of enemies] \".\r\n}\r\n20% of the time {\r\nno enemies will spawn. in this case you will not need to start your sentence with the type or the amount. simply give a normal statement of what you would like to describe within the room.\r\n}") //Decides system personality
        };

        string startString = "Welcome adventurer, you have just begun youre journey into this unforgiving dungeon.\n\n";
        textField.text += startString;
        Debug.Log(startString);
    }

    public async void GetResponse(string response)
    {
        //disable button for no spam and reset variables
        okButton.enabled = false;
        enemyType = "";
        numEnemies = 0;


        //fill message into inputfield
        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = ChatMessageRole.User;
        userMessage.Content = response;
        Debug.Log(string.Format("{0}: {1}", userMessage.rawRole, userMessage.Content));
        //add message to list
        messages.Add(userMessage);
           
        
        //send message to chatGPT
        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 1,
            MaxTokens = 50,
            Messages = messages
        });
           
        //Get response message
        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.Content = chatResult.Choices[0].Message.Content;
        Debug.Log(string.Format("{0}: {1}", responseMessage.rawRole, responseMessage.Content));

        for (int i = 1; i < responseMessage.Content.Length; i++)
        {
            if (responseMessage.Content[i] == ']')
            {
                temp = responseMessage.Content.Substring(i + 1);
                break;
            }
            if (responseMessage.Content[i] != ',')
            {
                enemyType += responseMessage.Content[i];
            }
        }

        numEnemies = (int)enemyType[enemyType.Length - 1] - 48;     //gives ASCI value must subtract

        //Debug.Log(enemyType);
        Debug.Log(numEnemies);
        enemyType = enemyType.Substring(0, enemyType.Length - 2);
        Debug.Log(enemyType);

        //spawn enemies only if detect enemy
        if (enemyType == "Goblin")
        {
            enemySpawner.doorSpawnEnemy(numEnemies);
        }

        //add response to list of messages
        messages.Add(responseMessage);

        //update text field with response
        textField.text += string.Format("You: {0}\n\nDM: {1}\n\n", userMessage.Content, temp);
        
        //reactivate button
        okButton.enabled = true;
    }
}
