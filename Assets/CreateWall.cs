using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateWall : MonoBehaviour
{
    bool isCreating;
    ShowBuildPosition buildPosition;
    public GameObject pole;
    public GameObject wall;
    GameObject lastpole;
    cameraController _cameraController;
    // Start is called before the first frame update
    void Start()
    {
        buildPosition = GetComponent<ShowBuildPosition>();
        _cameraController = FindObjectOfType<cameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            getInput();
        }
    }
    void getInput()
    {
        if (Input.GetMouseButtonDown(0)) { startBuilding(); }
        else if (Input.GetMouseButtonUp(0)) { endBuilding(); }
        else { if (isCreating) { updateWall(); } }
    }

    void startBuilding()
    {
        isCreating = true;
        _cameraController.enabled = false;
        Vector3 startPos = buildPosition.getSnappedPoint(buildPosition.getWorldPoint());
        GameObject startPole = Instantiate(pole, startPos, Quaternion.identity);
        startPole.transform.position = new Vector3(startPos.x, startPos.y + 0.5f, startPos.z);
        lastpole = startPole;
    }
    void endBuilding()
    {
        isCreating = false;
        _cameraController.enabled = true;
    }
    void updateWall()
    {
        Vector3 current = buildPosition.getSnappedPoint(buildPosition.getWorldPoint());
        current = new Vector3(current.x, current.y + 0.5f, current.z);
        if (!current.Equals(lastpole.transform.position))
        {
            createWallSegement(current);
        }
    }
    void createWallSegement(Vector3 current)
    {
        GameObject newPole = Instantiate(pole, current, Quaternion.identity);
        Vector3 mid = Vector3.Lerp(newPole.transform.position, lastpole.transform.position, 0.5f);
        GameObject newWall = Instantiate(wall, mid, Quaternion.identity);
        newWall.transform.LookAt(lastpole.transform);
        lastpole = newPole;
    }
}
