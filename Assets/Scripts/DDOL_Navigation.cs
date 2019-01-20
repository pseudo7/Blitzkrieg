using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DDOL_Navigation : MonoBehaviour
{
    public static DDOL_Navigation Instance;

    public static GameObject SelectedChopper { set; get; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Pseudo/Capture")]
    public static void Capture()
    {
        ScreenCapture.CaptureScreenshot(string.Format("{0}.png", System.DateTime.Now.Ticks.ToString()));
    }
#endif

    public void LoadGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
