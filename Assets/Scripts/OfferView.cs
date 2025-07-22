using System;
using TMPro;
using UnityEngine;

public class OfferView : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _nameText, _priceText;

    private Action _openAction;

    public void SetData(Action onOpen, string name, string price) {
        _nameText.text = name;
        _priceText.text = price;
        _openAction = onOpen;
    }

    public void OpenItem() {
        _openAction?.Invoke();
    }
}