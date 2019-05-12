using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Obsługuje wygląd i zachowanie na ekranie zielonego koła
/// </summary>
public class TapCircleViewPresenter : MonoBehaviour
{
    /// <summary>
    /// Czas życia koła
    /// </summary>
    public double lifetime = 3.0;

    /// <summary>
    /// Ile czasu zostało, zanim koło zniknie
    /// </summary>
    protected double timeLeft;

    /// <summary>
    /// Obiekt animatora czeronwej poświaty
    /// </summary>
    public Animator deathClockAnimator;

    /// <summary>
    /// Obiekt animatora dla animacji kliknięcia
    /// </summary>
    public Animator clickAnimator;

    /// <summary>
    /// Szybkość animacji po kliknięciu
    /// </summary>
    protected float shrinkSpeed = 2.0f;

    /// <summary>
    /// Obiekt kontenera kółka
    /// </summary>
    protected CircleContainerController containerController;

    /// <summary>
    /// Obiekt tekstu wyświetlającego pozostały czas życia komórki
    /// </summary>
    protected Text lifeText;

    /// <summary>
    /// Właściwość mówiąca, czy koło zostało kliknięte
    /// </summary>
    protected bool clicked;
    public bool IsClicked
    {
        get { return clicked; }
        private set { clicked = value; }
    }

    /// <summary>
    /// Czy nie zdążyliśmy kliknąć kołą w trakcie jego życia
    /// </summary>
    protected bool missed;

    protected virtual void Start()
    {
        IsClicked = false;

        timeLeft = lifetime;

        // Przejimij kontener zawierający kółko
        containerController = gameObject.GetComponentInParent<CircleContainerController>();

        shrinkSpeed = containerController.ShrinkSpeed;

        // Przejmij obiekt z liczbą pozostałych sekund
        lifeText = gameObject.GetComponentInChildren<Text>();

        if (deathClockAnimator != null)
        {
            // Odpowiednio spowolnij animację
            deathClockAnimator.Play("clockworkSlowerTest", 0);
            deathClockAnimator.speed = 1.0f / (float)lifetime;
        }

        if (clickAnimator != null)
        {
            clickAnimator.speed = 0;
        }
    }

    protected void OnEnable()
    {
        IsClicked = false;
        missed = false;

        // Przejimij kontener zawierający kółko
        containerController = gameObject.GetComponentInParent<CircleContainerController>();

        timeLeft = lifetime;

        if (deathClockAnimator != null)
        {
            // Zresetuj animację
            deathClockAnimator.enabled = false;
            deathClockAnimator.enabled = true;

            // Odpowiednio spowolnij animację
            deathClockAnimator.Play("clockworkSlowerTest", 0);
            deathClockAnimator.speed = 1.0f / (float)lifetime;
        }

        if (clickAnimator != null)
        {
            clickAnimator.enabled = false;
            clickAnimator.enabled = true;

            clickAnimator.speed = 0;
        }
    }

    protected void Update()
    {
        // Ustaw tekst na pozostałą ilość czasu
        if (timeLeft >= 1.0)
            lifeText.text = timeLeft.ToString("F1");
        else
            lifeText.text = timeLeft.ToString("F2");

        // Przegraliśmy
        if (timeLeft - Time.deltaTime < 0.0)
        {
            timeLeft = 0.0f;

            missed = true;

            DeactivateCircle();
        }
        // Zmniejsz czas życia kółka
        else if (!IsClicked && !missed)
            timeLeft -= Time.deltaTime;
    }

    /// <summary>
    /// Uruchamia dezaktywację koła
    /// </summary>
    protected void DeactivateCircle()
    {
        StartCoroutine(TriggerCircleMiss());
    }

    /// <summary>
    /// Obsługuje kliknięcie w kółko
    /// </summary>
    public void TapCircle()
    {
        if (!IsClicked && !missed)
        {
            IsClicked = true;

            // Zatrzymaj zegar
            deathClockAnimator.speed = 0;

            StartCoroutine("TriggerCircleClick");
        }
    }

    /// <summary>
    /// Uruchamia ukrywanie koła
    /// 
    /// Funkcja jest wywoływana podczas kończenia rozgrywki, aby usunąć wszystkie koła z ekranu
    /// </summary>
    public void CircleDisappear()
    {
        IsClicked = true;

        // Zatrzymaj zegar
        deathClockAnimator.speed = 0;

        StartCoroutine("TriggerCircleDisappear");
    }

    /// <summary>
    /// Korutyna uruchamiająca animację ukrywania koła
    /// </summary>
    /// <returns></returns>
    public IEnumerator TriggerCircleDisappear()
    {
        // Uruchom animację kliknięcia
        clickAnimator.Play("circleShrink", 0);
        clickAnimator.speed = shrinkSpeed;

        yield return new WaitForSeconds(1.0f / shrinkSpeed);
    }

    /// <summary>
    /// Korutyna ukrywająca koło z ekranu i informująca kontroler kontenera o kliknięciu koła
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator TriggerCircleClick()
    {
        yield return TriggerCircleDisappear();

        // Wyślij informację do kontrolera
        containerController.onTapClick();
    }

    /// <summary>
    /// Korutyna ukrywająca koło z ekranu i informująca kontroler kontenera o nie kliknięciu koła w czasie
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator TriggerCircleMiss()
    {
        yield return TriggerCircleDisappear();

        // Wyślij informację do kontrolera
        containerController.onTapMiss();
    }

    public override string ToString()
    {
        if (gameObject == null)
            return "null";
        else
            return gameObject.ToString();
    }
}
