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
        private Button _setDataButton;

        [SerializeField]
        private Button _getDataButton;

        private void Start()
        {
        }

        /// <summary>
        /// AuthButton��Event����Ă�
        /// </summary>
        public void GoogleAuth() => _sheetManager.Auth();

        /// <summary>
        /// GetDataButton��Event����Ă�
        /// </summary>
        public void GetFirebase() => _sheetManager.GetFirebase();

        /// <summary>
        /// SetDataButton��Event����Ă�
        /// </summary>
        public void SetFirebase() => _sheetManager.SetFirebase();
    }
}
