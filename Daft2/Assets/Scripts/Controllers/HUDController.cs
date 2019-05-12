using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Obsługuje logikę HUD-a
/// 
/// Rozporządza zmiany w widoku HUD-a
/// </summary>
public class HUDController : MonoBehaviour
{
    /// <summary>
    /// Komponent HUD View
    /// </summary>
    public HUDViewPresenter hudview;

    /// <summary>
    /// Obiekt kontroler gry
    /// </summary>
    public GameController gameController;

    /// <summary>
    /// Właściwość mówiąca, ile czasu upłynęło podczas obecnej rozgrywki
    /// </summary>
    public float TimeElapsed { get; private set; }

    /// <summary>
    /// Właściwość mówiąca, czy gra się zakończyła
    /// </summary>
    public bool GameOver
    {
        get
        {
            return gameController.IsGameOver;
        }
    }

    /// <summary>
    /// Właściwość mówiąca, czy nadal trwa samouczek
    /// </summary>
    public bool IsTutorial { get; set; }

    void Awake()
    {
        // Domyślnie samouczek jest włączony w każdej rozgrywce
        IsTutorial = true;

        TimeElapsed = 0f;
    }

    
    void Update()
    {
        if (!GameOver && !IsTutorial)
            SetTimer();
    }

    /// <summary>
    /// Aktualizuje czas gry (dodaje czas trwania jednej klatki) i aktualizuje obiekt wyświetlający czas
    /// </summary>
    public void SetTimer()
    {
        TimeElapsed += Time.deltaTime;

        hudview.setTimer(TimeElapsed);
    }

    /// <summary>
    /// Aktualizuje wynik
    /// </summary>
    /// <param name="score">Nowy wynik obecnej gry</param>
    public void SetScore(int score)
    {
        hudview.setScore(score);
    }
}
