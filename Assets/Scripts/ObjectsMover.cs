using UnityEngine;

public class ObjectsMover : MonoBehaviour {
    [Header("Drag Settings")]
    public float holdTimeToActivate = 1f;
    public LayerMask furnitureLayerMask = -1;
    public float dragSpeed = 1f;
    public float liftHeight = 0.2f;
    public float liftSpeed = 8f;
    public float baseY = 0f;
    
    // Ссылка на CameraController для блокировки управления
    private CameraController cameraController;
    private Camera mainCamera;
    private GameObject selectedObject;
    private bool isDragging = false;
    private float holdTimer = 0f;
    private Vector3 dragOffset;
    private Vector3 originalPosition;
    private float startY;
    
    void Start() {
        mainCamera = Camera.main;
        if (mainCamera == null) {
            mainCamera = FindObjectOfType<Camera>();
        }
        
        cameraController = FindObjectOfType<CameraController>();
    }
    
    void Update() {
        HandleMouseInput();
    }
    
    void HandleMouseInput() {
        if (Input.GetMouseButtonDown(0)) {
            // Начало нажатия
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, furnitureLayerMask)) {
                selectedObject = hit.collider.gameObject;
                originalPosition = selectedObject.transform.position;
                startY = selectedObject.transform.position.y;
                holdTimer = 0f;
                isDragging = false;
            }
        }
        
        if (Input.GetMouseButton(0) && selectedObject != null&& !FurnitureUIManager.IsPointerOverUI()) {
            holdTimer += Time.deltaTime;
            
            if (holdTimer >= holdTimeToActivate && !isDragging) {
                // Активируем drag&drop
                isDragging = true;
                StartDragging();
            }
            
            if (isDragging) {
                // Перемещаем объект
                UpdateDragPosition();
            }
        }
        
        if (Input.GetMouseButtonUp(0)) {
            // Если drag&drop не начался — это был клик, показываем UI
            if (selectedObject != null && !isDragging) {
                FurnitureUIManager.Instance.ShowUI(selectedObject.transform);
            }
            // Завершение drag&drop
            if (isDragging) {
                EndDragging();
            }
            selectedObject = null;
            isDragging = false;
            holdTimer = 0f;
        }
    }
    
    void StartDragging() {
        // Вычисляем смещение от центра объекта до точки клика
        Handheld.Vibrate();
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, furnitureLayerMask)) {
            dragOffset = selectedObject.transform.position - hit.point;
        }
        
        // Блокируем управление камерой
        if (cameraController != null) {
            cameraController.SetInputBlocked(true);
        }
    }
    
    void UpdateDragPosition() {
        if (selectedObject == null) return;
        
        // Получаем позицию мыши в мировых координатах на плоскости Y = baseY
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, baseY);
        float distance;
        
        if (plane.Raycast(ray, out distance)) {
            Vector3 targetPosition = ray.GetPoint(distance) + dragOffset;
            // Фиксируем высоту на baseY + liftHeight
            targetPosition.y = Mathf.Lerp(selectedObject.transform.position.y, baseY+ startY + liftHeight, liftSpeed * Time.deltaTime);
            // Плавно перемещаем объект
            Vector3 newPosition = Vector3.Lerp(selectedObject.transform.position, targetPosition, dragSpeed * Time.deltaTime);
            selectedObject.transform.position = newPosition;
        }
    }
    
    void EndDragging() {
        // Возвращаем объект на исходную высоту
        if (selectedObject != null) {
            Vector3 pos = selectedObject.transform.position;
            pos.y = startY;
            selectedObject.transform.position = pos;
        }
        
        // Разблокируем управление камерой
        if (cameraController != null) {
            cameraController.SetInputBlocked(false);
        }
    }
    
    // Метод для получения прогресса удержания (0-1)
    public float GetHoldProgress() {
        if (selectedObject == null) return 0f;
        return Mathf.Clamp01(holdTimer / holdTimeToActivate);
    }
    
    // Метод для проверки, активен ли drag&drop
    public bool IsDragging() {
        return isDragging;
    }
} 