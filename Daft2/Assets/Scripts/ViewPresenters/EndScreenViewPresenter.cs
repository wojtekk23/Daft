using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Obsługuje i aktualizuje ekran końcowy gry
/// </summary>
public class EndScreenViewPresenter : MonoBehaviour
{
    /// <summary>
    /// Sprite ze smutną buźką
    /// </summary>
    public Sprite sadSprite;

    /// <summary>
    /// Sprite z zaskoczoną buźką
    /// </summary>
    public Sprite shockSprite;

    /// <summary>
    /// Obiekt animatora dla ekranu końcowego
    /// </summary>
    public Animator endScreenAnimator;

    /// <summary>
    /// Obiekt przycisku przechodzącego do ekranu głównego
    /// </summary>
    public Button button;

    /// <summary>
    /// Komponent przypisany game objectowi buźki
    /// </summary>
    public Image smileyImage;

    /// <summary>
    /// Game object z przyciskiem
    /// </summary>
    public GameObject buttonObject;

    /// <summary>
    /// Tekst wiadomości o najlepszym wyniku
    /// </summary>
    public Text highScoreMessageText;

    /// <summary>
    /// Obiekt game kontrolera
    /// </summary>
    public GameController gameController;

    void Start()
    {
        // Wyłącz interaktywność przycisku
        button.interactable = false;

        // Wyłącz animację pojawiania się ekranu końcowego
        endScreenAnimator.speed = 0;
    }

    /// <summary>
    /// Trigger przycisku ładującego główny ekran aplikacji
    /// 
    /// Wysyła do game kontrolera prośbę o załadowanie ekranu menu
    /// </summary>
    public void OnEndButtonClick()
    {
        Debug.Log("MENU");

        gameController.LoadMainMenu();
    }

    /// <summary>
    /// Korutyna odkrywająca ekran końcowy po zakończonej rozgrywce
    /// </summary>
    /// <param name="isHighScoreSet">Czy ustanowiono nowy najlepszy wynik</param>
    /// <returns></returns>
    public IEnumerator RevealEndScreen(bool isHighScoreSet)
    {
        Debug.Log("REVEAL END SCREEN (highscore: " + isHighScoreSet + ")");

        // Ustaw odpowiedni sprite buźki w zaleźności od uzyskanego wyniku
        if (isHighScoreSet)
            smileyImage.overrideSprite = shockSprite;

        // Włącz przycisk przechodzący do ekranu głównego
        button.interactable = true;

        // Włącz raycasting na przycisku
        buttonObject.GetComponent<Image>().raycastTarget = true;

        //Uruchom animację uruchamiania ekranu końcowego
        if (!isHighScoreSet)
        {
            highScoreMessageText.text = "Koniec gry";
        }

        endScreenAnimator.Play("endScreenReveal");
        endScreenAnimator.speed = 1.0f;

        yield return new WaitForSeconds(1f);
    }
}
