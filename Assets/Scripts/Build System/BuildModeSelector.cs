using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildModeSelector : MonoBehaviour
{
    public ShowBuildPosition _showBuildPosition;
    public CreateWall _createWall;
    public GameObject buildpositionIndicator;
    BuildSelector _buildSelector;
    void Start()
    {
        _buildSelector = FindObjectOfType<BuildSelector>();
    }

    // Update is called once per frame
    public void setBuildmode(string buildmode)
    {

        if (buildmode == "Wall")
        {
            Debug.Log("wall!");
            buildpositionIndicator.SetActive(true);

            _showBuildPosition.enabled = true;
            _createWall.enabled = true;
            _buildSelector.HidePanel();
        }
        else
        {
            buildpositionIndicator.SetActive(false);
            _showBuildPosition.enabled = false;
            _createWall.enabled = false;
        }
    }
}
