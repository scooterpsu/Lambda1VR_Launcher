using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsRotate : MonoBehaviour
{
    void Update()
    {
        var SettingsButton = OVRInput.GetDown(OVRInput.Button.Start);
        if (SettingsButton)
        {
            RotateCamera();
        }
    }

    private int camState = 0;
    public void RotateCamera()
    {
        Vector3 euler = transform.rotation.eulerAngles;
        if (camState == 0)
        {
            euler.y = -90f;
            camState = 1;
        }
        else
        {
            euler.y = 0f;
            camState = 0;
        }
        transform.rotation = Quaternion.Euler(euler);
    }
}
