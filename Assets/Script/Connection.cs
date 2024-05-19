using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

public class Connection : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private TitleSky title;

#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void GetOAuth();
    [DllImport("__Internal")]
    private static extern void OAuthLogout();
    [DllImport("__Internal")]
    private static extern void LoadDataFromExtension();
    [DllImport("__Internal")]
    private static extern void SaveStatusToExtension(string data);
    [DllImport("__Internal")]
    private static extern void LoadFromGss(); 
    [DllImport("__Internal")]
    private static extern void SaveToGss(string dataPointer);

#endif

    public void enetLogin()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        GetOAuth();
#else
        getDummyOAuth();
#endif
    }

    public void googleLogout()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        OAuthLogout();
#else
        title.finishLogout();
#endif
    }

    public void loadExtension()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        LoadDataFromExtension(); // 拡張機能にデータを要求
#else
        getDummyExtension();
#endif
    }

    public void saveExtension(string data)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        SaveStatusToExtension(data);
#endif
    }

    public void saveGas(string dataPointer)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        SaveToGss(dataPointer);
#endif
    }

    public void loadGas()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        LoadFromGss();
#else
        getDummyGss();
#endif
    }

    /*
    private void getDummyDb()
    {
        string msg = "dummyneco";
        string msgK = "100, 101, 102, 103, 104, 105, 106, 107, 108, 109";
        // Gold,Server,Rank,Kpm
        string msgS = "30, 1, 150, 0";
        // RightHnad,Glasses(121),Head(151),LeftHand,CatBody(201)あえて0,CatFace(101),NickName(211)
        string msgE = "0, 120, 150, 0, 0, 100, 210";
        string msgI = "0, 0, 0, 0, 0, 0, 5, 0, 121, 0, 0, 6, 0, 151, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0";
        string msgM = "4, 4, 4, 4, 3, 3, 3, 3, 2, 3, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0";

        gm.savedata.setUserNameFromFireBase(msg);
        gm.setKpm(msgK);
        gm.setStatus(msgS);
        gm.setEquipment(msgE);
        gm.setInventory(msgI);
        gm.setMedals(msgM);

        title.setDummyData();
        //        title.finishDataLoad();
    }
    */

    private void getDummyOAuth()
    {
        if (gm.enetToggle.isOn)
        {
            title.finishOAuth("demonstration@e-net.nara.jp,Demo Robo,https://lh3.googleusercontent.com/a/AAcHTtdjq-TTMMygrjVNtRA6vb15AMinz6HfsldU-_wzQYF3F2j8=s96-c");
        }
        else
        {
            title.finishOAuth("rochy2moo@gmail.com,Ryosuke Mori,https://lh3.googleusercontent.com/a/AAcHTtdjq-TTMMygrjVNtRA6vb15AMinz6HfsldU-_wzQYF3F2j8=s96-c");
        }
    }

    private void getDummyExtension()
    {
        if (!gm.exToggle.isOn)
        {
            Thread.Sleep(2000);
            gm.savedata.Settings[se.Extension] = 0;
            title.OnRequestTimeout();       // 拡張機能なし。タイムアウトのイメージ
        }
        else
        {
            // rankingDataとstatusDataを含むダミーのJSON文字列
            string combinedJson = @"{
                ""rankingData"": {
                    ""value"": [[0,1,""haruto"",1,0,0,0,201,0,211,574],[0,2,""yuto"",2,0,0,0,202,0,211,573],[0,3,""sota"",3,0,0,1,203,0,211,573],[0,4,""yuki"",0,0,0,2,204,0,212,572],[0,5,""hayato"",0,0,0,3,205,0,213,572],[0,6,""haruki"",0,151,121,4,205,0,211,571],[0,7,""ryusei"",4,0,0,5,208,0,212,571],[0,8,""kaito"",5,0,0,1,209,0,211,570],[0,9,""kota"",0,151,0,1,210,0,0,570],[0,10,""yuma"",1,0,0,2,207,0,213,569],[0,11,""soma"",1,0,0,0,201,0,214,569],[0,12,""riku"",0,0,0,0,202,0,214,568],[0,13,""sora"",0,0,0,3,203,0,212,568],[0,14,""ryota"",0,0,121,0,204,0,214,567],[0,15,""daiki"",0,0,0,0,205,0,215,567],[0,16,""minato"",6,0,0,0,206,0,212,566],[0,17,""ren"",1,0,0,0,208,0,211,566],[0,18,""hinata"",2,0,0,0,209,0,213,565],[0,19,""kazuki"",3,0,0,1,210,0,214,565],[0,20,""takumi"",0,0,0,2,207,0,213,564],[0,21,""hiroto"",0,0,0,3,201,0,215,564],[0,22,""ryuto"",0,0,0,4,202,0,212,563],[0,23,""yuma"",4,0,121,5,203,0,214,563],[0,24,""sosuke"",5,0,0,1,204,0,0,562],[0,25,""ryu"",0,0,0,1,205,0,0,562],[0,26,""keita"",1,0,0,2,206,0,0,561],[0,27,""koki"",1,0,0,0,208,0,0,561],[0,28,""toma"",6,0,0,0,209,0,0,560],[0,29,""seiji"",1,0,0,3,210,0,0,560],[0,30,""yu"",2,0,0,0,207,0,0,559],[0,31,""hana"",3,0,0,0,201,0,0,559],[0,32,""yui"",0,0,121,0,202,0,0,558],[0,33,""rin"",0,0,121,0,203,0,0,558],[0,34,""mei"",0,0,0,0,204,0,0,557],[0,35,""mio"",4,0,0,1,205,0,0,557],[0,36,""saki"",5,0,0,2,206,0,0,556],[0,37,""aoi"",0,0,0,3,208,0,0,556],[0,38,""yuna"",1,0,0,4,209,0,0,555],[0,39,""maika"",1,151,121,5,210,0,0,555],[0,40,""kokona"",6,0,0,1,207,0,0,554],[0,41,""miku"",1,0,0,1,201,0,0,554],[0,42,""nana"",2,151,0,2,202,0,0,553],[0,43,""rika"",3,0,0,0,203,0,0,553],[0,44,""yuka"",0,0,0,0,204,0,0,552],[0,45,""haruka"",0,0,0,3,205,0,0,552],[0,46,""emi"",0,0,0,0,206,0,0,551],[0,47,""risa"",4,0,121,0,208,0,0,551],[0,48,""yuri"",5,0,0,0,209,0,0,550],[0,49,""sakura"",0,0,0,0,210,0,0,550],[0,50,""rei"",1,0,0,0,207,0,0,549],[0,51,""noa"",1,0,0,1,201,0,0,549],[0,52,""mai"",6,0,0,2,202,0,0,548],[0,53,""rio"",1,0,0,3,203,0,0,548],[0,54,""meika"",2,0,0,4,204,0,0,547],[0,55,""erika"",3,0,0,5,205,0,0,547],[0,56,""airi"",0,0,121,1,206,0,0,546],[0,57,""marin"",0,0,0,1,208,0,0,546],[0,58,""aya"",0,0,0,2,209,0,0,545],[0,59,""mina"",4,0,0,0,210,0,0,545],[0,60,""yuko"",5,0,0,0,207,0,0,544],[0,61,""kaede"",0,0,0,3,201,0,0,544],[0,62,""ayumu"",1,0,0,0,202,0,0,543],[0,63,""taiga"",1,0,0,0,203,0,0,543],[0,64,""shota"",6,0,0,0,204,0,0,542],[0,65,""eito"",1,0,121,0,205,0,0,542],[0,66,""reo"",2,0,121,0,206,0,0,541],[0,67,""kensei"",3,0,0,1,208,0,0,541],[0,68,""shin"",0,0,0,2,209,0,0,540],[0,69,""manato"",0,0,0,3,210,0,0,540],[0,70,""ryoga"",0,0,0,4,207,0,0,539],[0,71,""kanata"",4,0,0,5,201,0,0,539],[0,72,""tsubasa"",5,151,121,1,202,0,0,538],[0,73,""itsuki"",0,0,0,1,203,0,0,538],[0,74,""asahi"",1,0,0,2,204,0,0,537],[0,75,""mahiro"",1,151,0,0,205,0,0,537],[0,76,""haru"",6,0,0,0,206,0,0,536],[0,77,""ikki"",1,0,0,3,208,0,0,536],[0,78,""sho"",2,0,0,0,209,0,0,535],[0,79,""yuki"",3,0,0,0,210,0,0,535],[0,80,""kyou"",0,0,121,0,207,0,0,534],[0,81,""ayaka"",0,0,0,0,201,0,0,534],[0,82,""sena"",0,0,0,0,202,0,0,533],[0,83,""himari"",4,0,0,1,203,0,0,533],[0,84,""yume"",5,0,0,2,204,0,0,532],[0,85,""aina"",0,0,0,3,205,0,0,532],[0,86,""kanon"",1,0,0,4,206,0,0,531],[0,87,""ryosuke"",6,0,121,3,207,0,0,531],[0,88,""saya"",1,0,0,5,208,0,0,530],[0,89,""kaho"",6,0,0,1,209,0,0,530],[0,90,""fumi"",1,0,121,1,210,0,0,529],[0,91,""sara"",2,0,0,2,207,0,0,529],[0,92,""momoka"",3,0,0,0,201,0,0,528],[0,93,""sumire"",0,0,0,0,202,0,0,528],[0,94,""akari"",0,0,0,3,203,0,0,527],[0,95,""hinako"",0,0,0,0,204,0,0,527],[0,96,""yuina"",4,0,0,0,205,0,0,526],[0,97,""riona"",5,0,0,0,206,0,0,526],[0,98,""manami"",0,0,0,0,208,0,0,525],[0,99,""sayaka"",1,0,121,0,209,0,0,525],[0,100,""nao"",1,0,121,1,210,0,0,524],[0,101,""yusuke"",6,0,0,2,207,0,0,524],[0,102,""tatsuya"",1,0,0,3,201,0,0,523],[0,103,""kazuma"",2,0,0,4,202,0,0,523],[0,104,""masato"",3,0,0,5,203,0,0,522],[0,105,""shun"",0,0,0,1,204,0,0,522],[0,106,""kyohei"",0,151,121,1,205,0,0,521],[0,107,""takuya"",0,0,0,2,206,0,0,521],[0,108,""naoki"",4,0,0,0,208,0,0,520],[0,109,""kenta"",5,151,0,0,209,0,0,520],[0,110,""jun"",0,0,0,3,210,0,0,519],[0,111,""misaki"",1,0,0,0,207,0,0,519],[0,112,""riko"",1,0,0,0,201,0,0,518],[0,113,""chinatsu"",6,0,0,0,202,0,0,518],[0,114,""kumi"",1,0,121,0,203,0,0,517],[0,115,""miyu"",2,0,0,0,204,0,0,517],[0,116,""ryou"",3,0,0,1,205,0,0,516],[0,117,""naoko"",0,0,0,2,206,0,0,516],[0,118,""keiko"",0,0,0,3,208,0,0,515],[0,119,""chie"",0,0,0,4,209,0,0,515],[0,120,""akiko"",4,0,0,5,210,0,0,514],[0,121,""asuka"",5,0,0,1,207,0,0,514],[0,122,""kaito"",0,0,0,1,201,0,0,513],[0,123,""natsuki"",1,0,121,2,202,0,0,513],[0,124,""ryohei"",1,0,0,0,203,0,0,512],[0,125,""satoshi"",6,0,0,0,204,0,0,512],[0,126,""takahiro"",1,0,0,3,205,0,0,511],[0,127,""yasuharu"",2,0,0,0,206,0,0,511],[0,128,""yoshiki"",3,0,0,0,208,0,0,510],[0,129,""yota"",0,0,0,0,209,0,0,510],[0,130,""daigo"",0,0,0,0,210,0,0,509],[0,131,""ema"",0,0,0,0,207,0,0,509],[0,132,""himawari"",4,0,121,1,201,0,0,508],[0,133,""ichika"",5,0,0,2,202,0,0,508],[0,134,""juri"",0,0,0,3,203,0,0,507],[0,135,""kairi"",1,0,0,4,204,0,0,507],[0,136,""runa"",1,0,0,5,205,0,0,506],[0,137,""mao"",6,0,0,1,206,0,0,506],[0,138,""nagisa"",1,0,0,1,208,0,0,505],[0,139,""otoha"",2,0,0,2,209,0,0,505],[0,140,""hina"",3,0,0,0,210,0,0,504],[0,141,""rena"",0,0,121,0,207,0,0,504],[0,142,""suzu"",0,0,0,3,201,0,0,503],[0,143,""saiga"",0,0,0,0,202,0,0,503],[0,144,""umi"",4,0,0,0,203,0,0,502],[0,145,""nami"",5,0,121,0,204,0,0,502],[0,146,""wakana"",0,0,121,0,205,0,0,501],[0,147,""haruto"",1,0,0,0,206,0,0,501],[0,148,""yuto"",1,0,0,0,208,0,0,500],[0,149,""sota"",1,0,0,0,209,0,0,500],[0,150,""yuki"",1,0,0,0,210,0,0,499]]
                },
                ""statusData"": {
                    ""Email"": ""demonstration@e-net.nara.jp"",
                    ""Ou"": ""/公立学校/低学年/OU市/OU小学校"",
                    ""LastName"": ""0603-24"",
                    ""Gold"": 999,
                    ""Stage"": 7,
                    ""Ranking"": 87,
                    ""Name"": ""moru"",
                    ""RightHand"": 6,
                    ""Glasses"": 0,
                    ""Head"": 121,
                    ""LeftHand"": 3,
                    ""CatBody"": 206,
                    ""CatFace"": 0,
                    ""NickName"": 0,
                    ""Kpm"": 333,
                    ""Inventory"": [151,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],
                    ""Items"": [88,144115188075855872,8388608,0],
                    ""Medals"": [656279013556373796,476371964491057444,471305275021828764,511767441717405468,86064876791434],
                    ""Kpms"": ""001022333444555666777888"",
                    ""Settings"": [0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
                }
            }";

            gm.savedata.Settings[se.Extension] = 1;

            // finishDataLoadを呼び出して、組み合わせたデータを渡す
            title.finishDataLoad(combinedJson);
        }
    }
    private void getDummyGss()
    {
        string gssData;
        if (gm.gssToggle.isOn)
        {
            // rankingDataとstatusDataを含むダミーのJSON文字列
            gssData = @"{
            ""done"": true,
            ""response"": {
                ""type"": ""type.googleapis.com/google.apps.script.v1.ExecutionResponse"",
                ""result"": ""demonstration@e-net.nara.jp,/公立学校/低学年/OU市/OU小学校,0603-24,659,7,87,moru,6,0,121,3,202,0,0,333,122333444555666777,656279013556373800,476371964491057500,471305275021828740,511767441717405440,0,0,0,0,0""
                }
            }";
        }
        else
        {
            gssData = "";
        }
        title.finishDataLoadGas(gssData);
    }
}