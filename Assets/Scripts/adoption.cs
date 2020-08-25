using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adoption : MonoBehaviour
{
    private Stats stats;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SelectForAdoption", 10, 10);
        stats = FindObjectOfType<Stats>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void SelectForAdoption()
    {
        if (transform.childCount > 9)
        {
            int rand = Random.Range(0, transform.childCount - 1);
            if (transform.GetChild(rand).gameObject != null)
            {
                GameObject animal = transform.GetChild(rand).gameObject;
                Debug.Log(animal.GetComponent<AnimalController>().animalName + " was adopted");
                stats.animalCount--;
                stats.XP += 100;
                UIManager.instance.UpdateValues();
                Destroy(animal);
            }
        }
    }
}
