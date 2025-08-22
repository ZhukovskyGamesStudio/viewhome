using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour {
    [SerializeField]
    private Vector3 _startingRotation;

    [SerializeField]
    private float _idealSize = 20f;
    
    [SerializeField]
    private Room _room;
    
    
    public float rotationSpeed = 0.2f;
    public float zoomSpeed = 5f;
    public float panSpeed = 0.0004f;
    public float smoothSpeed = 5f;
    public float minDistance = 3f;
    public float maxDistance = 20f;
    public Vector2 moveLimitX = new(-10f, 10f);
    public Vector2 moveLimitZ = new(-10f, 10f);
    private Vector3 target = Vector3.zero;
    private Vector3 currentTarget;
    private float distance;
    private float currentDistance;
    private float xAngle;
    private float yAngle = 20f;
    private Vector2 lastTouchPos;
    private bool isDragging;
    private Vector3 panStartTarget;
    private Vector2 panStartPos;
    private bool isPanning;
    private bool inputBlocked = false;
    private float _multiplier;

    [SerializeField]
    private Transform _roomTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start() {
        _multiplier =  Mathf.Sqrt(Mathf.Max(_room.Size.x, _room.Size.y) / _idealSize);
        Vector3 offset = transform.position - target;
        distance = offset.magnitude * _multiplier;
        distance = Mathf.Clamp(distance, minDistance  *_multiplier, maxDistance* _multiplier);
        currentDistance = distance;
        currentTarget = target;
        xAngle = Mathf.Atan2(offset.x, offset.z) * Mathf.Rad2Deg;
        _roomTransform.rotation = Quaternion.Euler(_startingRotation);
    }

    // Update is called once per frame
    private void Update() {
        if (!inputBlocked) {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            HandleMouse();
#else
            HandleTouch();
#endif
        }

        UpdateCameraPosition();
        //  Debug.Log($"IsDragging: {isDragging}, IsPanning: {isPanning}, Target: {target}, Distance: {distance}, XAngle: {xAngle}, YAngle: {yAngle}");
    }

    private void HandleMouse() {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
            isDragging = true;
            lastTouchPos = Input.mousePosition;
        } else if (Input.GetMouseButtonUp(0)) {
            isDragging = false;
        }

        // Панорамирование мышью при зажатых двух кнопках
        if (Input.GetMouseButton(0) && Input.GetMouseButton(1)) {
            if (!isPanning) {
                isPanning = true;
                panStartPos = Input.mousePosition;
                panStartTarget = target;
            } else {
                Vector2 delta = (Vector2)Input.mousePosition - panStartPos;
                PanCamera(delta);
                panStartPos = Input.mousePosition;
                panStartTarget = target;
            }
        } else {
            isPanning = false;
        }

        // Вращение и зум только если не панорамируем
        if (!isPanning) {
            if (isDragging) {
                Vector2 delta = (Vector2)Input.mousePosition - lastTouchPos;
                if (_roomTransform != null) {
                    _roomTransform.Rotate(Vector3.up, -delta.x * rotationSpeed, Space.World);
                }

                yAngle -= delta.y * rotationSpeed * 0.5f;
                yAngle = Mathf.Clamp(yAngle, 5f, 80f);
                lastTouchPos = Input.mousePosition;
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f) {
                distance -= scroll * zoomSpeed;
                distance = Mathf.Clamp(distance, minDistance * _multiplier, maxDistance* _multiplier);
            }
        }
    }

    private void HandleTouch() {
        if (Input.touchCount == 1 && !EventSystem.current.IsPointerOverGameObject(0)) {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) {
                isDragging = true;
                lastTouchPos = touch.position;
            } else if (touch.phase == TouchPhase.Moved && isDragging) {
                if (!isPanning) {
                    Vector2 delta = touch.position - lastTouchPos;
                    if (_roomTransform != null) {
                        _roomTransform.Rotate(Vector3.up, -delta.x * rotationSpeed, Space.World);
                    }

                    yAngle -= delta.y * rotationSpeed * 0.5f;
                    yAngle = Mathf.Clamp(yAngle, 5f, 80f);
                    lastTouchPos = touch.position;
                }
            } else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
                isDragging = false;
            }
        } else if (Input.touchCount == 2) {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);
            Vector2 avgPos = (t0.position + t1.position) * 0.5f;
            // Панорамирование
            if (!isPanning) {
                isPanning = true;
                panStartPos = avgPos;
                panStartTarget = target;
            } else {
                Vector2 delta = avgPos - panStartPos;
                PanCamera(delta);
                panStartPos = avgPos;
                panStartTarget = target;
            }

            // Зум pinch-жестом
            float prevDist = (t0.position - t0.deltaPosition - (t1.position - t1.deltaPosition)).magnitude;
            float currDist = (t0.position - t1.position).magnitude;
            float pinchDelta = currDist - prevDist;
            if (Mathf.Abs(pinchDelta) > 0.01f) {
                distance -= pinchDelta * zoomSpeed * 0.01f;
                distance = Mathf.Clamp(distance, minDistance  * _multiplier, maxDistance* _multiplier);
            }
        } else {
            isPanning = false;
        }
    }

    private void UpdateCameraPosition() {
        // Плавное движение к целевой позиции
        currentTarget = Vector3.Lerp(currentTarget, target, smoothSpeed * Time.deltaTime);
        currentDistance = Mathf.Lerp(currentDistance, distance, smoothSpeed * Time.deltaTime);

        Quaternion rotation = Quaternion.Euler(yAngle, 0, 0);
        Vector3 dir = rotation * Vector3.forward;
        transform.position = currentTarget + -dir * currentDistance;
        transform.LookAt(currentTarget);
    }

    private void PanCamera(Vector2 delta) {
        float panSpeedMultiplier = panSpeed * distance;
        // Простое движение в мировых координатах X и Z
        Vector3 move = new Vector3(-delta.x, 0, -delta.y) * panSpeedMultiplier;
        Vector3 newTarget = panStartTarget + move;
        newTarget.x = Mathf.Clamp(newTarget.x, moveLimitX.x, moveLimitX.y);
        newTarget.z = Mathf.Clamp(newTarget.z, moveLimitZ.x, moveLimitZ.y);
        target = new Vector3(newTarget.x, 0, newTarget.z);
    }

    public void SetInputBlocked(bool blocked) {
        inputBlocked = blocked;

        // Сбрасываем состояния при блокировке
        if (blocked) {
            isDragging = false;
            isPanning = false;
        }
    }
}