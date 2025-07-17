using UnityEngine;

public class FurnitureUI : MonoBehaviour {
    public float rotateSpeed = 60f;
   
    private Transform target;
    private bool rotateLeft;
    private bool rotateRight;

    public void SetTarget(Transform t) {
        target = t;
    }

    void Update() {
        if (target == null) return;
        if (rotateLeft)
            target.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime, Space.World);
        if (rotateRight)
            target.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }

    void OnDisable() {
        rotateLeft = false;
        rotateRight = false;
    }

    public void OnLeftDown() {
        rotateLeft = true;
    }

    public void OnLeftUp() {
        rotateLeft = false;
    }

    public void OnRightDown() {
        rotateRight = true;
    }

    public void OnRightUp() {
        rotateRight = false;
    }
}