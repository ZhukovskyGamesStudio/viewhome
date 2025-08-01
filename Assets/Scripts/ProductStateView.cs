using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductStateView : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _nameText, _descriptionText, _priceText;

    [SerializeField]
    private RawImage _icon;

    private Action<Product> _placeAction;
    private Product _product;

    public void Subscribe(Action<Product> openAction) {
        _placeAction = openAction;
    }

    public async void SetData(Product product) {
        _product = product;
        _nameText.text = product.title;
        _descriptionText.text = $"{product.description} <color=#4B946C>about the product ></color>";
        _priceText.text = $"{product.price} {product.currency}";

        _icon.texture = await ApiBase.GetPicture(product.FixedImageLink(0));
    }

    public void Place() {
        _placeAction?.Invoke(_product);
    }
}