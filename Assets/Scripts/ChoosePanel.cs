using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePanel : MonoBehaviour {
    [SerializeField]
    private GameObject _typeState, _sizeState, _confirmState;

    [SerializeField]
    private List<Toggle> _typeToggles = new List<Toggle>();

    [SerializeField]
    private TMP_InputField _lengthInput, _widthInput;

    private int _selectedType = 0;
    private Vector2 _selectedSize = Vector2.zero;

    public void ConfirmType() {
        _selectedType = _typeToggles.FindIndex(t => t.isOn);
        _typeState.SetActive(false);

        if (_selectedSize == Vector2.zero) {
            _sizeState.SetActive(true);
        } else {
            _confirmState.SetActive(true);
        }
    }

    public void ConfirmSize() {
        _selectedSize = new Vector2(float.TryParse(_lengthInput.text, out float length) ? length : 0,
            float.TryParse(_widthInput.text, out float width) ? width : 0);
        _sizeState.SetActive(false);
        _confirmState.SetActive(true);
    }

    public void ConfirmConfirm() {
        Room.Instance.CreateRoom(_selectedSize, _selectedType);
        gameObject.SetActive(false);
    }
}