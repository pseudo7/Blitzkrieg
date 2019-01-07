using UnityEngine;

public class EngineController : MonoBehaviour
{

    public Transform mainRotor;
    public Transform tailRotor;
    public AudioSource helicopterSound;

    public float targetRPM;
    public bool engineOn;
    public bool useAcceleration;
    public float currentRPM;

    bool disableAudio = false;
    float clampedRPM;
    float accelerationRate;
    float initialVolume;
    bool hasRotors;

    void Awake()
    {
        accelerationRate = 0.2f;
        ControlSetup();
    }

    void Update()
    {
        if (hasRotors) HelicopterRotor();
    }

    void HelicopterRotor()
    {
        if (engineOn || (currentRPM > 0.0 && useAcceleration))
        {
            clampedRPM = Mathf.Clamp(targetRPM * 0.7f + 0.3f, 0.3f, 1f);

            if (useAcceleration)
            {
                if (engineOn)
                {
                    if (currentRPM <= clampedRPM)
                    {
                        currentRPM = Mathf.Clamp(currentRPM + (accelerationRate * Time.deltaTime), 0, clampedRPM);
                    }
                    else
                    {
                        currentRPM = Mathf.Clamp(currentRPM - (accelerationRate * Time.deltaTime), 0, 1);
                    }
                }
                else
                {
                    currentRPM = Mathf.Clamp(currentRPM - (accelerationRate * Time.deltaTime), 0, 1);
                }
            }
            else
            {
                currentRPM = clampedRPM;
            }

            mainRotor.Rotate(new Vector3(0, -1460 * currentRPM * Time.deltaTime, 0), Space.Self);

            tailRotor.Rotate(new Vector3(1460 * currentRPM * Time.deltaTime, 0, 0), Space.Self);

            if (disableAudio == false)
            {
                if (helicopterSound.isPlaying == false) helicopterSound.Play();
                helicopterSound.pitch = 0.1f + currentRPM;
                helicopterSound.volume = Mathf.Clamp(initialVolume * Mathf.Clamp(currentRPM * 3.333f, 0, 1), 0, initialVolume);
            }
        }
        else
        {
            if (disableAudio == false) helicopterSound.Stop();
        }
    }

    void ControlSetup()
    {
        hasRotors = true;

        if (!mainRotor)
            hasRotors = false;

        if (!tailRotor)
            hasRotors = false;

        if (!helicopterSound)
            disableAudio = true;
        else
            initialVolume = helicopterSound.volume;
    }
}
