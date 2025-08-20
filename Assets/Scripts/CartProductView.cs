using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CartProductView : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _nameText, _descriptionText, _priceText;

    [SerializeField]
    private RawImage _icon;

    private Action<Product> _deleteAction;
    private Product _product;

    public async void SetData(Product product, Action<Product> onDelete) {
        _product = product;
        _nameText.text = product.title;
        _descriptionText.text = product.description;
        _priceText.text = $"{product.price} {product.currency}";
        _deleteAction = onDelete;

        _icon.texture = await ApiBase.GetPicture(product.FixedImageLink(0));
    }

    public void Delete() {
        _deleteAction?.Invoke(_product);
    }
}