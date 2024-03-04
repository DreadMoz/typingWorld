using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    public GameObject npcPrefab; // NPCのプレハブ
    public Transform[] spawnPoints; // NPCを生成する位置を保持する配列
    public int numberOfNPCs = 10; // 生成するNPCの数、デフォルトは5

    void Start()
    {
        // ゲーム開始時にNPCをスポーン
        SpawnNPCs();
    }

    void SpawnNPCs()
    {
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
            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(110, 270), 0);
            // NPCプレハブのインスタンスを生成し、指定された位置に配置
            Instantiate(npcPrefab, spawnPoints[i].position, randomRotation, transform);
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
