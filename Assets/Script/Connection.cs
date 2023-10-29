using System.Runtime.InteropServices;
using UnityEngine;

public class Connection : MonoBehaviour
{
    [SerializeField] private GameManager gm;

#if UNITY_WEBGL
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

    public void Auth()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        Firebase();
        GoogleAuth();
#endif
    }

    public void SetFirebase()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        setFirebase();
#endif
    }

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
        string msg = "dummyneco";
        string msgK = "100, 101, 102, 103, 104, 105, 106, 107, 108, 109";
        string msgS = "731, 14, 31, 131, 8, 101, 0, 0, 201, 1, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0";
        string msgI = "1, 2, 3, 4, 0, 0, 5, 0, 121, 0, 0, 6, 0, 151, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0";
        string msgM = "3, 3, 3, 3, 3, 3, 3, 3, 2, 3, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0";

        gm.setUserName(msg);
        gm.setKpm(msgK);
        gm.setStatus(msgS);
        gm.setInventory(msgI);
        gm.setMedals(msgM);
    }
}