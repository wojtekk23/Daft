using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModel : MonoBehaviour
{
    private float bestTime;
    public bool DoneTutorial
    {
        get
        {
            return PlayerPrefs.HasKey("tutorial") && PlayerPrefs.GetInt("tutorial") == 1;
        }
        set
        {
            PlayerPrefs.SetInt("tutorial", value ? 1 : 0);
        }
    }

    public float HighScore
    {
        get
        {
            if (PlayerPrefs.HasKey("highscore"))
                return PlayerPrefs.GetFloat("highscore");
            else
                return 0f;
        }
    }

    void Awake()
    {
        Debug.Log(Screen.height);
        Debug.Log(Screen.width);
        Debug.Log("tut: " + DoneTutorial);

        // Pobierz high score
        if (PlayerPrefs.HasKey("highscore"))
            bestTime = PlayerPrefs.GetFloat("highscore");
        else
            bestTime = 0;

        Debug.Log("bestTime: " + bestTime);
    }

    // zwraca true, gdy udało ustawić się najlepszy wynik
    public bool SetBestTime(float gameTime)
    {
        if (gameTime <= bestTime)
            return false;
        else
        {
            bestTime = gameTime;

            PlayerPrefs.SetFloat("highscore", bestTime);
            PlayerPrefs.Save();

            return true;
        }
    }

}
