using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePanel : MonoBehaviour {
    [SerializeField]
    private GameObject _typeState, _sizeState, _confirmState;

    [SerializeField]
    private List<Toggle> _typeToggles = new();

    [SerializeField]
    private TMP_InputField _lengthInput, _widthInput;

    [SerializeField]
    private TextMeshProUGUI _typeText, _sizeText;

    private int _selectedType = 0;
    private Vector2 _selectedSize = Vector2.zero;

    private List<string> _typeNames = new() {
        "Living room",
        "Bedroom",
        "Kitchen",
        "Bathroom",
        "Guestroom"
    };

    public void Show() {
        gameObject.SetActive(true);
        _typeState.SetActive(true);
        _sizeState.SetActive(false);
        _confirmState.SetActive(false);
    }

    public void ConfirmType() {
        _selectedType = _typeToggles.FindIndex(t => t.isOn);
        _typeState.SetActive(false);
        _sizeState.SetActive(true);
    }

    public void ConfirmSize() {
        _selectedSize = new Vector2(float.TryParse(_lengthInput.text, out float length) ? length : 0,
            float.TryParse(_widthInput.text, out float width) ? width : 0);
        _sizeState.SetActive(false);
        OpenConfirm();
    }

    private void OpenConfirm() {
        _confirmState.SetActive(true);
        _typeText.text = $"Room type: {_typeNames[_selectedType]}";
        _sizeText.text = $"Room footage: {_selectedSize.x}*{_selectedSize.y}";
    }

    public void BackToParameters() {
        _confirmState.SetActive(false);
        _typeState.SetActive(true);
    }

    public void ConfirmConfirm() {
        Room.Instance.CreateRoom(_selectedSize, _selectedType);
        gameObject.SetActive(false);
    }
}