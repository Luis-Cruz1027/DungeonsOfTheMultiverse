using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//import libraries
using OpenAI_API;
using OpenAI_API.Images;
using OpenAI_API.Models;
using System.Threading.Tasks;
using System;

public class imageGeneration : MonoBehaviour
{
    private OpenAIAPI api;
    public string url;
    private OpenAIController controller;
    private Dictionary<string, string> errorMonsters;

    void Awake()
    {
        api = new OpenAIAPI("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");

        controller = GameObject.FindGameObjectWithTag("StartMenu").GetComponentInChildren<OpenAIController>();

        //make hardcoded data for error handling
        errorMonsters = new Dictionary<string, string>() {
            { "Goblins", "Close up, gothic fantasy, digital painting, of a green goblin." },
            { "Vampires", "Close up, gothic fantasy, digital painting, of a pale vampire." },
            { "Bandits", "Close up, gothic fantasy, digital painting, of a beared bandit with ragged clothes." },
            { "Trolls", "Close up, gothic fantasy, digital painting, of a green troll and a giant club." }
        };

        //makeURLImages("A red dragon from mediaval times spewing fire from its mouth", () => { });
    }

    public IEnumerator helperFunc(string prompt, string currentMonster, Action callback)
    {
        yield return makeURLImages(prompt, currentMonster, callback);
    }

    public async Task makeURLImages(string prompt, string currentMonster, Action callback)
    {
        try
        {
            Debug.Log("making url and waiting...");
            Debug.Log(prompt);
            var result = await api.ImageGenerations.CreateImageAsync(prompt);
            url = result.ToString();
            Debug.Log(url);

            callback.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogError("An error occurred while creating URL: " + ex.Message);

            //use hardcoded data to handle errors and pop previsouly failed data from list
            controller.monsters.Remove(currentMonster);
            foreach (KeyValuePair<string, string> Item in errorMonsters)
            {
                if (Item.Key.Length > 0)
                {
                    controller.monsters.Add(Item.Key, Item.Value);

                    var result = await api.ImageGenerations.CreateImageAsync(Item.Value);
                    url = result.ToString();
                    Debug.Log("new url for error: " + url);

                    errorMonsters.Remove(Item.Key);

                    break;
                }
            }

            //Create new monster List to send to ChatGPT
            controller.monsterList = "[";

            foreach (KeyValuePair<string, string> Item in controller.monsters)
            {
                Debug.Log(Item.Key + " ");
                controller.monsterList += Item.Key + ", ";
            }

            controller.monsterList += "]";

            callback.Invoke();
        }
    }
}