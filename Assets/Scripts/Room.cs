using System;
using UnityEngine;

public class Room : MonoBehaviour {
    [Header("Wall Settings")]
    public float wallAdjustSpeed = 3f;
    public float shortWallHeight = 0.5f;
    public float fullWallHeight = 3f;
    
    [SerializeField]
    private Transform[] _firstWalls;

    [SerializeField]
    private Transform _floor;
    
    
    private Transform[] walls;
    private float[] targetHeights;
    private float[] currentHeights;
    private Camera mainCamera;

    public static Room Instance { get; private set; }
    
    private void Awake() {
        Instance = this;
    }

    public void CreateRoom(Vector2 size, int type) {
        _firstWalls[0].localScale = new Vector3(size.x, 1, 1);
        _firstWalls[1].localScale = new Vector3(size.y, 1, 1);
        _firstWalls[2].localScale = new Vector3(size.x, 1, 1);
        _firstWalls[3].localScale = new Vector3(size.y, 1, 1);

        _firstWalls[0].localPosition = new Vector3(0, 0, size.y / 2);
        _firstWalls[1].localPosition = new Vector3(size.x / 2, 0, 0);
        _firstWalls[2].localPosition = new Vector3(0, 0, -size.y / 2);
        _firstWalls[3].localPosition = new Vector3(-size.x / 2, 0, 0);
        _floor.localScale = new Vector3(size.x + 1, 1, size.y + 1);
    }
    
    void Start() {
        mainCamera = Camera.main;
        if (mainCamera == null) {
            mainCamera = FindObjectOfType<Camera>();
        }
        
        // Находим все стены (предполагаем, что они имеют тег "Wall" или содержат "wall" в имени)
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        System.Collections.Generic.List<Transform> wallList = new System.Collections.Generic.List<Transform>();
        
        foreach (Transform child in allChildren) {
            if (child != transform && (child.name.ToLower().Contains("wall") || child.CompareTag("Wall"))) {
                wallList.Add(child);
            }
        }
        
        walls = wallList.ToArray();
        targetHeights = new float[walls.Length];
        currentHeights = new float[walls.Length];
        
        // Инициализируем высоты
        for (int i = 0; i < walls.Length; i++) {
            currentHeights[i] = walls[i].localScale.y;
            targetHeights[i] = currentHeights[i];
        }
    }
    
    void Update() {
        if (mainCamera == null || walls == null) return;
        
        for (int i = 0; i < walls.Length; i++) {
            UpdateWallHeight(i);
        }
    }
    
    void UpdateWallHeight(int wallIndex) {
        Transform wall = walls[wallIndex];
        if (wall == null) return;
        
        // Определяем, с какой стороны камера смотрит на стену
        Vector3 wallPosition = wall.position;
        Vector3 cameraPosition = mainCamera.transform.position;
        Vector3 wallForward = wall.forward;
        
        // Вектор от стены к камере
        Vector3 toCamera = cameraPosition - wallPosition;
        
        // Определяем, с какой стороны камера (внутренняя или внешняя)
        float dotProduct = Vector3.Dot(wallForward, toCamera);
        bool isCameraInside = dotProduct > 0; // Камера с внутренней стороны стены
        
        // Устанавливаем целевую высоту
        targetHeights[wallIndex] = isCameraInside ? fullWallHeight : shortWallHeight;
        
        // Плавно изменяем высоту через Lerp
        currentHeights[wallIndex] = Mathf.Lerp(currentHeights[wallIndex], targetHeights[wallIndex], wallAdjustSpeed * Time.deltaTime);
        
        // Применяем новую высоту к стене, сохраняя основание на месте
        Vector3 newScale = wall.localScale;
        float heightDifference = currentHeights[wallIndex] - newScale.y;
        newScale.y = currentHeights[wallIndex];
        wall.localScale = newScale;
        
        // Сдвигаем позицию вверх на половину изменения высоты, чтобы основание осталось на месте
        Vector3 newPosition = wall.position;
        newPosition.y += heightDifference * 0.5f;
        wall.position = newPosition;
    }

    public void PlaceItem() {
        
    }
}