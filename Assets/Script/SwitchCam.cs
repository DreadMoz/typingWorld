using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCam : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;

    void Start()
    {
        camera2.enabled = false;
    }

    public void SwitchCamera()
    {
        camera1.enabled = !camera1.enabled;
        camera2.enabled = !camera2.enabled;
    }
}
