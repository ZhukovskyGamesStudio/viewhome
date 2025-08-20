using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CartTab : MonoBehaviour {
    [SerializeField]
    private CartVendorView _vendorPrefab;

    [SerializeField]
    private Transform _cartContainer;

    private void OnEnable() {
        SetData(UserDataManager.UserData.CartProducts);
    }

    public void SetData(List<Product> products) {
        Vendor vendor = Vendor.Wildberries;
        var possibleVendors = Enum.GetValues(typeof(Vendor)).Cast<Vendor>();

        foreach (Transform child in _cartContainer.transform) {
            Destroy(child.gameObject);
        }

        foreach (var vendorType in possibleVendors) {
            var productsOfVendor = products.Where(v => v.Vendor == vendorType).ToList();
            if (productsOfVendor.Count > 0) {
                var vendorView = Instantiate(_vendorPrefab, _cartContainer);
                vendorView.SetData(vendorType, productsOfVendor);
            }
        }
    }
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SwitchBack();
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.RightBracket)) {
            SwitchBack();
        }
#endif
    }
    
    
    private void SwitchBack() {
        BackToRoom();
    }
    
    public void BackToRoom() {
        TabsPanel.Instance.SelectTab(TabTypes.Room);
    }

}