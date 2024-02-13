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
    }

    private void StartConversation()
    {
        messages = new List<ChatMessage> {
            new ChatMessage(ChatMessageRole.System, "You are a Dungeon Master in a Dungeon and Dragons campaign. You will only decide what encounters we face whenever we enter a new room.") //Decides system personality
        };

        string startString = "Welcome adventurer, you have just begun youre journey into this unforgiving dungeon.\n\n";
        textField.text += startString;
        Debug.Log(startString);
    }

    public async void GetResponse(string response)
    {
        //disable button for no spam
        okButton.enabled = false;

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

        //add response to list of messages
        messages.Add(responseMessage);

        //update text field with response
        textField.text += string.Format("You: {0}\n\nDM: {1}\n\n", userMessage.Content, responseMessage.Content);
           
        //reactivate button
        okButton.enabled = true;
    }
}
