using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSystem : MonoBehaviour
{

    public Camera cam;
    public LayerMask layer;

    public BuildSelector selector;

    private GameObject preview;
    private previewObject previewScript;

    private bool isBuilding = false;
    private void Start()
    {

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isBuilding && previewScript.CanBuild())
        {
            BuildIt();

        }

        if (Input.GetMouseButtonDown(1) && isBuilding)
        {
            StopBuild();

        }

        if (Input.GetKeyDown(KeyCode.R) && isBuilding)
        {
            preview.transform.Rotate(0f, 90f, 0f);
        }

        if (isBuilding)
        {
            DoRay();
        }
    }

    public void NewBuild(GameObject _go)
    {
        preview = Instantiate(_go, Vector3.zero, Quaternion.identity);
        previewScript = preview.GetComponent<previewObject>();

        isBuilding = true;
    }

    private void StopBuild()
    {
        Destroy(preview);
        preview = null;
        previewScript = null;
        isBuilding = false;
        //selector.TogglePanel();
        selector.showPanel = !selector.showPanel;
    }

    private void BuildIt()
    {
        previewScript.Build();
        StopBuild();
    }

    private void DoRay()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, layer))
        {
            PositionObj(hit.point);
        }
    }

    private void PositionObj(Vector3 _pos)
    {
        if (previewScript.snapToGrid)
        {
            float x = Mathf.Floor(_pos.x + 0.5f); //modiify the second +0.5f when you import actual models.
            //float y = Mathf.Floor(_pos.y + 0.5f); //modiify the second +0.5f when you import actual models.
            float z = Mathf.Floor(_pos.z + 0.5f);//modiify the second +0.5f when you import actual models.

            preview.transform.position = new Vector3(x, 0, z);
        }
        else { preview.transform.position = new Vector3(_pos.x, _pos.y, _pos.z); }

    }


    public bool GetIsBuilding()
    {
        return isBuilding;
    }

}


