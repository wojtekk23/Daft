using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Obsługuje widok ekranu Menu
/// 
/// Manipuluje gameObjectami ekranu Menu i ich komponentami
/// </summary>
public class MenuScreenViewPresenter : MonoBehaviour
{
    /// <summary>
    /// Game object z panelem z najlepszym wynikiem
    /// </summary>
    public GameObject highScorePanel;

    /// <summary>
    /// Game object z paskiem ładowania
    /// </summary>
    public GameObject sliderObject;

    /// <summary>
    /// Komoponent Slider paska łądowania sceny
    /// </summary>
    private Slider loadingBar;

    /// <summary>
    /// Kontroler sceny
    /// </summary>
    public MenuScreenController menuScreenController;

    void Start()
    {
        if (menuScreenController.HighScore == 0f)
            highScorePanel.SetActive(false);
        else
            highScorePanel.GetComponentInChildren<Text>().text = "Najlepszy czas:\n" + DisplayTime(menuScreenController.HighScore);

        sliderObject.SetActive(false);
    }

    /// <summary>
    /// Ustawia postęp ładowania w pasku ładowania
    /// </summary>
    /// <param name="progress">Postęp ładowania (0.0 - 1.0)</param>
    public void SetLoadingProgress(float progress)
    {
        loadingBar.value = progress;
    }

    /// <summary>
    /// Zwraca tekst z czasem w formacie m:ss
    /// </summary>
    /// <param name="time">Czas do przekonwertowania (w sekundach)</param>
    /// <returns>Tekst ze skonwertowanym czasem w formacie m:ss</returns>
    private string DisplayTime(float time)
    {
        int rawSeconds = (int)time;

        string seconds = string.Empty;

        if (rawSeconds % 60 < 10)
            seconds += "0";

        seconds += (rawSeconds % 60).ToString();

        return (rawSeconds / 60).ToString() + ":" + seconds;
    }

    /// <summary>
    /// Trigger przycisku "Graj"
    /// 
    /// Wysyła do kontrolera prośbę o zmianę sceny
    /// </summary>
    public void OnPlayButtonClick()
    {
        // Ukryj panel z wynikiem
        highScorePanel.SetActive(false);

        // Włącz slider
        sliderObject.SetActive(true);
        loadingBar = sliderObject.GetComponent<Slider>();

        // Wyślij informację o zmianie sceny do kontrolera
        menuScreenController.LoadGame();
    }
}
