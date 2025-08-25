using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using TriLibCore;
using UnityEngine;
using Random = UnityEngine.Random;

public class Room : MonoBehaviour {
    [Header("Wall Settings")]
    public float wallAdjustSpeed = 3f;

    public float shortWallHeight = 0.5f;
    public float fullWallHeight = 3f;

    [SerializeField]
    private Transform[] _firstWalls;

    [SerializeField]
    private Transform _floor;

    [SerializeField]
    private Transform _furnitureContainer;

    [SerializeField]
    private AssetLoaderOptions _assetLoaderOptions;

    [SerializeField]
    private Material _furnitureFallbackMaterial;

    private Transform[] walls;
    private float[] targetHeights;
    private float[] currentHeights;
    private Camera mainCamera;

    [SerializeField]
    private RoomTab _roomTab;

    [SerializeField]
    private GameObject _itemsCount;

    [SerializeField]
    private TextMeshProUGUI _itemsCountText;

    public Vector2 Size { get; private set; } = Vector2.one;
    public int Type { get; private set; }

    public Dictionary<Product, GameObject> ObjectsInRoom = new();

    [SerializeField]
    private List<GameObject> _mockModels;

    public static Room Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public void CreateRoom(Vector2 size, int type) {
        Size = size;
        Type = type;
        _firstWalls[0].localScale = new Vector3(size.x, 1, 1);
        _firstWalls[1].localScale = new Vector3(size.y, 1, 1);
        _firstWalls[2].localScale = new Vector3(size.x, 1, 1);
        _firstWalls[3].localScale = new Vector3(size.y, 1, 1);

        _firstWalls[0].localPosition = new Vector3(0, 0, size.y / 2);
        _firstWalls[1].localPosition = new Vector3(size.x / 2, 0, 0);
        _firstWalls[2].localPosition = new Vector3(0, 0, -size.y / 2);
        _firstWalls[3].localPosition = new Vector3(-size.x / 2, 0, 0);
        _floor.localScale = new Vector3(size.x + 1, 1, size.y + 1);
        _roomTab.UpdateParameters(type, size);
    }

    private void Start() {
        mainCamera = Camera.main;
        if (mainCamera == null) {
            mainCamera = FindObjectOfType<Camera>();
        }

        // Находим все стены (предполагаем, что они имеют тег "Wall" или содержат "wall" в имени)
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        List<Transform> wallList = new();

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

        UpdateCost();
        UpdateCount();
    }

    private void Update() {
        if (mainCamera == null || walls == null) {
            return;
        }

        for (int i = 0; i < walls.Length; i++) {
            UpdateWallHeight(i);
        }
    }

    private void UpdateWallHeight(int wallIndex) {
        Transform wall = walls[wallIndex];
        if (wall == null) {
            return;
        }

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

    public void PlaceItem(Product product) {
        PlaceItemAsync(product).Forget();
    }

    private async UniTask PlaceItemAsync(Product product) {
        if (string.IsNullOrEmpty(product.modelId)) {
            Debug.LogError($"ModelId is empty {product.title}");
            var go0 = CreateRandomMockModel(product);
            AddItem(product, go0, false);
            return;
        }

        Model modelData = await ModelControllerApi.GetModel(product.modelId);
        if (modelData == null || string.IsNullOrEmpty(modelData.model)) {
            Debug.LogError($"No model found {modelData}");
            var go1 = CreateRandomMockModel(product);
            AddItem(product, go1, false);
            return;
        }

        string url = modelData.model;
        bool isZip = url.EndsWith(".zip", StringComparison.OrdinalIgnoreCase);
        if (isZip) {
            UnityEngine.Networking.UnityWebRequest webRequest = AssetDownloader.CreateWebRequest(url);
            var go = await AssetDownloader.LoadModelFromUri(webRequest, OnLoad, OnMaterialsLoad, OnProgress, OnError,
                _furnitureContainer.gameObject, _assetLoaderOptions, isZipFile: true, fileExtension: "fbx");
            AddItem(product, go);
            return;
        }

        string localPath = await ApiBase.GetModelObject(url);
        if (string.IsNullOrEmpty(localPath)) {
            Debug.LogError($"No model found on path {localPath}");
            var go1 = CreateRandomMockModel(product);
            AddItem(product, go1, false);
            return;
        }

        GameObject go2 = await AssetLoader.LoadModelFromFile(localPath, OnLoad, OnMaterialsLoad, OnProgress, OnError,
            _furnitureContainer.gameObject, _assetLoaderOptions);
        if (go2 != null) {
            AddItem(product, go2);
        }
    }

    private void AddItem(Product product, GameObject go, bool isFixing = true) {
        if (isFixing) {
            TryFixScaleAndMaterial(go);
        }

        ObjectsInRoom.Add(product, go);
        UpdateCount();
        UpdateCost();
    }

    private void UpdateCount() {
        _itemsCount.gameObject.SetActive(ObjectsInRoom.Count > 0);
        _itemsCountText.text = $"{ObjectsInRoom.Count}";
    }

    private void UpdateCost() {
        float totalCost = ObjectsInRoom.Sum(o => float.Parse(o.Key.price, CultureInfo.InvariantCulture));
        _roomTab.UpdateCost(totalCost);
    }

    public void RemoveItem(Product product) {
        var item = ObjectsInRoom.Keys.FirstOrDefault(k => k.productId == product.productId);
        if (item == null) {
            return;
        }

        Destroy(ObjectsInRoom[item]);
        ObjectsInRoom.Remove(item);
        UpdateCost();
        UpdateCount();
    }

    private void TryFixScaleAndMaterial(GameObject res) {
        res.gameObject.layer = LayerMask.NameToLayer("Furniture");
        var meshFilter = res.gameObject.GetComponent<MeshFilter>();
        var meshCollider = res.gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = meshFilter.sharedMesh;
        res.transform.localScale = Vector3.one;
        var resRenderers = res.GetComponent<Renderer>();
        if (resRenderers.material == null) {
            resRenderers.material = _furnitureFallbackMaterial;
        }
    }

    private GameObject CreateRandomMockModel(Product product) {
        Random.InitState(product.productId.GetHashCode());
        GameObject mock = _mockModels[Random.Range(0, _mockModels.Count)];
        Vector3 pos = new(0, mock.transform.position.y, 0);
        GameObject furniture = Instantiate(mock, pos, mock.transform.rotation, _furnitureContainer);
        furniture.gameObject.SetActive(true);
        return furniture;
    }

    public Product GetProductByObject(GameObject obj) {
        return ObjectsInRoom.First(kvp => kvp.Value == obj).Key;
    }

    private void OnError(IContextualizedError error) {
        Debug.LogError(error.GetInnerException());
    }

    private void OnProgress(AssetLoaderContext context, float progress) { }

    private void OnMaterialsLoad(AssetLoaderContext context) { }

    private void OnLoad(AssetLoaderContext context) { }
}