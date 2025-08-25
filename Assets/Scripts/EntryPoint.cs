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

    [SerializeField]
    private FtueManager _ftueManager;

    [SerializeField]
    private Animation _loadingPanel;

    [SerializeField]
    private AnimationClip _loadingEnd;

    private void Start() {
        MenuTab.Instance = _menuTab;
        _loadingPanel.gameObject.SetActive(true);
        _iconsManager.Init();
        _ftueManager.Init();
        StartAsync().Forget();
    }

    private async UniTask StartAsync() {
        _tabsPanel.SelectTab(TabTypes.Room);
        _choosePanel.Show();
        List<Category> categories = await PanhomeApi.GetCategories();
        _menuTab.Init(categories);
        _ftueManager.StartFtue();
        _loadingPanel.Play(_loadingEnd.name);
    }
}