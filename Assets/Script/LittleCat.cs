using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleCat : MonoBehaviour
{
    private Animator cAnimator;

    void Start()
    {
        cAnimator = GetComponent<Animator>(); // LittleCatのアニメーターを取得
    }

    void Update()
    {

    }

    void OnMouseDown()
    {
        cAnimator.SetTrigger("jump");
    }
}
