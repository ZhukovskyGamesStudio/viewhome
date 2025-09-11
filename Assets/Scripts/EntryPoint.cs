using System.Collections.Generic;
using System.Linq;
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

    [SerializeField]
    private ModelScalesConfig _modelScalesConfig;

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
        PrintReceivedCategories(categories);
        var filtered = FilterAllowedCategories(categories, _modelScalesConfig);
        _menuTab.Init(filtered);
        _ftueManager.StartFtue();
        _loadingPanel.Play(_loadingEnd.name);
    }

    private static void PrintReceivedCategories(List<Category> categories) {
        string combined = "";
        foreach (var cat in categories) {
            combined += cat.name + "\n";
        }

        Debug.Log(combined);
    }

    private static List<Category> FilterAllowedCategories(List<Category> categories, ModelScalesConfig modelScalesConfig) {
        List<Category> res = categories.Where(cat => modelScalesConfig.AllowedCategories.Contains(cat.name)).ToList();
        return res;
    }
}