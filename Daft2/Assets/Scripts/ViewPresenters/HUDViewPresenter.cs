using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDViewPresenter : MonoBehaviour
{
    // Tekst przechowujący wynik
    private Text scoreText;

    // Tekst timera
    private Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        // Pobierz obiekty przechowujące teksty HUD-a
        scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
        timerText = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();

        //timerText.text = "0:00";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void setScore(int score)
    {
        scoreText.text = score.ToString();
    }

    internal void setTimer(float timeElapsed)
    {
        int minutes = (int)(timeElapsed / 60);
        int seconds = (int)timeElapsed % 60;

        String secs;

        if (seconds < 10)
            secs = "0" + seconds;
        else
            secs = seconds.ToString();

        timerText.text = minutes.ToString() + ":" + secs;
    }
}
