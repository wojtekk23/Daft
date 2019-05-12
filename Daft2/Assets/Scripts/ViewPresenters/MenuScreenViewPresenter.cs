using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreenViewPresenter : MonoBehaviour
{
    // Obiekt game Modela
    public GameModel gameModel;

    // Game object z panelem z najlepszym wynikiem
    public GameObject highScorePanel;

    // Game object z paskiem ładowania
    public GameObject sliderObject;

    // Komoponent Slider paska łądowania sceny
    private Slider loadingBar;

    // Kontroler sceny
    public MenuScreenController menuScreenController;

    // Start is called before the first frame update
    void Start()
    {
        if (gameModel.HighScore == 0f)
            highScorePanel.SetActive(false);
        else
            highScorePanel.GetComponentInChildren<Text>().text = "Najlepszy czas:\n" + displayTime(gameModel.HighScore);

        sliderObject.SetActive(false);
    }

    internal void setLoadingProgress(float progress)
    {
        loadingBar.value = progress;
    }

    private string displayTime(float highScore)
    {
        int rawSeconds = (int)highScore;

        string seconds = string.Empty;

        if (rawSeconds % 60 < 10)
            seconds += "0";

        seconds += (rawSeconds % 60).ToString();

        return (rawSeconds / 60).ToString() + ":" + seconds;
    }

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
