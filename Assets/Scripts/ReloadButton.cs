using UnityEngine;
using UnityEngine.UI;

public class ReloadButton : MonoBehaviour {
    private Button button;
    private ReloadManager reloadManager;

    private void Start() {
        button = GetComponent<Button>();
        reloadManager = FindObjectOfType<ReloadManager>();

        if (button != null && reloadManager != null) {
            button.onClick.AddListener(OnReloadButtonClick);
        }
    }

    private void OnReloadButtonClick() {
        if (reloadManager != null) {
            reloadManager.ReloadScene();
        }
    }

    private void OnDestroy() {
        if (button != null) {
            button.onClick.RemoveListener(OnReloadButtonClick);
        }
    }
}