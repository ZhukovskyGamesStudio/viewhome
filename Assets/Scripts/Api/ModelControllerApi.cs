using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class ModelControllerApi : ApiBase {
    public static async UniTask<Model> GetModel(string modelId) {
        using UnityWebRequest request = Get($"model/{modelId}");
        await request.SendWebRequest();

        Debug.Log($"GetUser: {request.responseCode}");
        if (request.responseCode == 200) {
            Debug.Log($"GetUserdata: {request.downloadHandler.text}");
            Data data = JsonUtility.FromJson<ModelApiData>(request.downloadHandler.text).data;
            return data.model;
        }

        return null;
    }
}