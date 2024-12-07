using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using System;
using UnityEditor.PackageManager.Requests;

using Newtonsoft.Json.Linq;


public class Quote3DBillboard : MonoBehaviour
{
    public TextMeshPro quoteText;
    public TextMeshPro authorText;

    // API endpoint for random quotes
    private string url = "https://api.quotable.io/random";

    private void Start()
    {

        // Fetch a quote when the script starts
        StartCoroutine(FetchQuote());

    }

    IEnumerator FetchQuote()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url)) 
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError) 
            { 
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("So far so good");
                string json = request.downloadHandler.text;
                var quote = JObject.Parse(json);
            }
        }
    }


}