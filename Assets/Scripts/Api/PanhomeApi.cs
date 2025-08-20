using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class PanhomeApi : ApiBase {
    public static async UniTask<List<Category>> GetCategories() {
        using UnityWebRequest request = Get($"panHome/categories?sourceName=Pan%20Home&limit=30&page=1");
        await request.SendWebRequest();

        Debug.Log($"GetCategories: {request.responseCode}");
        if (request.responseCode == 200) {
            Debug.Log($"GetCategories data: {request.downloadHandler.text}");
            Categories data = JsonUtility.FromJson<CategoriesData>(request.downloadHandler.text).data;
            return data.items;
        }

        return null;
    }

    public static async UniTask<List<Product>> GetProducts(Category category) {
        using UnityWebRequest request = Get($"panHome/products?category={category.categoryId}&limit=25&page=1");
        await request.SendWebRequest();

        Debug.Log($"GetProducts: {request.responseCode}");
        if (request.responseCode == 200) {
            Debug.Log($"GetProducts data: {request.downloadHandler.text}");
            Products data = JsonUtility.FromJson<ProductsData>(request.downloadHandler.text).data;
            foreach (Product product in data.items) {
                product.InitRandomValues();
            }

            if (AdminManager.IsShowOnlyWithModels) {
                var res = data.items.Where(p => !string.IsNullOrEmpty(p.modelId)).ToList();
                Debug.Log($"GetProducts with models: {JsonUtility.ToJson(res)} ");
                return res;
            }

            return data.items;
        }

        return null;
    }
}

[Serializable]
public class CategoriesData {
    public Categories data;
}

[Serializable]
public class Categories {
    public List<Category> items;
}

[Serializable]
public class Category {
    public string categoryId;
    public string sourceName;
    public string name;
}

[Serializable]
public class ProductsData {
    public Products data;
}

[Serializable]
public class Products {
    public List<Product> items;
}

[Serializable]
public class Product {
    public Guid productId;
    public Vendor Vendor;

    public string articleId;
    public string sourceId;
    public string modelId;
    public string title;
    public string description;
    public string price;
    public string currency;
    public List<string> images;

    public void InitRandomValues() {
        productId = Guid.NewGuid();
        Random.InitState(productId.GetHashCode());
        string[] values = Enum.GetNames(typeof(Vendor));
        Vendor = Enum.Parse<Vendor>(values[Random.Range(0, values.Length - 1)]);
    }

    public string FixedImageLink(int i) {
        return $"{images[i]}&{images[i + 1]}&{images[i + 2]}&{images[i + 3]}";
    }
}


[Serializable]
public enum Vendor {
    Unknown = -1,
    Wildberries = 0,
    YandexMarket,
    Ozon,
    Avito
}

/*
{
        "articleId": "112AIC9902662",
        "sourceId": "Pan Home",
        "modelId": "",
        "title": "Evelyn Fibre Glass Vase 42x42x120cm- Black",
        "description": "",
        "images": [
          "https://cdn.panhomestores.com/cdn-cgi/image/width=525px",
          "quality=70",
          "%20format=auto",
          "%20dpr=1/media/catalog/product/1/1/112AIC9902662_04_1.jpg",
          "https://cdn.panhomestores.com/cdn-cgi/image/width=525px",
          "quality=70",
          "%20format=auto",
          "%20dpr=1/media/catalog/product/1/1/112AIC9902662_01_4.jpg",
          "https://cdn.panhomestores.com/cdn-cgi/image/width=525px",
          "quality=70",
          "%20format=auto",
          "%20dpr=1/media/catalog/product/1/1/112AIC9902662_02_3.jpg",
          "https://cdn.panhomestores.com/cdn-cgi/image/width=525px",
          "quality=70",
          "%20format=auto",
          "%20dpr=1/media/catalog/product/1/1/112AIC9902662_03_2.jpg",
          "https://cdn.panhomestores.com/cdn-cgi/image/width=200px",
          "quality=70",
          "%20format=auto",
          "%20dpr=1/media/catalog/product/1/1/112AIC9902664_01_4.jpg",
          "https://cdn.panhomestores.com/cdn-cgi/image/width=200px",
          "quality=70",
          "%20format=auto",
          "%20dpr=1/media/catalog/product/1/1/112AIC9902663_6.jpg",
          "https://cdn.panhomestores.com/cdn-cgi/image/width=200px",
          "quality=70",
          "%20format=auto",
          "%20dpr=1/media/catalog/product/2/0/20250719_1347_mulled_berries_candle_remix_01k0h16ynrfjwbyta7ptpev237.png",
          "https://cdn.panhomestores.com/cdn-cgi/image/width=200px",
          "quality=70",
          "%20format=auto",
          "%20dpr=1/media/catalog/product/1/1/112BIX9900093_4.jpg",
          "https://cdn.panhomestores.com/cdn-cgi/image/width=200px",
          "quality=70",
          "%20format=auto",
          "%20dpr=1/media/catalog/product/1/1/112ATM9900006_3.jpg",
          "https://cdn.panhomestores.com/cdn-cgi/image/width=200px",
          "quality=70",
          "%20format=auto",
          "%20dpr=1/media/catalog/product/1/1/112ORC9900095_2.jpg",
          "https://cdn.panhomestores.com/cdn-cgi/image/width=200px",
          "quality=70",
          "%20format=auto",
          "%20dpr=1/media/catalog/product/1/1/111STV9900038_2.jpg",
          "https://cdn.panhomestores.com/cdn-cgi/image/width=200px",
          "quality=70",
          "%20format=auto",
          "%20dpr=1/media/catalog/product/1/1/112aic9902062.jpg",
          "https://cdn.panhomestores.com/cdn-cgi/image/width=200px",
          "quality=70",
          "%20format=auto",
          "%20dpr=1/media/catalog/product/1/1/112PRH9900150_5.jpg",
          "https://cdn.panhomestores.com/cdn-cgi/image/width=200px",
          "quality=70",
          "%20format=auto",
          "%20dpr=1/media/catalog/product/1/1/111AIC9902777_5.jpg",
          "https://cdn.panhomestores.com/cdn-cgi/image/width=200px",
          "quality=70",
          "%20format=auto",
          "%20dpr=1/media/catalog/product/2/2/222AIC9901999_6.jpg",
          "https://cdn.panhomestores.com/cdn-cgi/image/width=200px",
          "quality=70",
          "%20format=auto",
          "%20dpr=1/media/catalog/product/1/1/111SGO9900191_4.jpg"
        ],
        "categories": [
          "51b6bb38-d7ac-464b-ba01-896391e702dd",
          "e2f0b905-c2de-413a-914c-8a96f35011e5",
          "99e8511d-e42c-4c03-88ff-6b887821c13e",
          "73443005-dc29-40b3-9755-83ea360a3139"
        ],
        "price": "399.00",
        "currency": "AED",
        "dimensions": {
          "unit": "mm",
          "length": 420,
          "width": 420,
          "height": 1200
        },
        "weight": {
          "value": 10,
          "unit": "kg"
        },
        "specs": [
          {
            "Color": "Black"
          },
          {
            "Care Instruction": "Wipe Clean"
          },
          {
            "Features": "Shock Proof, Light Weight, Durable & Sturdy, Easy Care"
          },
          {
            "Material": "Fiberglass"
          }
        ],
        "model": null
      },*/