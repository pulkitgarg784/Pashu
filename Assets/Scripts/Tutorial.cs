using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject[] panels;
    private int panelIndex;


    private void Start()
    {
        panelIndex = 0;
    }
    void Update()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (i == panelIndex)
            {
                panels[i].SetActive(true);
            }
            else
            {
                panels[i].SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            panelIndex++;
        }
        if (panelIndex >= panels.Length)
        {
            Destroy(gameObject);
        }
    }
}
