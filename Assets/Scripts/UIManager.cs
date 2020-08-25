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

    public static UIManager instance;
    private void Awake()
    {
        instance = this;

    }
    void Start()
    {
        stats = GetComponent<Stats>();
    }

    // Update is called once per frame
    public void UpdateValues()
    {
        moneyText.text = stats.Money.ToString();
        levelText.text = stats.Level.ToString();
        xpSlider.value = stats.XP;
        animalText.text = "Animals: " + stats.animalCount.ToString();
    }
}
