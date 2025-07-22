using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdminManager : MonoBehaviour {
    private void Awake() {
        UserDataManager.CreateRandomValues();
    }

    public void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        UserDataManager.CreateRandomValues();
    }

    public void SignUp() {
        AuthApi.SignUp(UserDataManager.email);
    }

    public void SignIn() {
        AuthApi.SignIn(UserDataManager.email, UserDataManager.password);
    }
}