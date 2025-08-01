using UnityEngine;

public class EntryPoint : MonoBehaviour {
    [SerializeField]
    private ChoosePanel _choosePanel;

    [SerializeField]
    private TabsPanel _tabsPanel;

    private void Start() {
        _tabsPanel.SelectTab(TabTypes.Room);
        _choosePanel.Show();
    }
}