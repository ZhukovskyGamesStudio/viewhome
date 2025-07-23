using Cysharp.Threading.Tasks;
using UnityEngine;

public class ModelControllerApi: ApiBase {
    public static async UniTask<Model> GetModel(string modelId) {
        using var request = Get($"model/{modelId}");
        await request.SendWebRequest();

        Debug.Log($"GetUser: {request.responseCode}");
        if (request.responseCode == 200) {
            Debug.Log($"GetUserdata: {request.downloadHandler.text}");
            var data = JsonUtility.FromJson<ModelApiData>(request.downloadHandler.text).data;
            return data.model;
        }

        return null;
    }
}
