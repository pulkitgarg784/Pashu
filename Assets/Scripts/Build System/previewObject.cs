using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class previewObject : MonoBehaviour
{

    private List<GameObject> obj = new List<GameObject>();

    public Material goodMat;
    public Material badMat;

    public GameObject prefab;

    public MeshRenderer myRend;
    private bool canBuild = false;
    public bool snapToGrid;
    public float cost;
    private Stats stats;
    public Text control;

    private void OnEnable()
    {
        control = GameObject.Find("Controls").GetComponent<Text>();
        control.text = "LMB to place object, RMB to cancel, R to rotate.";
    }
    private void OnDisable()
    {
        control.text = "";
    }

    private void Start()
    {
        if (myRend == null)
        {
            myRend = GetComponent<MeshRenderer>();
        }
        stats = FindObjectOfType<Stats>();
        ChangeColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ground"))
        {
            obj.Add(other.gameObject);
        }
        ChangeColor();
    }


    private void OnTriggerExit(Collider other)
    {

        if (!other.CompareTag("Ground"))
        {
            obj.Remove(other.gameObject);
        }


        ChangeColor();
    }



    private void ChangeColor()
    {

        if (obj.Count == 0)
        {
            if (myRend != null)
            {
                myRend.material = goodMat;
            }
            canBuild = true;
        }
        else
        {
            if (myRend != null)
            {
                myRend.material = badMat;
            }
            canBuild = false;
        }

    }

    public void Build()
    {

        if (stats.Money >= cost)
        {
            stats.Money -= cost;
            UIManager.instance.UpdateValues();
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
