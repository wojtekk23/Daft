using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Obsługuje logikę ekranu Menu
/// </summary>
public class MenuScreenController : MonoBehaviour
{
    /// <summary>
    /// Menadżer aplikacji
    /// </summary>    
    public AppManager appManager;

    /// <summary>
    /// View presenter ekranu menu
    /// </summary>
    public MenuScreenViewPresenter menuScreenViewPresenter;

    /// <summary>
    /// Obiekt modelu gry
    /// </summary>
    public GameModel gameModel;

    /// <summary>
    /// Właściwość zwracająca obecnie najwyższy wynik
    /// </summary>
    public float HighScore
    {
        get
        {
            return gameModel.HighScore;
        }
    }

    /// <summary>
    /// Wysyła do menadżera aplikacji zapytanie o wczytanie ekranu gry
    /// </summary>
    public void LoadGame()
    {
        // Wyślij zapytanie do appManagera o ładowanie sceny z grą
        StartCoroutine(appManager.LoadGameScene());
    }

    /// <summary>
    /// Wysyła zapytanie o zaktualizowania paska ładowania
    /// </summary>
    /// <param name="progress">Aktualny postęp ładowania (0.0 - 1.0)</param>
    public void SetLoadingProgress(float progress)
    {
        menuScreenViewPresenter.SetLoadingProgress(progress);
    }
}
