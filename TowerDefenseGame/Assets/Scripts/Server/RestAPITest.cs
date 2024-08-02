using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RestAPITest : MonoBehaviour
{
    private string url = "http://ec2-52-78-67-151.ap-northeast-2.compute.amazonaws.com:9000";

    void Start()
    {
        StartCoroutine(GetRequest(url));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log("Received: " + webRequest.downloadHandler.text);
            }
        }
    }

    IEnumerator PostRequest(string uri, string jsonData)
    {
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

        using (UnityWebRequest webRequest = new UnityWebRequest(uri, "POST"))
        {
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log("Received: " + webRequest.downloadHandler.text);
            }
        }
    }

}
