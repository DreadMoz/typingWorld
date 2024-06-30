using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class NpcNeco : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent; // NavMeshAgentへの参照
    public float radius = 16f; // ランダムな目的地を探す範囲
    private float speed = 5f;
    public float interval = 8f;
    private bool moveFlg = false;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("animation", 10);

        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        // ◯秒後に最初の移動を開始し、その後◯秒ごとに繰り返し実行
        InvokeRepeating("SetRandomDestination", Random.Range(1f, interval), interval);
    }

    // Update is called once per frame
    void Update()
    {
        if (moveFlg)
        {
            if (Vector3.Distance(transform.position, agent.destination) < 0.2f)
            {
                moveFlg = false;
                animator.SetTrigger("idol");

                // 目的地を解除し、エージェントの移動を停止します。
                agent.ResetPath(); // 目的地を解除する

                // ランダムな整数を生成
                int randomIndex = Random.Range(0, 10);
                if (randomIndex == 0)
                {
                    animator.SetTrigger("dig");
                }
                else if (randomIndex == 1)
                {
                    animator.SetTrigger("sit");
                }

            }
        }
    }

    void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere;
        double distance = System.Math.Sqrt(randomDirection.x * randomDirection.x + randomDirection.z * randomDirection.z);

        if (distance < 0.5f)
        {
            return;
        }
        else if (1.0f < distance)
        {
            animator.SetTrigger("run");
            agent.speed = speed;
        }
        else
        {
            animator.SetTrigger("walk");
            agent.speed = speed / 3;
        }
        moveFlg = true;
        randomDirection *= radius;

        randomDirection += transform.position; // 現在位置からの相対位置を計算
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;

        // ランダムな方向にあるナビゲーションメッシュ上の位置を探す
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }

        // 計算された位置に移動する
        agent.SetDestination(finalPosition);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name != "Terrain")
        {
            animator.SetTrigger("run");
            animator.SetTrigger("walk");
            agent.ResetPath(); // 目的地を解除する
            if (col.gameObject.name == "Player")
            {
                // ランダムな整数を生成
                int randomIndex = Random.Range(0, 5);
                animator.SetInteger("animation", randomIndex);
            }
            else if (col.gameObject.CompareTag("InvisibleFence"))
            {
                animator.SetInteger("animation", 3);    // 怒る
            }
            else
            {
                // 現在の向きから少し後ろの位置を計算
                Vector3 stepBackDestination = transform.position - transform.forward * 0.7f;

                // エージェントに新しい目的地を設定して、後ろに歩かせる
                agent.SetDestination(stepBackDestination);       // 少し戻る（向きを反転）
            }
        }
    }

    void OnCollisionExit(Collision col)
    {
        animator.SetInteger("animation", 10);
    }
}
