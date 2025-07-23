using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class ApiBase {
    protected static readonly string _baseUrl = "https://viewhome.simul.me/";

    protected static UnityWebRequest Get(string uri) {
        return UnityWebRequest.Get(_baseUrl + uri);
    }
    
    public static async UniTask<Texture2D> GetPicture(string uri) {
        using UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(uri);
        await uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success) {
            Debug.Log(uwr.error);
        } else {
            Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
            return texture;
        }

        return null;
    }
    
    public static async UniTask<string> GetModel(string uri) {
        var request = UnityWebRequest.Get(uri);
        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success) {
            Debug.Log(request.error);
        } else {
            string filename = uri.Split('/')[^1];
            if (!filename.Contains('.')) {
                filename = $"model{Random.Range(1000, 9999)}.obj"; // Default filename if none is provided
            }

            string savePath = string.Format($"{Application.persistentDataPath}/{filename}", Application.persistentDataPath, filename);
           await System.IO.File.WriteAllBytesAsync(savePath, request.downloadHandler.data);
            Debug.Log($"File saved to: {savePath}");
            return savePath;
        }

        return null;
    }

    protected static UnityWebRequest Post(string uri, string json) {
        return UnityWebRequest.Post(_baseUrl + uri, json, contentType: "application/json");
    }

    protected static UnityWebRequest Put(string uri, string json) {
        return UnityWebRequest.Put(_baseUrl + uri, json);
    }

    protected static UnityWebRequest Delete(string uri) {
        return UnityWebRequest.Delete(_baseUrl + uri);
    }
}