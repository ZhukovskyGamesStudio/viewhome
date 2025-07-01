using UnityEngine;
using UnityEngine.EventSystems;

public class FurnitureUIManager : MonoBehaviour {
    public static FurnitureUIManager Instance;
    public GameObject uiPrefab;
    private static GameObject currentUI;
    private Transform currentTarget;
    private Camera mainCamera;
    public float uiYOffset = 0.1f;

    void Awake() {
        Instance = this;
        mainCamera = Camera.main;
    }

    void Update() {
        if (currentUI != null && currentTarget != null) {
            // UI всегда над объектом
            Vector3 pos = currentTarget.position + Vector3.up * uiYOffset;
            currentUI.transform.position = pos;
            // UI всегда смотрит в камеру
            currentUI.transform.rotation = Quaternion.LookRotation(currentUI.transform.position - mainCamera.transform.position);
        }
        // Скрытие UI по клику вне кнопок
        if (currentUI != null && Input.GetMouseButtonDown(0)) {
            if (!IsPointerOverUI()) {
                HideUI();
            }
        }
    }

    public void ShowUI(Transform target) {
        if(target == null || target == currentTarget) return;
        HideUI();
        currentTarget = target;
        currentUI = Instantiate(uiPrefab, target.position + Vector3.up * uiYOffset, Quaternion.identity);
        var ui = currentUI.GetComponent<FurnitureUI>();
        if (ui != null) ui.SetTarget(target);
    }

    public void HideUI() {
        if (currentUI != null) Destroy(currentUI);
        currentUI = null;
        currentTarget = null;
    }

    // Проверка, наведён ли курсор на UI (кнопки)
    public static bool IsPointerOverUI() {
        if (currentUI == null) return false;
        if (!EventSystem.current) return false;
        return EventSystem.current.IsPointerOverGameObject();
    }
} 