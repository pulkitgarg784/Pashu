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

    int WallIndex;
    GameObject wallParent;
    public GameObject Walls;
    public int money = 100;
    public int cost = 10;
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
        if (money >= cost)
        {
            if (Input.GetMouseButtonDown(0)) { startBuilding(); }
            else if (Input.GetMouseButtonUp(0)) { endBuilding(); }
            else { if (isCreating) { updateWall(); } }
        }
        else
        {
            isCreating = false;

            _cameraController.enabled = true;

        }
    }

    void startBuilding()
    {
        isCreating = true;
        _cameraController.enabled = false;
        WallIndex++;
        wallParent = new GameObject();
        wallParent.transform.parent = Walls.transform;
        wallParent.name = "WallParent" + WallIndex.ToString();
        Vector3 startPos = buildPosition.getSnappedPoint(buildPosition.getWorldPoint());
        GameObject startPole = Instantiate(pole, startPos, Quaternion.identity, wallParent.transform);
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
        GameObject newPole = Instantiate(pole, current, Quaternion.identity, wallParent.transform);
        Vector3 mid = Vector3.Lerp(newPole.transform.position, lastpole.transform.position, 0.5f);
        GameObject newWall = Instantiate(wall, mid, Quaternion.identity, wallParent.transform);
        money -= cost;
        newWall.transform.LookAt(lastpole.transform);
        float dist = Vector3.Distance(newPole.transform.position, lastpole.transform.position);
        newWall.transform.localScale = new Vector3(newWall.transform.localScale.x, newWall.transform.localScale.y, dist);
        lastpole = newPole;
    }
}
