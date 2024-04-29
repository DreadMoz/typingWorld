using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gm;

    public GameObject npcPrefab; // NPCのプレハブ
    public Transform[] spawnPoints; // NPCを生成する位置を保持する配列
    public int numberOfNPCs = 10; // 生成するNPCの数、デフォルトは10
    private int[] pickedPlayers;     // NPCとして登場するユーザーの順位

    void Start()
    {
    }

    void shufflePlayers()
    {
        List<int> playerPool = new List<int>();
        for (int i = 1; i <= 150; i++)
        {
            playerPool.Add(i);
        }

        pickedPlayers = new int[numberOfNPCs];
        for (int i = 0; i < numberOfNPCs; i++)
        {
            int randomIndex = Random.Range(0, playerPool.Count);
            pickedPlayers[i] = playerPool[randomIndex];
            playerPool.RemoveAt(randomIndex); // 選んだ数値をプールから削除して重複を防ぐ
        }
    }

    public void SpawnNPCs()
    {
        if (gm.savedata.Settings[se.Extension] == 0)
        {
            return;
        }

        shufflePlayers();
        // 生成するNPCの数をスポーンポイントの数と比較し、小さい方を使用
        int spawnCount = Mathf.Min(numberOfNPCs, spawnPoints.Length);

        // 既存のNPCをクリア
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // 指定された数だけNPCをスポーン
        for (int i = 0; i < spawnCount; i++)
        {
            // Y軸周りでランダムな角度を選択
            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(110, 220), 0);
            // NPCプレハブのインスタンスを生成し、指定された位置に配置
            GameObject npcInstance = Instantiate(npcPrefab, spawnPoints[i].position, randomRotation, transform);

            // インスタンスにアタッチされているChibiCatスクリプトを取得
            ChibiCat chibiCatScript = npcInstance.GetComponentInChildren<ChibiCat>();

            if (chibiCatScript != null)
            {
                if (gm.savedata.ExRankings.Count > 0)
                {
                    string nickname;
                    Item item = gm.db.GetItemList()[gm.savedata.ExRankings[pickedPlayers[i]].NickName];
                    if (item != null)
                    {
                        nickname = item.MyItemName;
                    }
                    else
                    {
                        nickname = "さん";
                    }
                    chibiCatScript.setName(gm.savedata.ExRankings[pickedPlayers[i]].Name + nickname);

                    chibiCatScript.setChara(gm.savedata.ExRankings[pickedPlayers[i]].CatBody - 200);
                    chibiCatScript.releaseAllEquip();
                    chibiCatScript.changeEquipHands(gm.savedata.ExRankings[pickedPlayers[i]].RightHand, gm.savedata.ExRankings[pickedPlayers[i]].LeftHand, 0);
                    chibiCatScript.changeEquipHead(gm.savedata.ExRankings[pickedPlayers[i]].Head);
                    chibiCatScript.changeEquipGlasses(gm.savedata.ExRankings[pickedPlayers[i]].Glasses);
                }
            }
        }
    }

    // デバッグやUIから呼び出すためのメソッド
    // スライダーなどを使用してNPCの数を変更したい場合に使用します
    public void UpdateNPCCount(int newCount)
    {
        if ((newCount < 0) || (10 < newCount))
        {
            return;
        }
        if (numberOfNPCs == newCount)
        {
            return;
        }
        numberOfNPCs = newCount;
        SpawnNPCs();
    }
}
