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
                stats.animalCount--;
                stats.XP += 100;
                int fees = Random.Range(50, 100) * stats.Level;
                stats.Money += fees;
                Debug.Log(animal.GetComponent<AnimalController>().animalName + " was adopted for: $" + fees.ToString());
                UIManager.instance.UpdateValues();
                Destroy(animal);
            }
        }
    }
}
