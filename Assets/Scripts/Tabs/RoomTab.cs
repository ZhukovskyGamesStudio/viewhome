using TMPro;
using UnityEngine;

public class RoomTab : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI _roomType, _roomSize;

    [SerializeField]
    private TextMeshProUGUI _roomCost;

    public void EditRoom() { }

    public void UpdateCost(float roomCost) {
        _roomCost.text = "Total cost: " + roomCost.ToString("F");
    }

    public void UpdateParameters(int roomType, Vector2 roomSize) {
        _roomType.text = ChoosePanel.TypeNames[roomType];
        _roomSize.text = $"{roomSize.x}x{roomSize.y}";
    }
}