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

    void Start()
    {
        api = new OpenAIAPI("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");

        //makeURLImages("A red dragon from mediaval times spewing fire from its mouth", () => { });
    }

    public async void makeURLImages(string prompt, Action callback) {

        var result = await api.ImageGenerations.CreateImageAsync(prompt);
        url = result.ToString();
        //Debug.Log(url);

        callback.Invoke();
    }
}