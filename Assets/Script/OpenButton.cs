using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OpenButton : MonoBehaviour
{
    private bool doOpen = false;
    
    public void OnButton()
    {
        doOpen = !doOpen;
    }

    public bool isOpen()
    {
        return doOpen;
    }
    public void resetOpen()
    {
        doOpen = false;
    }
    public void forceOpen()
    {
        doOpen = true;
    }

}
