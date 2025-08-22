using System;
using UnityEngine;

public class FtueManager : MonoBehaviour {
    [SerializeField]
    private FtueChair _ftueChairPrefab;

    [SerializeField]
    private Transform _roomContainer;

    [SerializeField]
    private GameObject _ftueShadow, _cartToggle, _tabsPanel, _changeRoomButton;
    

    private FtueChair _ftueChair;

    private void Start() {
        _ftueChair = Instantiate(_ftueChairPrefab, _roomContainer);
        _ftueChair.Init(OnChairClicked);
        _ftueShadow.SetActive(true);
     
        _cartToggle.SetActive(false);
        _changeRoomButton.SetActive(false);
        _tabsPanel.SetActive(false);
    }

    private void OnChairClicked() {
        _cartToggle.SetActive(true);
        _changeRoomButton.SetActive(true);
        _tabsPanel.SetActive(true);
        TabsPanel.Instance.SelectTab(TabTypes.Menu);
        Destroy(_ftueShadow);
        Destroy(_ftueChair.gameObject);
    }
}