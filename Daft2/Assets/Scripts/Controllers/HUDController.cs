using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    // Komponent HUD View
    public HUDViewPresenter hudview;
    public GameController gameController;
    public float TimeElapsed { get; private set; }

    public bool GameOver
    {
        get
        {
            return gameController.IsGameOver;
        }
    }
    public bool IsTutorial { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        IsTutorial = true;

        TimeElapsed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameOver && !IsTutorial)
            setTimer();
    }

    public void setTimer()
    {
        TimeElapsed += Time.deltaTime;

        hudview.setTimer(TimeElapsed);
    }

    public void setScore(int score)
    {
        hudview.setScore(score);
    }
}
