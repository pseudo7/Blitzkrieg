using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliController : MonoBehaviour
{
    [Header("Controls")]
    public float strafeTilt = 5f;
    public float movementSpeed = 5f;
    public float fallbackSmooth = 3f;
    public Rigidbody heliRB;
    public Transform heliModel;
    public EngineController engine;

    [Space]
    [Header("Constraints")]
    public float maxXDipAngle = 30;
    public float maxZDipAngle = 30;

    float rotY, moveZ, tiltZ;
    Vector3 heliRotation;

    void Update()
    {
        rotY = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        tiltZ = Input.GetAxis("Tilt");
        heliModel.Rotate(0, 0, -tiltZ, Space.Self);

        engine.targetRPM = Mathf.Abs(moveZ);

        heliRB.AddRelativeTorque(0, rotY, 0);
        heliRB.AddRelativeForce(tiltZ * movementSpeed, 0, moveZ * movementSpeed, ForceMode.Acceleration);
    }

    private void LateUpdate()
    {
        if (tiltZ != 0)
            return;
        heliModel.localRotation = Quaternion.Lerp(heliModel.localRotation, Quaternion.identity, 1 / fallbackSmooth);
    }
}
