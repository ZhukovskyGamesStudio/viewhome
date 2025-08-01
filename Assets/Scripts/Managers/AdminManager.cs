using System;
using Cysharp.Threading.Tasks;
using Dummiesman;
using TriLibCore;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdminManager : MonoBehaviour {

    [SerializeField]
    private RawImage _rawImage;

    [SerializeField]
    private GameObject _modelContainer;
    
    [SerializeField]
    private AssetLoaderOptions _assetLoaderOptions;

    [SerializeField]
    private string _modelFilePath;
    
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

        if (string.IsNullOrEmpty(_modelFilePath)) {
            var webRequest = AssetDownloader.CreateWebRequest(ApiMocksIds.DownloadModelFbxBed2ZipMock);
            AssetDownloader.LoadModelFromUri(webRequest, OnLoad, OnMaterialsLoad, OnProgress, OnError, _modelContainer, _assetLoaderOptions,
                isZipFile: true, fileExtension: "fbx");  
        } else {
            _modelFilePath = _modelFilePath.Replace("\"", "");
            AssetLoader.LoadModelFromFile(_modelFilePath, OnLoad, OnMaterialsLoad, OnProgress, OnError, _modelContainer, _assetLoaderOptions);
        }

      
      


        var picture = await ApiBase.GetPicture(ApiMocksIds.DownloadPictureMock);
        if (picture != null) {
            _rawImage.texture = picture;
        }
    }

        /// <summary>
        /// Called when any error occurs.
        /// </summary>
        /// <param name="obj">The contextualized error, containing the original exception and the context passed to the method where the error was thrown.</param>
        private void OnError(IContextualizedError obj)
        {
            Debug.LogError($"An error occurred while loading your Model: {obj.GetInnerException()}");
        }

        /// <summary>
        /// Called when the Model loading progress changes.
        /// </summary>
        /// <param name="assetLoaderContext">The context used to load the Model.</param>
        /// <param name="progress">The loading progress.</param>
        private void OnProgress(AssetLoaderContext assetLoaderContext, float progress)
        {
            Debug.Log($"Loading Model. Progress: {progress:P}");
        }

        /// <summary>
        /// Called when the Model (including Textures and Materials) has been fully loaded.
        /// </summary>
        /// <remarks>The loaded GameObject is available on the assetLoaderContext.RootGameObject field.</remarks>
        /// <param name="assetLoaderContext">The context used to load the Model.</param>
        private void OnMaterialsLoad(AssetLoaderContext assetLoaderContext)
        {
            Debug.Log("Materials loaded. Model fully loaded.");
        }

        /// <summary>
        /// Called when the Model Meshes and hierarchy are loaded.
        /// </summary>
        /// <remarks>The loaded GameObject is available on the assetLoaderContext.RootGameObject field.</remarks>
        /// <param name="assetLoaderContext">The context used to load the Model.</param>
        private void OnLoad(AssetLoaderContext assetLoaderContext)
        {
            Debug.Log("Model loaded. Loading materials.");
        }
    
    


}