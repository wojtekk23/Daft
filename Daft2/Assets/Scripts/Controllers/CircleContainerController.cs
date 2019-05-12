using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleContainerController : MonoBehaviour
{
    // Obiekt game kontrolera
    private GameController gameController;

    private bool activatedTap = false;
    private bool activatedDeath = false;

    public bool isActive
    {
        get { return activatedTap || activatedDeath; }
    }

    public float ShrinkSpeed
    {
        get { return gameController.shrinkSpeed; }
    }

    // Obiekty odpowiednio zielonego i czarnego kółka
    private TapCircleViewPresenter tapCircle;
    private DeathCircleViewPresenter deathCircle;

    public bool isClicked
    {
        get
        {
            if (!isActive)
                return false;
            else
            {
                if (activatedTap)
                    return tapCircle.isClicked;
                else
                    return deathCircle.isClicked;
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        activatedTap = false;
        activatedDeath = false;

        // Przechwyć game kontrolera
        gameController = GameObject.FindGameObjectWithTag("Controllers").GetComponentInChildren<GameController>();

        // Przechywć zielone i czarne kółko, chcemy znaleźć te komponenty w nieaktywnych gameobjects
        tapCircle = GetComponentInChildren<TapCircleViewPresenter>(true);
        deathCircle = GetComponentInChildren<DeathCircleViewPresenter>(true);

        if (tapCircle == null || deathCircle == null)
        {
            Debug.LogError("Couldn't find tap or death circle (tapCircle: " + tapCircle + ", deathCircle: " + deathCircle);
        }
    }

    public IEnumerator deactivateAll()
    {
        // Uruchom korutyny zmniejszające kółko i dezaktywujące go
        if (activatedTap)
        {
            yield return tapCircle.TriggerCircleDisappear();
            deactivateTap();
        }
        else if (activatedDeath)
        {
            yield return deathCircle.TriggerCircleDisappear();
            deactivateDeath();
        }
    }

    public void activateTap(float lifetime)
    {
        activatedTap = true;

        tapCircle.lifetime = lifetime;

        tapCircle.gameObject.SetActive(true);
    }

    public void deactivateTap()
    {
        activatedTap = false;
        tapCircle.gameObject.SetActive(false);
    }

    public void activateDeath(float lifetime)
    {
        activatedDeath = true;

        deathCircle.lifetime = lifetime;

        deathCircle.gameObject.SetActive(true);
    }

    public void deactivateDeath()
    {
        activatedDeath = false;
        deathCircle.gameObject.SetActive(false);
    }

    // Funkcja uruchamiana, gdy klikniemy czarne kółko
    public void onDeathClick()
    {
        deactivateDeath();

        // PRZEGRYWAMY
        gameController.onLose();
    }

    // Funkcja uruchamiana, gdy nie zdążymy kliknąć czarnego kółka
    public void onDeathMiss()
    {
        deactivateDeath();
    }

    // Funkcja uruchamiana, gdy klikniemy zielone kółko
    public void onTapClick()
    {
        deactivateTap();

        gameController.updateScore();
    }

    // Funkcja uruchamiana, gdy nie zdążymy kliknąć zielonego kółka
    public void onTapMiss()
    {
        deactivateTap();

        // WYŚLIJ INFO DO GAME CONTROLLER O TYM, ŻE PRZEGRALIŚMY    
        gameController.onLose();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
