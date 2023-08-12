using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace AdventCalendar2020
{
    public class SampleGoogleSheet : MonoBehaviour
    {
        [SerializeField]
        private GoogleSheetManager _sheetManager;

        [SerializeField]
        private Button _authButton;

        [SerializeField]
        private Button _singOutButton;

        [SerializeField]
        private Button _getDataButton;

        private void Start()
        {
            // 最初は何も表示しない
            _authButton.gameObject.SetActive(false);
            _singOutButton.gameObject.SetActive(false);
            _getDataButton.gameObject.SetActive(false);
            Bind();
        }

        private void Bind()
        {
            // サインイン状態が更新されたら検知
            _sheetManager.IsSignedInProperty
                .Subscribe(UpdateButtonActive) // isSigned => UpdateButtonActive(isSigned);
                .AddTo(gameObject);
        }

        /// <summary>
        /// サインイン状態に応じてボタンの表示切り替え
        /// </summary>
        private void UpdateButtonActive(bool isSigned)
        {
            _authButton.gameObject.SetActive(isSigned == false);
            _singOutButton.gameObject.SetActive(isSigned);
            _getDataButton.gameObject.SetActive(isSigned);
        }

        /// <summary>
        /// AuthButtonのEventから呼ぶ
        /// </summary>
        public void GoogleAuth() => _sheetManager.Auth();

        /// <summary>
        /// SignOutButtonのEventから呼ぶ
        /// </summary>
        public void GoogleSignOut() => _sheetManager.SignOut();

        /// <summary>
        /// GetDataButtonのEventから呼ぶ
        /// </summary>
        public void GetSheetData() => GetSheetDataAsync(gameObject.GetCancellationTokenOnDestroy()).Forget();

        /// <summary>
        /// スプシのデータをとりコンソールに出力
        /// </summary>
        private async UniTask GetSheetDataAsync(CancellationToken ct)
        {
            if (_sheetManager.IsSignedIn == false) return;

            // プライベートなスプシIDでも可能（認証した際のアカウントが見れるなら取れる
            // 今回はサンプルそのままを書きます
            // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
            var spreadsheetId = "1Eu9XzoSDAEfmO3mrOnImbnKm0qF_CBa4eUqV3cPG_dU";
            var range = "Class Data!A2:E";

            var data = await _sheetManager.GetSpreadsheetAsync(spreadsheetId, range, ct);
            if (data != null && data.values.Length > 0)
            {
                foreach (var row in data.values)
                {
                    Debug.Log($"{row[0]}, {row[4]}");
                }
            }
            else
            {
                Debug.Log("No data found.");
            }
        }
    }
}
