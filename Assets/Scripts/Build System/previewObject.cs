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
    public bool snapToGrid;
    public float cost;
    private Stats stats;


    private void Start()
    {
        myRend = GetComponent<MeshRenderer>();
        stats = FindObjectOfType<Stats>();
        ChangeColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Building") || other.CompareTag("Wall") || other.CompareTag("Food"))
        {
            obj.Add(other.gameObject);
        }
        ChangeColor();
    }


    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Building") || other.CompareTag("Wall") || other.CompareTag("Food"))
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

        if (stats.Money >= cost)
        {
            stats.Money -= cost;
            Instantiate(prefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else { Debug.Log("no money left"); }

    }

    public bool CanBuild()
    {
        return canBuild;
    }


}
