using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class OpenAIController : MonoBehaviour
{
    private OpenAIAPI api;
    private List<ChatMessage> messages;

    public Button okButton;

    // Start is called before the first frame update
    void Start()
    {
        api = new OpenAIAPI("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");
        StartConversation();
        okButton.onClick.AddListener(() => GetResponse());
    }

    private void StartConversation()
    {
        messages = new List<ChatMessage> {
            new ChatMessage(ChatMessageRole.System, "You are a Dungeon Master in a Dungeon and Dragons campaign.") //Decides system personality
        };
        
    }

    private async void GetResponse()
    {


        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 0.2,
            MaxTokens = 50,
            Messages = messages
        });

        //get response message
        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.Content = chatResult.Choices[0].Message.Content;
        Debug.Log(string.Format("{0}: {1}", responseMessage.rawRole, responseMessage.Content));

        //add response to list of messages
        messages.Add(responseMessage);

        //update text field with response
    }
}
