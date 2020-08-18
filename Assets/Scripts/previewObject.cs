using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class previewObject : MonoBehaviour
{

    private List<GameObject> obj = new List<GameObject>();

    public Material goodMat;
    public Material badMat;
    public GameObject prefab;

    private MeshRenderer myRend;
    private bool canBuild = false;


    private void Start()
    {
        myRend = GetComponent<MeshRenderer>();
        ChangeColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Building") || other.CompareTag("Wall"))
        {
            obj.Add(other.gameObject);
        }
        ChangeColor();
    }


    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Building") || other.CompareTag("Wall"))
        {
            obj.Remove(other.gameObject);
        }


        ChangeColor();
    }



    private void ChangeColor()
    {
        if (obj.Count == 0)
        {
            myRend.material = goodMat;
            canBuild = true;
        }
        else
        {
            myRend.material = badMat;
            canBuild = false;
        }
    }

    public void Build()
    {


        Instantiate(prefab, transform.position, transform.rotation);
        Destroy(gameObject);


    }

    public bool CanBuild()
    {
        return canBuild;
    }

}
