using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroCamera : MonoBehaviour
{
    public bool useGyro;

    float yRotation, xRotation;

    private void Start()
    {
        Input.gyro.enabled = true;
    }

    void Update()
    {
        yRotation += -Input.gyro.rotationRateUnbiased.y;
        xRotation += -Input.gyro.rotationRateUnbiased.x;

        transform.eulerAngles = new Vector3(xRotation, yRotation, 0);
    }
}