using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using static Item;

namespace AdventCalendar2020
{
    public class dataManager : MonoBehaviour
    {
        [SerializeField]
        private GameManager gameManager;

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
            int[] msgS = { 1, 202, 3, 37, 178, 999, 101, 1, 2, 151, 0, 202 };
            int[] msgI = { 1, 2, 3, 4, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] msgM = { 3, 3, 3, 3, 3, 3, 3, 3, 2, 3, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            gameManager.setName("neko");
            gameManager.setStatus(msgS);
            gameManager.setInventry(msgI);
            gameManager.setMedals(msgM);
        }
    }
}
