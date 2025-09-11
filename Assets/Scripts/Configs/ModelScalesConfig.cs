using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ModelScalesConfig", menuName = "Scriptable Objects/ModelScalesConfig")]
public class ModelScalesConfig : ScriptableObject {
    public AYellowpaper.SerializedCollections.SerializedDictionary<string, Vector3> UrlScaleDict = new();
    public List<string> AllowedCategories = new();
}