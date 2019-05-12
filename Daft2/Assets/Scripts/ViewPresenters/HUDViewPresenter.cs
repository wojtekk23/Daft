using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Obsługuje widok HUD-a
/// 
/// Zmienia teksty wyświetlające czas rozgrywki i liczbę klikniętych kółek
/// </summary>
public class HUDViewPresenter : MonoBehaviour
{
    /// <summary>
    /// Tekst przechowujący wynik
    /// </summary>
    private Text scoreText;

    /// <summary>
    /// Tekst timera
    /// </summary>    
    private Text timerText;

    void Start()
    {
        // Pobierz obiekty przechowujące teksty HUD-a
        scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
        timerText = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
    }

    /// <summary>
    /// Zmienia tekst wyniku tak, by wyświetlał nowo uzyskany wynik
    /// </summary>
    /// <param name="score">Nowo uzyskany wynik</param>
    public void setScore(int score)
    {
        scoreText.text = score.ToString();
    }

    /// <summary>
    /// Zmienia tekst czasu tak, by wyświetlał obecny czas gry
    /// </summary>
    /// <param name="timeElapsed">Obecny czas gry</param>
    public void setTimer(float timeElapsed)
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
