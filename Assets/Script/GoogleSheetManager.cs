using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using static Item;

namespace AdventCalendar2020
{
    public class GoogleSheetManager : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;

        /// <summary>
        /// Json???????p?N???X
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
        /// jslib?????O??????????
        /// </summary>
        [DllImport("__Internal")]
        private static extern bool IsGoogleSignedIn();

        [DllImport("__Internal")]
        private static extern void GoogleAuth();

        [DllImport("__Internal")]
        private static extern void Firebase();

        [DllImport("__Internal")]
        private static extern void getFirebase();

        [DllImport("__Internal")]
        private static extern void setFirebase();
#endif

        /// <summary>
        /// JS???????n???????A?N?Z?X?g?[?N??
        /// </summary>
        private string _accessToken;

        private readonly ReactiveProperty<bool> _isSignedInProperty = new ReactiveProperty<bool>();
        public IReactiveProperty<bool> IsSignedInProperty => _isSignedInProperty;
        public bool IsSignedIn => IsSignedInProperty.Value;

        /// <summary>
        /// google?F??
        /// </summary>
        public void Auth()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            Firebase();
            GoogleAuth();
#endif
        }

        /// <summary>
        /// google?F??
        /// </summary>
        public void SetFirebase()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            setFirebase();
#endif
        }

        /// <summary>
        /// google?F??
        /// </summary>
        public void GetFirebase()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            getFirebase();
#else
            getDummyDatabase();
#endif
        }

        private void getDummyDatabase()
        {
            int[] msgS = { 1, 1, 1, 178, 999 };
            bool[] msgI = { true, true, true, true, true, true, true, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
            int[] msgE = { 201, 1, 2, 151, 0 };
            int[] msgM = { 3, 3, 3, 3, 3, 3, 3, 3, 2, 3, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            gameManager.getStatus(msgS);
            gameManager.getInventry(msgI);
            gameManager.getEquipments(msgE);
            gameManager.getMedals(msgM);
        }

        /// <summary>
        /// ?X?v???b?h?V?[?g???????W??????
        /// </summary>
        /// <param name="spreadsheetId">?X?v???b?h?V?[?g?????j?[?NID</param>
        /// <param name="range">?????W Ex: "?V?[?g?P!A1:B2" ???V?[?g?P??A1????B2??????????????</param>
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
                Debug.Log("???F??");
                return null;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// UnityWebRequest.Get?????M????
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
                    // ???F???G???[
                    if (errorData.error.status == "UNAUTHENTICATED") throw new UnauthorizedAccessException();
                    // ???????G???[
                    throw new Exception(uwr.downloadHandler.text);
                }

                return uwr.downloadHandler.text;
            }
        }

        /// <summary>
        /// URL?????????N?G????????
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

        private void getDummyDataBase()
        {

        }
    }
}
