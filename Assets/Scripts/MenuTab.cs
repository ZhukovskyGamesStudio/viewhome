using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    private void Start() {
        Init();
    }

    public void Init() {
        CreateCategories();
        CreateOffers();
        _categoriesState.SetActive(true);
        _offersState.SetActive(false);
        _infoState.SetActive(false);
      
    }

    private void CreateCategories() {
        for (int i = 0; i < _categoriesCount; i++) {
            var button = Instantiate(_categoryButtonPrefab, _categoriesGridContainer);
            int i1 = i;
            button.SetData(_categoriesNames[i1], () => SelectCategory(i1));
        }
    }

    private void CreateOffers() {
        for (int i = 0; i < _offersCount; i++) {
            var offer = Instantiate(_offerViewPrefab, _offersViewContainer);
            int i1 = i;
            offer.SetData(() => { SelectItem(i1); }, _categoriesNames[i1], Random.Range(1000,10000).ToString("C0"));
        }
    }

    private void SelectItem(int itemIndex) {
        _offersState.SetActive(false);
        _infoState.SetActive(true);
        _infoHeader.text = $"Item: {_categoriesNames[itemIndex]}";
    }

    public void SelectCategory(int category) {
        _offersState.SetActive(true);
        _categoriesState.SetActive(false);
        _categoryHeader.text = _categoriesNames[category];
    }


    public void BackToCategories() {
        _offersState.SetActive(false);
        _categoriesState.SetActive(true);
    }
    public void BackToOffers() {
        _infoState.SetActive(false);
        _offersState.SetActive(true);
    }

    public void PlaceItem() {
        Room.Instance.PlaceItem();
        BackToOffers();
        BackToCategories();
    }
}