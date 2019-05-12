using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Obsługuje wygląd i zachowanie na ekranie czarnego koła
/// </summary>
public class DeathCircleViewPresenter : TapCircleViewPresenter
{
    /// <summary>
    /// Komponent obrazu koła
    /// </summary>
    Image circleMask;

    /// <summary>
    /// Sprite czarnego koła
    /// </summary>
    public Sprite blackCircleSprite;

    protected override void Start()
    {
        base.Start();

        circleMask = GetComponentInChildren<Image>();

        Debug.Log(circleMask.gameObject);

        circleMask.sprite = blackCircleSprite;
    }

    /// <summary>
    /// Korutyna ukrywająca koło z ekranu i informująca kontroler kontenera o kliknięciu koła
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator TriggerCircleClick()
    {
        // Uruchom animację kliknięcia
        clickAnimator.Play("circleShrink", 0);
        clickAnimator.speed = shrinkSpeed;

        yield return new WaitForSeconds(1.0f / shrinkSpeed);

        // Wyślij informację do kontrolera
        containerController.onDeathClick();
    }

    /// <summary>
    /// Korutyna ukrywająca koło z ekranu i informująca kontroler kontenera o nie kliknięciu koła w czasie
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator TriggerCircleMiss()
    {
        // Uruchom animację kliknięcia
        clickAnimator.Play("circleShrink", 0);
        clickAnimator.speed = shrinkSpeed;

        yield return new WaitForSeconds(1.0f / shrinkSpeed);

        // Wyślij informację do kontrolera
        containerController.onDeathMiss();
    }

}
