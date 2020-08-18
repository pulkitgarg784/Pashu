using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBuildPosition : MonoBehaviour
{
    public GameObject mousePointer;

    // Update is called once per frame
    void Update()
    {
        if (mousePointer != null)
        {
            mousePointer.transform.position = getSnappedPoint(getWorldPoint());
        }
    }
    public Vector3 getWorldPoint()
    {
        Camera cam = GetComponent<Camera>();
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
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
