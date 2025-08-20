using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuTab : MonoBehaviour {
    [SerializeField]
    private CategoryButtonView _categoryButtonPrefab;

    [SerializeField]
    private OfferView _offerViewPrefab;

    [SerializeField]
    private Transform _categoriesGridContainer, _offersViewContainer;

    [SerializeField]
    private int _categoriesCount = 15;

    [SerializeField]
    private int _offersCount = 10;

    [SerializeField]
    private GameObject _categoriesState, _offersState, _infoState;

    [SerializeField]
    private TextMeshProUGUI _categoryHeader, _infoHeader;

    [SerializeField]
    private ProductStateView _productStateView;

    [SerializeField]
    private List<string> _categoriesNames = new List<string> {
        "Living room",
        "Bedroom",
        "Kitchen",
        "Bathroom",
        "Guestroom",
        "Office",
        "Dining room",
        "Hallway",
        "Garage",
        "Garden",
        "Balcony",
        "Laundry room",
        "Storage room",
        "Playroom",
        "Workshop",
        "Home theater",
        "Library",
        "Gym",
        "Nursery",
        "Guest bathroom",
        "Utility room",
    };

    private PanelType _panelType;

    public void Init(List<Category> categories) {
        CreateCategories(categories);
        _categoriesState.SetActive(true);
        _offersState.SetActive(false);
        _infoState.SetActive(false);
        _productStateView.Subscribe(PlaceItem);
        _panelType = PanelType.Room;
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

    private void OnEnable() {
        _panelType = PanelType.Categories;
    }

    private void SwitchBack() {
        switch (_panelType) {
            case PanelType.Categories:
                BackToRoom();
                break;
            case PanelType.Product:
                BackToOffers();
                break;
            case PanelType.Offers:
                BackToCategories();
                break;
        }
    }

    private void CreateCategories(List<Category> categories) {
        foreach (Category t in categories) {
            CategoryButtonView button = Instantiate(_categoryButtonPrefab, _categoriesGridContainer);
            button.SetData(t, SelectCategory);
        }
    }

    private async void CreateOffers(Category category) {
        foreach (Transform child in _offersViewContainer) {
            Destroy(child.gameObject);
        }

        List<Product> products = await PanhomeApi.GetProducts(category);
        foreach (Product t in products) {
            var offer = Instantiate(_offerViewPrefab, _offersViewContainer);
            offer.SetData(t, SelectItem);
        }
    }

    public void SelectCategory(Category category) {
        _offersState.SetActive(true);
        _categoriesState.SetActive(false);
        _categoryHeader.text = category.name;
        CreateOffers(category);
        _panelType = PanelType.Offers;
    }

    private void SelectItem(Product product) {
        _offersState.SetActive(false);
        _infoState.SetActive(true);
        _infoHeader.text = $"Item: {product.title}";
        _productStateView.SetData(product);
        _panelType = PanelType.Product;
    }

    public void BackToCategories() {
        _offersState.SetActive(false);
        _categoriesState.SetActive(true);
        _panelType = PanelType.Categories;
    }

    private void BackToRoom() {
        TabsPanel.Instance.SelectTab(TabTypes.Room);
        _panelType = PanelType.Room;
    }

    public void BackToOffers() {
        _infoState.SetActive(false);
        _offersState.SetActive(true);
        _panelType = PanelType.Offers;
    }

    public void PlaceItem(Product product) {
        Room.Instance.PlaceItem(product);
        UserDataManager.UserData.CartProducts.Add(product);
        BackToOffers();
        BackToCategories();
        TabsPanel.Instance.SelectTab(TabTypes.Room);
    }
}

[Serializable]
public enum PanelType {
    Room,
    Categories,
    Offers,
    Product
}