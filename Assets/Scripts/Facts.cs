using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Facts : MonoBehaviour
{
    public Text factText;
    private string[] animalFacts = new string[] { "You Can Find a Well-Trained Pet at the Animal Shelter", "Shelter Pets Are Health-Screened Before Adoption", "Shelter Animals Make Great Pets,and Sometimes Great Stars. Most of the animals you see in movies and online are animals rescued by shelters", "Your dog often dreams about you", "Goats and cows have accents and moo or bleat differently based on their location", "Rats laugh when tickled", "Dolphins have names for each other", "Dogs nose prints are as unique as human fingerprints and can be used to identify them", "Crows play pranks on each other", "Sea otters hold each other while sleeping so that they do not drift apart", "Cats do not meow for communication, it is a method to attract the attention of humans", "Flamingos are actually white. Their diet turns them pink", "The British Monarch legally owns all the swans in the open british waters", "Cats cannot taste sugar as they do not have sweet taste buds", "Chillies and peppers do not affect birds", "The eagle screeching sound made popular by Hollywood is actually that of a red herring", "Some Cats are allergic to humans" };
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DisplayFact", 10, Random.Range(20, 40));
    }

    // Update is called once per frame
    void Update()
    {

    }
    void DisplayFact()
    {
        factText.text = animalFacts[Random.Range(0, animalFacts.Length)];
        Invoke("ClearFact", 10);
    }
    void ClearFact()
    {
        factText.text = "";
    }
}
