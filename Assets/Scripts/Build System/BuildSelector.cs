using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSelector : MonoBehaviour
{

    public GameObject buildPanel;
    public BuildSystem buildSystem;
    public bool showPanel = true;
    PiUIManager piUi;
    private PiUI normalMenu;
    cameraController _cameraController;

    private void Start()
    {
        _cameraController = FindObjectOfType<cameraController>();

        piUi = FindObjectOfType<PiUIManager>();
        normalMenu = piUi.GetPiUIOf("Normal Menu");
        //piUi.ChangeMenuState("Normal Menu", new Vector2(Screen.width / 2f, Screen.height / 2f));

    }
    public void StartBuild(GameObject go)
    {
        buildSystem.NewBuild(go);
        TogglePanel();
    }

    public void TogglePanel()
    {
        showPanel = !showPanel;
        piUi.ChangeMenuState("Normal Menu", new Vector2(Screen.width / 2f, Screen.height / 2f));
        _cameraController.enabled = !_cameraController.enabled;


    }

    public void HidePanel()
    {
        piUi.ChangeMenuState("Normal Menu", new Vector2(Screen.width / 2f, Screen.height / 2f));
        _cameraController.enabled = !_cameraController.enabled;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && showPanel)
        {
            piUi.ChangeMenuState("Normal Menu", new Vector2(Screen.width / 2f, Screen.height / 2f));
            _cameraController.enabled = !_cameraController.enabled;
        }

    }


}
