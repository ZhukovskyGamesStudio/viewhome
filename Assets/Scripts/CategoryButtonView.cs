using System;
using TMPro;
using UnityEngine;

public class CategoryButtonView : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _labelText;

    private Action<Category> _onClick;
    private Category _category;
    public void SetData(Category category, Action<Category> click) {
        _category = category;
        _labelText.text = category.name;
        _onClick = click;
    }

    public void Click() {
        _onClick?.Invoke(_category);
    }
}