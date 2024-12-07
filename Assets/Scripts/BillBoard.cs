using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
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
            // Attach the certificate bypass handler
            request.certificateHandler = new BypassCertificateHandler();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                Debug.Log("So far so good");
                string json = request.downloadHandler.text;

                // Parse the JSON response
                var quote = JObject.Parse(json);

                string quoteContent = quote["content"].ToString();
                string authorName = quote["author"].ToString();

                // Update text fields
                if (quoteText != null) quoteText.text = quoteContent;
                if (authorText != null) authorText.text = authorName;
            }
        }
    }
}

// Bypass SSL certificate validation
public class BypassCertificateHandler : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        // Always return true to bypass SSL validation
        return true;
    }
}
