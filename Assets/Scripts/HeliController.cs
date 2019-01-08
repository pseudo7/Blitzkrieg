using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class HeliController : MonoBehaviour
{
    [Header("Controls")]
    public float strafeSpeed = 3f;
    public float throttleSpeed = 5f;
    public float liftSpeed = 5f;
    public float rotationSpeed = 40f;
    public float normalFallback = 60f;
    public float tiltFallback = 40f;
    public float dipFallback = 40f;

    [Header("References")]
    public Rigidbody heliRB;
    public Transform heliModel;
    public EngineController engine;

    [Header("Constraints")]
    public float maxDipAngle = 15;
    public float maxTiltAngle = 25;

    float rotY, moveY, moveX, moveZ;

    Quaternion leftStrafe, rightStrafe;
    Quaternion forwardDip, backwardDip;

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Start()
    {
        leftStrafe = Quaternion.Euler(0, 0, maxTiltAngle * 2);
        rightStrafe = Quaternion.Euler(0, 0, -maxTiltAngle * 2);
        forwardDip = Quaternion.Euler(maxDipAngle * 2, 0, 0);
        backwardDip = Quaternion.Euler(-maxDipAngle * 2, 0, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        rotY = CrossPlatformInputManager.GetAxis("Rotational_H");
        moveY = CrossPlatformInputManager.GetAxis("Rotational_V");

        moveX = CrossPlatformInputManager.GetAxis("Strafe_H");
        moveZ = CrossPlatformInputManager.GetAxis("Strafe_V");

        if (rotY == 0 && moveX != 0)
            heliModel.localRotation = Quaternion.Lerp(heliModel.localRotation, moveX < 0 ? leftStrafe : rightStrafe, 1 / tiltFallback);
        if (moveZ != 0)
            heliModel.localRotation = Quaternion.Lerp(heliModel.localRotation, moveZ > 0 ? forwardDip : backwardDip, 1 / dipFallback);
        if (rotY != 0)
            heliRB.transform.Rotate(0, rotY * Time.deltaTime * rotationSpeed, 0);

        engine.targetRPM = Mathf.Max(.6f, Mathf.Abs(moveZ), Mathf.Abs(moveY), Mathf.Abs(moveX));

        heliRB.AddRelativeForce(moveX * strafeSpeed, moveY * liftSpeed, moveZ * throttleSpeed, ForceMode.Acceleration);
    }

    private void LateUpdate()
    {
        if (moveX != 0)
            return;
        heliModel.localRotation = Quaternion.Lerp(heliModel.localRotation, Quaternion.identity, 1 / normalFallback);
        //heliModel.localRotation = Quaternion.Lerp(heliModel.localRotation, Quaternion.Euler(0, heliModel.eulerAngles.y, 0), 1 / normalFallback);
    }
}
