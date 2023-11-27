using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimerManager : StaticInstance<TimerManager>
{
    public static Action<bool> ChangeLightColor;

    [SerializeField] private TMP_Text text;
    [SerializeField] private float minuteToReal;

    [SerializeField] private GameObject EndUI;
    [SerializeField] private TMP_Text EndUIText;

    private int day;
    private int hour;
    private int minute;
    private int totalMinute;


    private float timer;

    private string ampm;

    private bool increase = true;

    public TMP_Text Text { get => text; private set => text = value; }
    public int Day { get => day; private set => day = value; }
    public int Hour { get => hour; private set => hour = value; }
    public int Minute { get => minute; private set => minute = value; }
    public int TotalMinute { get => totalMinute; set => totalMinute = value; }

    // Start is called before the first frame update
    void Start()
    {
        ResetTimer();
        ShowTimerUI();
        timer = minuteToReal;
        ampm = "am";
    }

    // Update is called once per frame
    void Update()
    {
        if (minute == 15 || minute == 30 || minute == 45 || minute == 0)
        {
            //OnChangeLightColor();
            ShowTimerUI();
        }
        //ShowTimerUI();
        if (!GameManager.Instance.isPaulsed)
        {
            if (text.text == "")
            {
                text.text = $"Day {day} {hour}:{minute}{ampm}";
            }
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                minute++;
                totalMinute++;
                if (minute >= 60)
                {
                    OnChangeLightColor();
                    if (hour == 11 && ampm == "pm")
                    {
                        ResetTimer();
                        // End UI
                        EndUI.SetActive(true);
                        EndUIText.text = $"Your Score is: {GameManager.Instance.karmaScore[GameManager.Instance.playTime - 1]}";
                        UIManager.Instance.UpdateChart();
                        GameManager.Instance.isPaulsed = true;
                    }
                    if (hour != 12)
                    {
                        hour++;
                        if (hour == 12)
                        {
                            increase = false;
                            ampm = "pm";
                        }
                    }
                    else
                    {
                        if (!GameManager.Instance.isPaulsed)
                            hour = 1;
                    }
                    minute = 0;
                }

                timer = minuteToReal;
            }
        }
        else
        {
            text.text = "";
        }
    }

    public void OnChangeLightColor()
    {
        ChangeLightColor?.Invoke(increase);
    }

    public void ResetTimer()
    {
        day = 1;
        hour = 12;
        minute = 0;
        totalMinute = 0;
        ampm = "am";
        increase = true;

    }

    private void ShowTimerUI()
    {
        text.text = $"Day {day} {hour}:{minute}{ampm}";
    }
}
