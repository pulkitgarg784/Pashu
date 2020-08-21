using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class cameraController : MonoBehaviour
{
    public static cameraController instance;
    public Transform followTransform;
    public Transform CameraTransform;

    public float moveSpeed;

    public float moveTime;
    public float rotationAmount;
    public Vector3 zoomAmount;

    private Vector3 newPos;
    public Vector3 newZoom;
    private Quaternion newRot;

    //mouse
    private Vector3 dragStart;
    private Vector3 dragEnd;

    private Vector3 RotateStart;
    private Vector3 RotateEnd;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        newPos = transform.position;
        newRot = transform.rotation;
        newZoom = CameraTransform.localPosition;

    }

    // Update is called once per frame
    void Update()
    {
        if (followTransform != null)
        {
            transform.position = followTransform.position;
        }

        HandleInput();
        MouseInput();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            followTransform = null;
        }
    }

    void HandleInput()
    {
        newZoom.y = Mathf.Clamp(newZoom.y, 10, 150);
        newZoom.z = Mathf.Clamp(newZoom.z, -150, -10);
        if (followTransform == null)
        {
            if (Mathf.Abs(Input.GetAxis("Vertical")) > 0)
            {
                newPos += transform.forward * moveSpeed * Input.GetAxis("Vertical");
            }
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
            {
                newPos += transform.right * moveSpeed * Input.GetAxis("Horizontal");
            }

            if (Input.GetKey(KeyCode.Q))
            {
                newRot *= Quaternion.Euler(Vector3.up * rotationAmount);
            }
            if (Input.GetKey(KeyCode.E))
            {
                newRot *= Quaternion.Euler(Vector3.up * -rotationAmount);
            }

            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * moveTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * moveTime);
        }

        CameraTransform.localPosition = Vector3.Lerp(CameraTransform.localPosition, newZoom, Time.deltaTime * moveTime);
    }

    void MouseInput()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {


            if (Input.mouseScrollDelta.y != 0)
            {
                newZoom += Input.mouseScrollDelta.y * zoomAmount;
            }
            if (followTransform == null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Plane plane = new Plane(Vector3.up, Vector3.zero);
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    float start;
                    if (plane.Raycast(ray, out start))
                    {
                        dragStart = ray.GetPoint(start);
                    }
                }

                if (Input.GetMouseButton(0))
                {
                    Plane plane = new Plane(Vector3.up, Vector3.zero);
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    float point;
                    if (plane.Raycast(ray, out point))
                    {
                        dragEnd = ray.GetPoint(point);
                        newPos = transform.position + (dragStart - dragEnd);
                    }
                }

                if (Input.GetMouseButtonDown(2))
                {
                    RotateStart = Input.mousePosition;
                }

                if (Input.GetMouseButton(2))
                {
                    RotateEnd = Input.mousePosition;
                    Vector3 difference = RotateStart - RotateEnd;
                    RotateStart = RotateEnd;

                    //newRot *= Quaternion.Euler(Vector3.right * (-difference.y / 5f)); //this is for up-down rotation
                    newRot *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));


                }
            }
        }
    }
}