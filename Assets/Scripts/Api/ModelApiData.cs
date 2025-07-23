using System;

[Serializable]
public class ModelApiData {
    
    //fill class like this json
  
    public Data data;
   
    /*
     * {
    "model": {
      "modelId": "15a1b24c-e504-4c2f-a156-11e3e5ce433f",
      "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "title": "Современный диван",
      "price": "2999.99",
      "currency": "RUB",
      "length": 2000,
      "height": 850,
      "width": 950,
      "centerAnchorGeo": [
        37.6176,
        55.7558,
        55.7558
      ],
      "centerAnchorFloor": [
        150,
        75
      ],
      "model": "https://storage.example.com/models/sofa.glb",
      "pic": "https://storage.example.com/previews/sofa.jpg",
      "resourceId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "createdAt": "2025-07-23T02:33:34.069Z",
      "updatedAt": "2025-07-23T02:33:34.069Z"
    }
  }
     */
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
