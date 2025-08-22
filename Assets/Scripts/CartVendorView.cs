using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CartVendorView : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _headerText, _dateText;

    [SerializeField]
    private Image _vendorIcon;

    [SerializeField]
    public Transform _productsContainer;

    [SerializeField]
    private CartProductView _productPrefab;

    public Dictionary<Product, CartProductView> Products { get; private set; } = new();

    public void SetData(Vendor vendor, List<Product> products) {
        _headerText.text = vendor.ToString();
        _vendorIcon.sprite = IconsManager.Instance.Icons[vendor];

        foreach (Product product in products) {
            CartProductView productView = Instantiate(_productPrefab, _productsContainer);
            Products.Add(product, productView);
            productView.SetData(product, RemoveItem);
        }

        _dateText.text = (DateTime.Now + TimeSpan.FromDays(Random.Range(5, 15))).ToString("m");
    }

    private void RemoveItem(Product product) {
        Destroy(Products[product].gameObject);
        Products.Remove(product);
        Room.Instance.RemoveItem(product);
        if (Products.Count == 0) {
            Destroy(gameObject);
        }
    }
}