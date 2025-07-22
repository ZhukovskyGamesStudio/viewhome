using UnityEngine.Networking;

public class ApiBase {
    protected static readonly string _baseUrl = "https://viewhome.simul.me/";

    protected static UnityWebRequest Get(string uri) {
        return UnityWebRequest.Get($"_baseUrl{uri}");
    }
    protected static UnityWebRequest Post(string uri, string json) {
        return UnityWebRequest.Post($"_baseUrl{uri}", json, contentType: "application/json");
    }
    
    protected static UnityWebRequest Put(string uri, string json) {
        return UnityWebRequest.Put($"_baseUrl{uri}", json);
    }
    
    protected static UnityWebRequest Delete(string uri) {
        return UnityWebRequest.Delete($"_baseUrl{uri}");
    }
}
