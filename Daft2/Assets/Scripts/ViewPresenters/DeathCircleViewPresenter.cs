using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathCircleViewPresenter : TapCircleViewPresenter
{
    Image circleMask;

    public Sprite blackCircleSprite;

    protected override void Start()
    {
        base.Start();

        // Pokoloruj maskę na czarny kolor
        circleMask = GetComponentInChildren<Image>();

        Debug.Log(circleMask.gameObject);

        circleMask.sprite = blackCircleSprite;
    }

    override protected void OnEnable()
    {
        // Zainicjuj obiekt
        base.OnEnable();
    }

    protected override IEnumerator TriggerCircleClick()
    {
        // Uruchom animację kliknięcia
        clickAnimator.Play("circleShrink", 0);
        clickAnimator.speed = shrinkSpeed;

        yield return new WaitForSeconds(1.0f / shrinkSpeed);

        // Wyślij informację do kontrolera
        containerController.onDeathClick();
    }

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
