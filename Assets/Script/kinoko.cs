using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kinoko : MonoBehaviour
{
    private Animator kAnimator;

    void Start()
    {
        kAnimator = GetComponent<Animator>(); // kinokoのアニメーターを取得
    }

    void OnMouseDown()
    {
        kAnimator.SetTrigger("jump");
    }
}
