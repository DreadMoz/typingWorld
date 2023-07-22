using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windowButton : MonoBehaviour
{
    public bool openInventry = false;

    private void OnButtonInventry()
    {
        openInventry = !openInventry;
    }

    public bool isOpenInventry()
    {
        return openInventry;
    }

}
