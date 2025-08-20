using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EntryPoint : MonoBehaviour {
    [SerializeField]
    private ChoosePanel _choosePanel;

    [SerializeField]
    private TabsPanel _tabsPanel;

    [SerializeField]
    private MenuTab _menuTab;

    [SerializeField]
    private CartTab _cartTab;

    [SerializeField]
    private IconsManager _iconsManager;

    private void Start() {
        _iconsManager.Init();
        StartAsync().Forget();
    }

    private async UniTask StartAsync() {
        _tabsPanel.SelectTab(TabTypes.Room);
        _choosePanel.Show();
        List<Category> categories = await PanhomeApi.GetCategories();
        _menuTab.Init(categories);
    }
}