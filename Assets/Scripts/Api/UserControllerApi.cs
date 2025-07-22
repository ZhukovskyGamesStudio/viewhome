using Cysharp.Threading.Tasks;
using UnityEngine;

public class UserControllerApi : ApiBase {
    public static async UniTask GetUser(string userId) {
        using var request = Get($"user/{userId}");
        await request.SendWebRequest();

        Debug.Log($"GetUser: {request.responseCode}");
        if (request.responseCode == 200) {
            Debug.Log($"GetUserdata: {request.downloadHandler.text}");
        }
    }
}