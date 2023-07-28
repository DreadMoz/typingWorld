using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OpenButton : MonoBehaviour
{
    public bool doOpen = false;
    
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

}
