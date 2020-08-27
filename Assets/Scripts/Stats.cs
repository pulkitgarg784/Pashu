using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Stats : MonoBehaviour
{
    public float Money;
    public float XP;
    public int Level;
    public int animalCount;
    public int happiness;
    private void onEnable()
    {
        XP = 0;
        Level = 0;
        animalCount = 0;
    }
    private void Update()
    {
        if (XP >= 100)
        {
            XP = XP - 100;
            Level++;
        }
        if (XP < 0)
        {
            XP = 100 + XP;
            Level--;
        }
        if (Money < 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
    public float happinessIndex()
    {
        if (animalCount != 0)
        {
            return happiness / animalCount;
        }
        return 0;
    }

}
