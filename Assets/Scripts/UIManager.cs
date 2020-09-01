using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    private Stats stats;
    public Text moneyText;
    public Text levelText;
    public Slider xpSlider;
    public Text dateText;
    public Text monthText;
    public Text animalText;
    public Text happinessText;

    public static UIManager instance;
    private void Awake()
    {
        instance = this;

    }
    void Start()
    {
        stats = GetComponent<Stats>();
        UpdateValues();

    }

    // Update is called once per frame
    public void UpdateValues()
    {
        moneyText.text = "$ " + stats.Money.ToString();
        levelText.text = "Level: " + stats.Level.ToString();
        xpSlider.value = stats.XP;
        animalText.text = "Animals: " + stats.animalCount.ToString();
        happinessText.text = "Happiness: " + stats.happinessIndex().ToString() + "%";
    }
    public void setDayMonth(string day, string month)
    {
        dateText.text = day;
        monthText.text = month;
    }
}
