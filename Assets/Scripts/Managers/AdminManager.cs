using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdminManager : MonoBehaviour {

    [SerializeField]
    private RawImage _rawImage;
    
    private void Awake() {
        UserDataManager.CreateRandomValues();
    }

    public void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        UserDataManager.CreateRandomValues();
    }

    public void SignUp() {
        AuthApi.SignUp(UserDataManager.email);
    }

    public void SignIn() {
        AuthApi.SignIn(UserDataManager.email, UserDataManager.password);
    }

    public void GetModel() {
        GetModelAsync().Forget();
    }

    private async UniTask GetModelAsync() {
        Model model = await ModelControllerApi.GetModel(ApiMocksIds.ModelMockId);
        if (model == null) {
            return;
        }

        Debug.Log($"GetModel: {model.title} {model.price} {model.pic} {model.model}");
        
        var picture = await ApiBase.GetPicture(ApiMocksIds.DownloadPictureMock);
        if (picture != null) {
            _rawImage.texture = picture;
        }
        
        var saveModelPath = await ApiBase.GetModel(ApiMocksIds.DownloadModelMock);
        if (saveModelPath != null) {
            
        }
        
    }
}