using System;
using TMPro;
using UnityEngine;

public class CategoryButtonView : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _labelText;

    private Action _onClick;

    public void SetData(string name, Action click) {
        _labelText.text = name;
        _onClick = click;
    }

    public void Click() {
        _onClick?.Invoke();
    }
}