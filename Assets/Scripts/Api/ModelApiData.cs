using System;

[Serializable]
public class ModelApiData {
    public Data data;
}

[Serializable]
public class Data {
    public Model model;
}

[Serializable]
public class Model {
    public string modelId;
    public string categoryId;
    public string title;
    public string price;
    public string currency;
    public int length;
    public int height;
    public int width;
    public float[] centerAnchorGeo; // [longitude, latitude, altitude]
    public float[] centerAnchorFloor; // [x, y]
    public string model; // URL to the 3D model
    public string pic; // URL to the preview image
    public string resourceId;
    public DateTime createdAt;
    public DateTime updatedAt;
}