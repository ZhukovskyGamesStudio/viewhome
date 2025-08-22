using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabsPanel : MonoBehaviour {
    [SerializeField]
    private List<Toggle> _tabsToggles;

    [SerializeField]
    private GameObject _menuTab;

    [SerializeField]
    private CanvasGroup _menuCanvasGroup;

    public static TabsPanel Instance;

    private void Awake() {
        Instance = this;
    }

    public void SelectTab(TabTypes tab) {
        _tabsToggles[(int)tab].isOn = true;
    }

    public void SetMenuToggle(bool isOn) {
        _menuCanvasGroup.blocksRaycasts = isOn;
        _menuCanvasGroup.alpha = isOn ? 1 : 0;
    }


public void SelectMenu() {
        SelectTab(TabTypes.Menu);
    }

    public void SelectRoom() {
        SelectTab(TabTypes.Room);
    }

    public void SelectCart() {
        SelectTab(TabTypes.Cart);
    }
}

[Serializable]
public enum TabTypes {
    Room = 0,
    Menu = 1,
    Cart = 2,
    Profile = 3
}