using System;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

namespace AdventCalendar2020
{
    public class GoogleSheetManager : MonoBehaviour
    {
        /// <summary>
        /// Jsonの変換用クラス
        /// </summary>
        public class ValueRange
        {
            public string range;

            public string majorDimension;

            public string[][] values;
        }

        private const string GoogleSpreadsheetURL = "https://sheets.googleapis.com/v4/spreadsheets";

#if UNITY_WEBGL
        /// <summary>
        /// jslibと名前を合わせる
        /// </summary>
        [DllImport("__Internal")]
        private static extern bool IsGoogleSignedIn();

        [DllImport("__Internal")]
        private static extern void GoogleAuth();

        [DllImport("__Internal")]
        private static extern void GoogleSignOut();
#endif

        /// <summary>
        /// JS側から渡されるアクセストークン
        /// </summary>
        private string _accessToken;

        private readonly ReactiveProperty<bool> _isSignedInProperty = new ReactiveProperty<bool>();
        public IReactiveProperty<bool> IsSignedInProperty => _isSignedInProperty;
        public bool IsSignedIn => IsSignedInProperty.Value;

        /// <summary>
        /// google認証
        /// </summary>
        public void Auth()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            GoogleAuth();
#endif
        }

        /// <summary>
        /// ログアウト
        /// </summary>
        public void SignOut()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            GoogleSignOut();
#endif
        }

        /// <summary>
        /// スプレッドシートのレンジを取得
        /// </summary>
        /// <param name="spreadsheetId">スプレッドシートのユニークID</param>
        /// <param name="range">レンジ Ex: "シート１!A1:B2" はシート１のA1からB2までという意味</param>
        public async UniTask<ValueRange> GetSpreadsheetAsync(string spreadsheetId, string range, CancellationToken ct)
        {
            var url = $"{GoogleSpreadsheetURL}/{spreadsheetId}/values/{range}";
            try
            {
                var responseText = await SendGetRequestAsync(url, ct);
                Debug.Log(responseText);
                return JsonConvert.DeserializeObject<ValueRange>(responseText);
            }
            catch (UnauthorizedAccessException)
            {
                Debug.Log("未認証");
                return null;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// UnityWebRequest.Getで送信する
        /// </summary>
        private async UniTask<string> SendGetRequestAsync(string url, CancellationToken ct)
        {
            var finalUrl = AppendAccessTokenToURL(url, _accessToken);
            using (var uwr = UnityWebRequest.Get(finalUrl))
            {
                await uwr.SendWebRequest().WithCancellation(ct);

                if (uwr.error != null)
                {
                    var errorDefinition = new
                    {
                        error = new
                        {
                            status = ""
                        }
                    };
                    var errorData = JsonConvert.DeserializeAnonymousType(uwr.downloadHandler.text, errorDefinition);
                    // 未認証エラー
                    if (errorData.error.status == "UNAUTHENTICATED") throw new UnauthorizedAccessException();
                    // その他エラー
                    throw new Exception(uwr.downloadHandler.text);
                }

                return uwr.downloadHandler.text;
            }
        }

        /// <summary>
        /// URLの最後にクエリを追加
        /// </summary>
        private string AppendAccessTokenToURL(string url, string token)
        {
            var delimiter = url.Contains("?") ? "&" : "?";
            return $"{url}{delimiter}access_token={token}";
        }

        private void OnDestroy()
        {
            _isSignedInProperty.Dispose();
        }

        #region From JavaScript

        public void SetAccessToken(string token)
        {
            _accessToken = token;
            IsSignedInProperty.Value = string.IsNullOrEmpty(_accessToken) == false;
        }

        #endregion
    }
}
