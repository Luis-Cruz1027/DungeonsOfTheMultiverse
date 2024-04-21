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

    void Awake()
    {
        api = new OpenAIAPI("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");

        //makeURLImages("A red dragon from mediaval times spewing fire from its mouth", () => { });
    }

    public IEnumerator helperFunc(string prompt, Action callback)
    {
        yield return makeURLImages(prompt, callback);
    }

    public async Task makeURLImages(string prompt, Action callback)
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
        }
    }
}