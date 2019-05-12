using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Obsługuje logikę pojedynczego kontenera (komórkę grida na planszy)
/// </summary>
public class CircleContainerController : MonoBehaviour
{
    /// <summary>
    /// Obiekt game kontrolera
    /// </summary>
    private GameController gameController;

    /// <summary>
    /// Czy aktywowane jest zielone kółko
    /// </summary>
    private bool activatedTap = false;
    /// <summary>
    /// Czy aktywowane jest czarne kółko
    /// </summary>
    private bool activatedDeath = false;

    /// <summary>
    /// Właściwość mówiące, czy kontener przetrzymuje jakiekolwiek kółko
    /// </summary>
    public bool isActive
    {
        get { return activatedTap || activatedDeath; }
    }

    /// <summary>
    /// Właściwość pozyskująca szybkość animacji kurczenia się koła
    /// </summary>
    public float ShrinkSpeed
    {
        get { return gameController.shrinkSpeed; }
    }

    /// <summary>
    /// Obiekt zielonego kółka
    /// </summary>
    private TapCircleViewPresenter tapCircle;
    /// <summary>
    /// Obiekt czarnego kółka
    /// </summary>
    private DeathCircleViewPresenter deathCircle;

    /// <summary>
    /// Własność mówiąca o tym, czy kontener został kilknięty i nadal znajduje się na nim obiekt
    /// </summary>
    public bool isClicked
    {
        get
        {
            if (!isActive)
                return false;
            else
            {
                if (activatedTap)
                    return tapCircle.IsClicked;
                else
                    return deathCircle.IsClicked;
            }
        }
    }

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

    /// <summary>
    /// Uruchom korutyny zmniejszające kółko i dezaktywujące go
    /// </summary>
    /// <returns></returns>
    public IEnumerator deactivateAll()
    {
        
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

    /// <summary>
    /// Uruchamia zielone kółko w kontenerze
    /// </summary>
    /// <param name="lifetime">Długość życia kółka w sekundach</param>
    public void activateTap(float lifetime)
    {
        activatedTap = true;

        tapCircle.lifetime = lifetime;

        tapCircle.gameObject.SetActive(true);
    }

    /// <summary>
    /// Wyłącza zielone kółko (wyłącza jego gameObjecta)
    /// </summary>
    public void deactivateTap()
    {
        activatedTap = false;
        tapCircle.gameObject.SetActive(false);
    }

    /// <summary>
    /// Uruchamia czarne kółko w kontenerze
    /// </summary>
    /// <param name="lifetime">Długość życia kółka w sekundach</param>
    public void activateDeath(float lifetime)
    {
        activatedDeath = true;

        deathCircle.lifetime = lifetime;

        deathCircle.gameObject.SetActive(true);
    }

    /// <summary>
    /// Wyłącza czarne kółko (wyłącza jego gameObjecta)
    /// </summary>
    public void deactivateDeath()
    {
        activatedDeath = false;
        deathCircle.gameObject.SetActive(false);
    }

    /// <summary>
    /// Funkcja uruchamiana, gdy klikniemy czarne kółko
    /// </summary>
    public void onDeathClick()
    {
        deactivateDeath();

        // PRZEGRYWAMY
        gameController.onLose();
    }

    /// <summary>
    /// Funkcja uruchamiana, gdy nie zdążymy kliknąć czarnego kółka
    /// </summary>
    public void onDeathMiss()
    {
        deactivateDeath();
    }

    /// <summary>
    /// Funkcja uruchamiana, gdy klikniemy zielone kółko
    /// </summary>
    public void onTapClick()
    {
        // Zaktualizuj wynik
        gameController.updateScore();

        deactivateTap();
    }

    /// <summary>
    /// Funkcja uruchamiana, gdy nie zdążymy kliknąć zielonego kółka
    /// </summary>
    public void onTapMiss()
    {
        deactivateTap();

        // WYŚLIJ INFO DO GAME CONTROLLER O TYM, ŻE PRZEGRALIŚMY    
        gameController.onLose();
    }
}
