using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class IconsManager : MonoBehaviour {
    [SerializedDictionary]
    public SerializedDictionary<Vendor, Sprite> Icons;

    public static IconsManager Instance;

    public void Init() {
        Instance = this;
    }
}