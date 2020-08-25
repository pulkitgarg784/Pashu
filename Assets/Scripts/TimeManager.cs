using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class TimeManager : MonoBehaviour
{
    private DateTime theDate;
    public int dayOfYear = 1;
    void Start()
    {
        convertDaytoDate();
        InvokeRepeating("NextDay", 30, 30);
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
        if (theDate.Day == DateTime.DaysInMonth(theDate.Year, theDate.Month))
        {
            Debug.Log("Month End");
            Time.timeScale = 0;
        }
        dayOfYear++;
        convertDaytoDate();

    }
    void convertDaytoDate()
    {

        int year = DateTime.Now.Year; //Or any year you want
        theDate = new DateTime(year, 1, 1).AddDays(dayOfYear - 1);
        UIManager.instance.setDayMonth(theDate.Day.ToString(), CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(theDate.Month).ToUpper());
    }
}
