using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadManager : MonoBehaviour {
    
    public void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void ReloadSceneWithFade() {
        // Можно добавить fade эффект перед перезагрузкой
        ReloadScene();
    }
} 