using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapCircleViewPresenter : MonoBehaviour
{
    public double lifetime = 3.0;

    protected double timeLeft;

    // Obiekt animatora czeronwej poświaty
    public Animator deathClockAnimator;

    // Obiekt animatora dla animacji kliknięcia
    public Animator clickAnimator;

    // Szybkość animacji po kliknięciu
    protected float shrinkSpeed = 2.0f;

    // Obiekt kontenera kółka
    protected CircleContainerController containerController;

    // Obiekt tekstu wyświetlającego pozostały czas życia komórki
    protected Text lifeText;

    protected bool clicked;
    public bool isClicked
    {
        get { return clicked; }
    }

    protected bool missed;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        clicked = false;

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

    virtual protected void OnEnable()
    {
        clicked = false;
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

    // Update is called once per frame
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
        else if (!clicked && !missed)
            timeLeft -= Time.deltaTime;
    }

    protected void DeactivateCircle()
    {
        StartCoroutine(TriggerCircleMiss());
    }

    public void TapCircle()
    {
        if (!clicked)
        {
            clicked = true;

            // Zatrzymaj zegar
            deathClockAnimator.speed = 0;

            StartCoroutine("TriggerCircleClick");
        }
    }

    public void CircleDisappear()
    {
        clicked = true;

        // Zatrzymaj zegar
        deathClockAnimator.speed = 0;

        StartCoroutine("TriggerCircleDisappear");
    }

    public IEnumerator TriggerCircleDisappear()
    {
        // Uruchom animację kliknięcia
        clickAnimator.Play("circleShrink", 0);
        clickAnimator.speed = shrinkSpeed;

        yield return new WaitForSeconds(1.0f / shrinkSpeed);

        //containerController.deactivateTap();
    }

    protected virtual IEnumerator TriggerCircleClick()
    {
        // Uruchom animację kliknięcia
        clickAnimator.Play("circleShrink", 0);
        clickAnimator.speed = shrinkSpeed;

        yield return new WaitForSeconds(1.0f / shrinkSpeed);

        // Wyślij informację do kontrolera
        containerController.onTapClick();
    }


    protected virtual IEnumerator TriggerCircleMiss()
    {
        // Uruchom animację kliknięcia
        clickAnimator.Play("circleShrink", 0);
        clickAnimator.speed = shrinkSpeed;

        yield return new WaitForSeconds(1.0f / shrinkSpeed);

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
