using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 rotationAngle;

    void Update()
    {
        transform.Rotate(rotationAngle);
    }
}
