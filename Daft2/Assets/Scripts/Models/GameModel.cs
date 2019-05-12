using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Model gry.
/// 
/// Manipuluje danymi trwałymi (nadal dstępnych po wyłączeniu aplikacji)
/// </summary>
public class GameModel : MonoBehaviour
{
    /// <summary>
    /// Najlepszy wynik
    /// </summary>
    private float bestTime;

    /// <summary>
    /// Właściwość mówiąca, czy gracz w przeszłości odbył smaouczek
    /// </summary>
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

    /// <summary>
    /// Właściwość zwracająca obecnie najlepszy czas
    /// </summary>
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

        // Pobierz high score
        if (PlayerPrefs.HasKey("highscore"))
            bestTime = PlayerPrefs.GetFloat("highscore");
        else
            bestTime = 0f;

        Debug.Log("bestTime: " + bestTime);
    }

    /// <summary>
    /// Przyjmuje wynik zakończonej rozgrywki i aktualizuje najlepszy czas.
    /// 
    /// Jeśli czas zakończonej rozgrywki jest lepszy od obecnego najwyższego wyniku, zwraca true, wpp. false.
    /// </summary>
    /// <param name="gameTime">Czas zakończonej rozgrywki w sekundach</param>
    /// <returns>True - jeśli zaktualizowaliśmy najlepszy czas, false - jeśli nie było takiej potrzeby</returns>
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
