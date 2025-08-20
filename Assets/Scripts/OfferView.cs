using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfferView : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _nameText, _descriptionText, _priceText;

    [SerializeField]
    private Image _vendorIcon;
    
    [SerializeField]
    private RawImage _icon;

    private Action<Product> _openAction;
    private Product _product;

    public async void SetData(Product product, Action<Product> onOpen) {
        _product = product;
        _vendorIcon.sprite = IconsManager.Instance.Icons[product.Vendor];
        _nameText.text = product.title;
        _descriptionText.text = product.description;
        _priceText.text = $"{product.price} {product.currency}";
        _openAction = onOpen;

        _icon.texture = await ApiBase.GetPicture(product.FixedImageLink(0));
    }

    public void OpenItem() {
        _openAction?.Invoke(_product);
    }
}