using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBuildPosition : MonoBehaviour
{
    public GameObject mousePointer;
    int layerMask;
    public Camera cam;
    private void Start()
    {
        layerMask = (1 << LayerMask.NameToLayer("Floor"));
    }
    void Update()
    {
        if (mousePointer != null)
        {
            mousePointer.transform.position = getSnappedPoint(getWorldPoint());
        }
    }
    public Vector3 getWorldPoint()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    public Vector3 getSnappedPoint(Vector3 originalPos)
    {
        Vector3 snapped = new Vector3(Mathf.Floor(originalPos.x + 0.5f), Mathf.Floor(originalPos.y + 0.5f), Mathf.Floor(originalPos.z + 0.5f));
        return snapped;
    }
}
