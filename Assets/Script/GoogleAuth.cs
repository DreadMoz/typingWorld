using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Oauth2.v2;
using Google.Apis.Oauth2.v2.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.IO;

public class GoogleAuth : MonoBehaviour
{
    private string clientId;
    private string clientSecret;

    void Start()
    {
        LoadCredentials();
    }

    private void LoadCredentials()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "credentials.json");

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            GoogleCredentials credentials = JsonUtility.FromJson<GoogleCredentials>(dataAsJson);

            clientId = credentials.installed.client_id;
            clientSecret = credentials.installed.client_secret;
        }
        else
        {
            Debug.LogError("Cannot find credentials file.");
        }
    }

    // 認証とユーザー情報の取得
    public async void AuthenticateAndDisplayUserInfo()
    {
        Userinfo userInfo = await Authenticate();
    }

    // プロフィール画像をロードして表示
    public IEnumerator LoadProfileImage(string imageUrl, Action<Texture2D> onCompleted)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            onCompleted?.Invoke(texture);
        }
        else
        {
            Debug.LogError("画像のダウンロードに失敗しました: " + request.error);
            onCompleted?.Invoke(null);
        }
    }


    public async Task<Userinfo> Authenticate()
    {
        // スコープを設定
        string[] scopes = new string[] { Oauth2Service.Scope.UserinfoEmail, Oauth2Service.Scope.UserinfoProfile };

        // ClientSecrets オブジェクトを作成
        ClientSecrets secrets = new ClientSecrets
        {
            ClientId = clientId,
            ClientSecret = clientSecret
        };

        // ユーザー認証リクエストを生成
        UserCredential credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            secrets,
            scopes,
            "user",
            CancellationToken.None);

        // ここで NullDataStore を使用してキャッシュされた認証情報を使用しないように設定
/*        UserCredential credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            secrets,
            scopes,
            "user",
            CancellationToken.None,
            new NullDataStore());
*/
        // Oauth2サービスを初期化
        var service = new Oauth2Service(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "Unity Google Auth"
        });

        // ユーザー情報を取得
        Userinfo userInfo = await service.Userinfo.Get().ExecuteAsync();
        return userInfo;
    }
}

[System.Serializable]
public class GoogleCredentials
{
    public Installed installed;

    [System.Serializable]
    public class Installed
    {
        public string client_id;
        public string client_secret;
        // 他のフィールドも必要に応じて追加...
    }
}