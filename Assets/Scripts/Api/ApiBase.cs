using System;
using System.Globalization;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public static class ApiBase {
    static string _baseUrl = "https://viewhome.simul.me/";
    
    

    public static async UniTask SignUp(string email) {
        var url = $"{_baseUrl}auth/signUp";
        
        var data = new SignUpApiData() {
            name = "name",
            email = email,
            consent = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ",CultureInfo.InvariantCulture), 
            surname = "surname",
            phone = "777777777777"
        };
        string dataStr = JsonUtility.ToJson(data);
        
        using (var request = UnityWebRequest.Post(url, dataStr, contentType: "application/json"))
        {
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error signing up: {request.error}");
            }
            else
            {
                Debug.Log("Sign up successful!");
            }
            Debug.Log($"SignUp: {request.responseCode}" );
        }
    }

    public static async UniTask SignIn(string email, string password) {
        var url = $"{_baseUrl}auth/signIn";
        var data = new SignInApiData() {
            email = email,
            password = password
        };
        string dataStr = JsonUtility.ToJson(data);
        using (var request = UnityWebRequest.Post(url, dataStr, contentType: "application/json"))
        {
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error signing up: {request.error}");
            }
            else
            {
                Debug.Log("Sign in successful!");
            }

            Debug.Log($"SignIn: {request.responseCode}" );
        }
        
    }
}
