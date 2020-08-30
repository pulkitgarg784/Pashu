using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    private DateTime theDate;
    private Stats stats;
    public int dayOfYear = 1;
    public GameObject costPanel;
    public Text costtext;
    int bill;
    bool stopTime;
    void Start()
    {
        convertDaytoDate();
        InvokeRepeating("NextDay", 30, 30);
        stats = FindObjectOfType<Stats>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Period))
        {
            Time.timeScale = 10;
        }
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            Time.timeScale = 1;

        }
    }
    void NextDay()
    {
        if (!stopTime)
        {
            if (theDate.Day == DateTime.DaysInMonth(theDate.Year, theDate.Month))
            {
                Debug.Log("Month End");
                costPanel.SetActive(true);
                stopTime = true;
                int govt = stats.animalCount * 100;
                int donations = UnityEngine.Random.Range(100, 500);
                int rent = UnityEngine.Random.Range(1000, 2000);
                bill = govt + donations - rent;
                costtext.text = "Govt. funds : $" + govt.ToString() + Environment.NewLine + "Donations: $" + donations.ToString() + Environment.NewLine + "Rent and costs: $" + rent.ToString() + Environment.NewLine + "Total: $" + bill.ToString();
                Time.timeScale = 0.01f;
            }
            dayOfYear++;
            convertDaytoDate();
        }

    }
    void convertDaytoDate()
    {
        int year = DateTime.Now.Year; //Or any year you want
        theDate = new DateTime(year, 1, 1).AddDays(dayOfYear - 1);
        UIManager.instance.setDayMonth(theDate.Day.ToString(), CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(theDate.Month).ToUpper());
    }
    public void PayBills()
    {
        stats.Money += bill;
        UIManager.instance.UpdateValues();
        stopTime = false;
    }
}
