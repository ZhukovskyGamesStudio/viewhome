using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CartVendorView : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _headerText;

    [SerializeField]
    public Transform _productsContainer;

    [SerializeField]
    private CartProductView _productPrefab;

    public List<CartProductView> Products { get; private set; } = new();

    public void SetData(Vendor vendor, List<Product> products) {
        _headerText.text = vendor.ToString();
        foreach (Product product in products) {
            CartProductView productView = Instantiate(_productPrefab, _productsContainer);
            Products.Add(productView);
            productView.SetData(product);
        }
    }
}