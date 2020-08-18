using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSelector : MonoBehaviour
{

    public GameObject buildPanel;
    public BuildSystem buildSystem;
    private bool showPanel = true;



    public void StartBuild(GameObject go)


    {
        buildSystem.NewBuild(go);
        TogglePanel();
    }

    public void TogglePanel()
    {
        showPanel = !showPanel;
        buildPanel.SetActive(showPanel);
    }

}
