using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabsPanel : MonoBehaviour {
    [SerializeField]
    private List<Toggle> _tabsToggles;

    [SerializeField]
    private GameObject _menuTab;

    public static TabsPanel Instance;

    private void Awake() {
        Instance = this;
    }

    public void SelectTab(TabTypes tab) {
        _menuTab.gameObject.SetActive(tab != TabTypes.Menu);
        _tabsToggles[(int)tab].isOn = true;
    }
}

[Serializable]
public enum TabTypes {
    Room = 0,
    Menu = 1,
    Cart = 2,
    Profile = 3
}