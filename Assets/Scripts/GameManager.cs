using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public HeliController heliController;
    public Transform spawnParent;

    void Awake()
    {
        var engine = Instantiate(DDOL_Navigation.SelectedChopper, spawnParent).GetComponent<EngineController>();
        engine.engineOn = true;
        heliController.engine = engine;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
