using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreenController : MonoBehaviour
{
    // Menadżer aplikacji
    public AppManager appManager;

    // View presenter ekranu menu
    public MenuScreenViewPresenter menuScreenViewPresenter;

    public void LoadGame()
    {
        // Wyślij zapytanie do appManagera o ładowanie sceny z grą
        StartCoroutine(appManager.LoadGameScene());
    }

    public void SetLoadingProgress(float progress)
    {
        menuScreenViewPresenter.setLoadingProgress(progress);
    }
}
