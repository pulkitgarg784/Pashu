using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    private bool creating;

    public GameObject StartPoint;
    public GameObject EndPoint;
    public GameObject WallObj;
    private GameObject wall;
    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        getInput();
    }

    void getInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            setStart();
        } else if (Input.GetMouseButtonUp(0))
        {
            setEnd();
        }
        else
        {
            if (creating)
            {
                adjust();
            }
        }
    }

    Vector3 SnapToGrid(Vector3 originalPos)
    {
        int gridScale = 1;
        Vector3 snappedPos= new Vector3(Mathf.Floor(originalPos.x/gridScale)*gridScale,originalPos.y,Mathf.Floor(originalPos.z/gridScale)*gridScale);
        return snappedPos;
    }
    void setStart()
    {
        creating = true;
        StartPoint.transform.position = SnapToGrid(getWorldPoint());
        wall = Instantiate(WallObj,StartPoint.transform.position,Quaternion.identity) as GameObject;
    }
    void setEnd()
    {
        creating = false;
        EndPoint.transform.position = SnapToGrid(getWorldPoint());

    }


    void adjust()
    {
        EndPoint.transform.position = SnapToGrid(getWorldPoint());
        adjustWall();
    }

    void adjustWall()
    {
        StartPoint.transform.LookAt(EndPoint.transform.position);
        EndPoint.transform.LookAt(StartPoint.transform.position);
        float dist = Vector3.Distance(StartPoint.transform.position, EndPoint.transform.position);
        wall.transform.position = StartPoint.transform.position + dist / 2 * StartPoint.transform.forward;
        wall.transform.rotation = StartPoint.transform.rotation;
        wall.transform.localScale = new Vector3(wall.transform.localScale.x,wall.transform.localScale.y,dist);
    }

    Vector3 getWorldPoint()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }

        return Vector3.zero;
    }
}
