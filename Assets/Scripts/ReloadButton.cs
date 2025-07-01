using UnityEngine;
using UnityEngine.UI;

public class ReloadButton : MonoBehaviour {
    
    private Button button;
    private ReloadManager reloadManager;
    
    void Start() {
        button = GetComponent<Button>();
        reloadManager = FindObjectOfType<ReloadManager>();
        
        if (button != null && reloadManager != null) {
            button.onClick.AddListener(OnReloadButtonClick);
        }
    }
    
    void OnReloadButtonClick() {
        if (reloadManager != null) {
            reloadManager.ReloadScene();
        }
    }
    
    void OnDestroy() {
        if (button != null) {
            button.onClick.RemoveListener(OnReloadButtonClick);
        }
    }
} 