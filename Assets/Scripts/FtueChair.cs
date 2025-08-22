using System;
using UnityEngine;

public class FtueChair : MonoBehaviour {
    private Action _onClick;

    public void Init(Action onClick) {
        _onClick = onClick;
    }

    private void OnMouseDown() {
        _onClick?.Invoke();
    }
}