using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenViewPresenter : MonoBehaviour
{
    public Sprite sadSprite;
    public Sprite shockSprite;

    // Obiekt animatora dla ekranu końcowego
    public Animator endScreenAnimator;

    // Obiekt przycisku przechodzącego do ekranu głównego
    public Button button;

    public Image smileyImage;

    public GameObject buttonObject;

    public Text highScoreMessageText;

    public GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        button.interactable = false;

        endScreenAnimator.speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SolidifyEndScreen()
    {
        smileyImage.color = Color.white;

        Color brownish = new Color(146.0f / 256.0f, 97.0f / 256.0f, 85.0f / 256.0f);

        buttonObject.GetComponent<Image>().color = brownish;

        buttonObject.GetComponentInChildren<Text>().color = brownish; 

    }

    // Trigger przycisku ładującego główny ekran aplikacji
    public void OnEndButtonClick()
    {
        Debug.Log("MENU");

        gameController.LoadMainMenu();
    }

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
