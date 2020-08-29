using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CreateWall : MonoBehaviour
{
    Stats stats;
    bool isCreating;
    ShowBuildPosition buildPosition;
    public GameObject pole;
    public GameObject wall;
    GameObject lastpole;
    cameraController _cameraController;
    public Text controls;

    int WallIndex;
    GameObject wallParent;
    public GameObject Walls;
    public float cost = 10;
    // Start is called before the first frame update

    private void OnEnable()
    {
        controls.text = "Drag LMB to build walls, Press RMB to stop";
    }
    private void OnDisable()
    {
        controls.text = "";

    }
    void Start()
    {
        buildPosition = GetComponent<ShowBuildPosition>();
        _cameraController = FindObjectOfType<cameraController>();
        stats = FindObjectOfType<Stats>();
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
        if (Input.GetMouseButtonUp(0))
        {
            _cameraController.enabled = true;

        }
        if (Input.GetMouseButtonDown(1))
        {
            isCreating = false;
            buildPosition.mousePointer.SetActive(false);
            buildPosition.enabled = false;
            this.enabled = false;
        }
        if (stats.Money >= cost)
        {
            if (Input.GetMouseButtonDown(0)) { startBuilding(); }
            else if (Input.GetMouseButtonUp(0))
            { endBuilding(); }
            else { if (isCreating) { updateWall(); } }
        }
        else
        {
            isCreating = false;
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
        stats.Money -= cost;
        UIManager.instance.UpdateValues();
        newWall.transform.LookAt(lastpole.transform);
        float dist = Vector3.Distance(newPole.transform.position, lastpole.transform.position);
        newWall.transform.localScale = new Vector3(newWall.transform.localScale.x, newWall.transform.localScale.y, dist);
        lastpole = newPole;
    }


}
