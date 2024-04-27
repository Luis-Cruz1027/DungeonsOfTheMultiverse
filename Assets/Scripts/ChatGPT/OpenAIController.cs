using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using JetBrains.Annotations;

public class OpenAIController : MonoBehaviour
{
    private OpenAIAPI api;
    private List<ChatMessage> messages;
    private List<ChatMessage> preGenMessages;
    public string monsterList;
    private string someText;
    private string temp;
    //spawner variables and GameObjects
    private enemySpawner enemySpawn;
    public string enemyType;
    public int numEnemies;
    public string monsterDescription;


    public TMP_Text textField;
    //public TMP_InputField inputField;
    public Dictionary<string, string> monsters;
    public StartMenu startMenu;

    private tileSpawner theSpawner;

    // Start is called before the first frame update
    void Start()
    {
        someText = "The party enters a new room.";

        api = new OpenAIAPI("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");
        StartConversation();

        startMenu = GameObject.FindGameObjectWithTag("StartMenu").GetComponent<StartMenu>();
    }

    private void StartConversation()
    {
        messages = new List<ChatMessage> {
            new ChatMessage(ChatMessageRole.System, "You are a Dungeon Master in a text based Dungeons and Dragons game. You will use Dungeons and Dragons handbook as reference.\r\n\r\nThe user will tell you once they enter a new room and give you a list of monsters that you will choose to decide which monster will spawn in the room.\r\n\r\nYour job will be to describe the room, decide if the party should do a AC check as soon as they enter the room based on location an, you must pick a single monster from a list of monsters given to you that will decide which monster will spawn in the given room, how many monsters, and describe the stats of the monsters they will face.  You will decide AC, HP, speed, and basic stats for the monsters. You will also decide what special abilities the monsters have.\r\n\r\nYou will start your response with the name of the monster followed a comma then the amount of monsters all be incased in brackets.\r\n\r\nExample of your response:\r\n[Spectral Wraiths, 2]\r\n\r\nAs the party steps into the room, they are met with a chilling sight of spectral wraiths gliding towards them, their incorporeal forms swaying with an otherworldly presence. The wraiths emit a faint, eerie glow that illuminates their tattered robes, giving them a ghastly appearance. Wisps of ethereal mist trail behind them, exuding an icy cold that sends shivers down the spines of the party members.\r\n\r\nDue to the spectral nature of the wraiths, the party will need to do an AC check upon entering to see if they can resist the wraiths' chilling touch.\r\n\r\nNumber of Monsters: 2\r\n\r\nSpectral Wraith Stats:\r\n- AC: 12\r\n- HP: 30\r\n- Speed: 0 ft (Hover)\r\n- Immunities: Cold, Necrotic, Poison\r\n- Condition Immunities: Exhaustion, Poisoned, Charmed, Frightened\r\n- Special Ability: Chilling Touch - The spectral wraiths can reach out and touch a target within 5 feet. The target must succeed on a DC 11 Constitution saving throw or take 2d6 cold damage.") //Decides system personality
        };

        preGenMessages = new List<ChatMessage> {
            new ChatMessage(ChatMessageRole.System, "Create a unique monster that you have not created yet from the Dungeon and Dragons Handbook.\r\n\r\nYou will start the your response the name of the monster followed by a comma then you will provide a visual description of a singular monster for a Dall-E prompt.\r\nThe prompts must always be close ups, digital painting, and gothic fantasy style.\r\n\r\nExample of your response: \r\nSpectral Wraith, A close up shot, digital painting, gothic fantasy picture of a wraith with tattered robes and dark backlighting. ") //Decides system personality
        };
    }

    public async void GetResponse(string response)
    {
        textField = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<TMP_Text>();
        //reset variables on each call


        enemyType = "";
        numEnemies = 0;
        monsterDescription = "";
        temp = "";

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
            MaxTokens = 1000,
            Messages = messages
        });
           
        //Get response message
        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.Content = chatResult.Choices[0].Message.Content;
        Debug.Log(string.Format("{0}: {1}", responseMessage.rawRole, responseMessage.Content));

        //skip this if not monster spawns
        if (responseMessage.Content[0] == '[')
        {
            int i;  //start parsing string
            for (i = 0; i < responseMessage.Content.Length; i++)
            {
                if (responseMessage.Content[i] == ']')
                {
                    break;
                }
                if (responseMessage.Content[i] != ',')
                {
                    enemyType += responseMessage.Content[i];
                }
            }
            int counter = i;    //need since for loops must be initialized in C#

            for (i = counter + 1; i < responseMessage.Content.Length; i++)
            {
                if (responseMessage.Content[i] == ']')
                {
                    temp = responseMessage.Content.Substring(i + 1);
                    break;
                }

                if (responseMessage.Content[i] != ']')
                {
                    monsterDescription += responseMessage.Content[i];
                }
            }
            numEnemies = (int)enemyType[enemyType.Length - 1] - 48;     //gives ASCI value must subtract

            //Debug.Log(numEnemies);
            enemyType = enemyType.Substring(0, enemyType.Length - 2);
            //Debug.Log(enemyType);


            Debug.Log(monsterDescription);

            //spawn enemies only if detect enemy
            if (enemyType.Length > 0)
            {
                enemySpawn.doorSpawnEnemy(numEnemies, enemyType);
            }
        }


        //add response to list of messages
        messages.Add(responseMessage);

        //update text field with response
        if (temp.Length > 0)
        {
            textField.text += string.Format("You: {0}\n\nDM: {1}\n\n", userMessage.Content, temp);
        }
        else
        {
            textField.text += string.Format("DM: {0}\n\n", responseMessage.Content);
        }
        
    }

    public async void loadMonsters(){
        theSpawner = GameObject.FindGameObjectWithTag("TileManager").GetComponent<tileSpawner>();
        monsters = new Dictionary<string,string>(); 
        //var chat = api.Chat.CreateConversation();


        for(int i = 0; i < theSpawner.GetList().Count - 1; i++){
            ChatMessage userMessage = new ChatMessage();
            userMessage.Role = ChatMessageRole.User;
            userMessage.Content = "Create a monster you haven't created yet.";

            //add message to list
            preGenMessages.Add(userMessage);

            var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
            {
                Model = Model.ChatGPTTurbo,
                Temperature = 1,
                MaxTokens = 200,
                Messages = preGenMessages
            });

            //string monster = await chat.GetResponseFromChatbot();
            ChatMessage responseMessage = new ChatMessage();
            responseMessage.Role = chatResult.Choices[0].Message.Role;
            responseMessage.Content = chatResult.Choices[0].Message.Content;

            string monsterName = "";
            int index = 0;
            for(int j = 0; j < responseMessage.Content.Length; j++){
                if(responseMessage.Content[j] == ','){
                    index = j + 1;
                    monsterList += ", ";
                    break;
                }
                else{
                    monsterName += responseMessage.Content[j];
                    monsterList += responseMessage.Content[j];
                }
            }
            
            string monsterDesc = "";
            for(int k = index; k < responseMessage.Content.Length; k++){
                if(responseMessage.Content[k] == null){
                    break;
                }
                monsterDesc += responseMessage.Content[k];
            }
            
            monsters.Add(monsterName, monsterDesc);
            Debug.Log(monsterName + " " + monsterDesc);
            // await Task.Delay(3000);

            //add response to list of messages to pregen list
            preGenMessages.Add(responseMessage);
        }

        //reference enemy spawner and call a method in it.
        enemySpawn = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<enemySpawner>();

        Debug.Log("got out of the loop successfully!");
        StartCoroutine(enemySpawn.preGenSpawner());
    }
}
