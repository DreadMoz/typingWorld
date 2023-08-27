using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UniRx;
using UnityEngine;

namespace AdventCalendar2020
{
    public class GoogleSheetManager : MonoBehaviour
    {
        [SerializeField] private GameManager gm;

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
            int[] msgS = { 202, 3, 37, 178, 50, 101, 1, 2, 151 };
            int[] msgI = { 1, 2, 3, 4, 0, 0, 5, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] msgM = { 3, 3, 3, 3, 3, 3, 3, 3, 2, 3, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            gm.getStatus(msgS);
            gm.getInventry(msgI);
            gm.getMedals(msgM);
        }
    }
}
