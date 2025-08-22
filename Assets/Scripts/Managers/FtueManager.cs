using System;
using UnityEngine;

public class FtueManager : MonoBehaviour {
    [SerializeField]
    private FtueChair _ftueChairPrefab;

    [SerializeField]
    private Transform _roomContainer;

    [SerializeField]
    private GameObject _ftueShadow, _menuToggle, _cartToggle;
    

    private FtueChair _ftueChair;

    private void Start() {
        _ftueChair = Instantiate(_ftueChairPrefab, _roomContainer);
        _ftueChair.Init(OnChairClicked);
        _ftueShadow.SetActive(true);
        _menuToggle.SetActive(false);
        _cartToggle.SetActive(false);
    }

    private void OnChairClicked() {
        _cartToggle.SetActive(true);
        _menuToggle.SetActive(true);
        TabsPanel.Instance.SelectTab(TabTypes.Menu);
        Destroy(_ftueShadow);
        Destroy(_ftueChair.gameObject);
    }
}