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
        ApiBase.SignUp(UserDataManager.email);
    }

    public void SignIn() {
        ApiBase.SignIn(UserDataManager.email, UserDataManager.password);
    }
}