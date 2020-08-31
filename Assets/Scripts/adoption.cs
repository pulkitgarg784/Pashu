using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class adoption : MonoBehaviour
{
    private Stats stats;
    public Text adoptionText;

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
                int fees = Random.Range(100, 300);
                stats.Money += fees;
                StartCoroutine(setText("Your " + animal.GetComponent<AnimalController>().animalType + " " + animal.GetComponent<AnimalController>().animalName + " was adopted for: $" + fees.ToString()));
                UIManager.instance.UpdateValues();
                Destroy(animal);
            }
        }
    }

    public IEnumerator setText(string str)
    {
        adoptionText.text = str;
        yield return new WaitForSeconds(5);
        adoptionText.text = "";
    }
}
