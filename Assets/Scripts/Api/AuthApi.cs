using System;
using System.Globalization;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AuthApi : ApiBase {
    public static async UniTask SignUp(string email) {
        string dataStr = JsonUtility.ToJson(new SignUpApiData() {
            name = "name",
            email = email,
            consent = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture),
            surname = "surname",
            phone = "777777777777"
        });

        using var request = Post("auth/signUp", dataStr);
        await request.SendWebRequest();

        Debug.Log($"SignUp: {request.responseCode}");
    }

    public static async UniTask SignIn(string email, string password) {
        string dataStr = JsonUtility.ToJson(new SignInApiData() {
            email = email,
            password = password
        });

        using var request = Post("auth/signIn", dataStr);
        await request.SendWebRequest();

        Debug.Log($"SignIn: {request.responseCode}");
    }
}