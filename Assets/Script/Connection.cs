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
        string msg = "neco";
        int[] msgS = { 50, 3, 37, 178, 1, 101, 121, 151, 201, 1, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] msgI = { 1, 2, 3, 4, 0, 0, 5, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] msgM = { 3, 3, 3, 3, 3, 3, 3, 3, 2, 3, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        gm.setUserName(msg);
        gm.setStatus(msgS);
        gm.setInventry(msgI);
        gm.setMedals(msgM);
    }
}