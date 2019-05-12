using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class GameController : MonoBehaviour
{
    // Obiekt menadżera aplikacji
    public AppManager appManager;

    // Obiekt Game Modela
    public GameModel gameModel;

    // Obiekt kontrolera HUD-a
    public HUDController hudcontroller;

    // View presenter dla ekranu końcowego
    public EndScreenViewPresenter endScreenViewPresenter;

    // Obiekt grida z kontenerami dla kółek
    public GridLayoutGroup grid;

    // Prefab kontenera dla kółek
    public GameObject containerPrefab;

    // Szybkość kurczenia się kółek
    public float shrinkSpeed = 2.0f;

    // Tablica kontrolerów kontenerów dla kółek
    private CircleContainerController[] circleContainers;

    // Liczba kontenerów na ekranie
    private int noOfCircleContainers;

    private bool isHighScoreSet;

    public bool IsGameOver { get; private set; }
    //public bool IsPaused { get; private set; }

    // Generator losowości
    Random rand;

    // obecna liczba punktów (zebranych kół)
    private int currentScore = 0;

    void Awake()
    {
        rand = new Random();

        IsGameOver = false;

        isHighScoreSet = false;

        if (shrinkSpeed == 0.0f)
            Debug.LogError("Shrink speed set to 0");

        noOfCircleContainers = countCircleContainers();
        Debug.Log(noOfCircleContainers);

        circleContainers = new CircleContainerController[noOfCircleContainers];

        // Znajdź Game Arena
        GameObject gameArena = GameObject.FindGameObjectWithTag("GameArena");

        for (int i = 0; i < noOfCircleContainers; i++)
        {
            GameObject containerObject = Instantiate(containerPrefab, gameArena.transform);

            circleContainers[i] = containerObject.GetComponent<CircleContainerController>();
        }
    }

    void Start()
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        // Przeprowadź "samouczek"
        if (!gameModel.DoneTutorial)
            yield return tutorialStage();
        else
            hudcontroller.IsTutorial = false;

        Debug.Log("Start proper gameplay");

        int noOfWaves = 6;
        int noOfSpawns = 4;
        float timeBetweenSpawns = 0.6f;
        float timeBetweenWaves = 1.0f;
        float ballLifetimeOffset = 3.0f;

        while (!IsGameOver)
        {
            for (int i = 0; i < noOfWaves && !IsGameOver; i++)
            {
                //while (IsPaused)
                //    yield return null;
                
                for (int j = 0; j < noOfSpawns && !IsGameOver; j++)
                {
                    //while (IsPaused)
                    //    yield return null;

                    int id = randomContainer();

                    while (id == -1)
                    {
                        yield return null;
                        id = randomContainer();
                    }

                    // Jeśli w trakcie losowania wolnego kontenera skończyła się gra, przerwij pętlę
                    if (IsGameOver)
                        break;

                    // Wyznacz czas życia kulki
                    float lifetime = (float)rand.NextDouble() + ballLifetimeOffset;

                    //// Wylosuj czarną kulkę
                    //if (rand.Next(0, 10) == 1)
                    //{
                    //    activateController(id, lifetime, false);
                    //}
                    //// Wylosuj zieloną kulkę
                    //else
                    //{
                    //    activateController(id, lifetime, true);
                    //}

                    activateController(id, lifetime, rand.Next(0, 10) != 1);

                    yield return new WaitForSeconds(timeBetweenSpawns);
                }

                yield return new WaitForSeconds(timeBetweenWaves);
            }

            if (timeBetweenWaves - 0.35f > 0.1f)
                timeBetweenWaves -= 0.35f;
            else
                timeBetweenWaves = 0.1f;

            if (timeBetweenSpawns - 0.1f > 0.1f)
                timeBetweenSpawns -= 0.1f;
            else
                timeBetweenSpawns = 0.1f;

            if (ballLifetimeOffset - 0.5f > 0.25f)
                ballLifetimeOffset -= 0.5f;
            else
                ballLifetimeOffset = 0.25f;

            if (noOfSpawns + 2 < 15)
                noOfSpawns += 2;
            else
                noOfSpawns = 15;
        }
    }

    // Funkcja obsługująca "przegranie" gry
    public void onLose()
    {
        IsGameOver = true;

        // Pobierz z hud kontrolera uzyskany czas
        float currentTime = hudcontroller.TimeElapsed;

        // Usuń kółka z planszy
        foreach ( CircleContainerController c in circleContainers )
        {
            StartCoroutine(c.deactivateAll());
        }

        // Przekaż wynik do Game Modela
        isHighScoreSet = gameModel.SetBestTime(currentTime);

        // Pokaż panel końcowy
        StartCoroutine(RevealEndPanel());
    }

    IEnumerator RevealEndPanel()
    {
        yield return new WaitForSeconds(1.0f);

        // Pokaż Panel końcowy
        StartCoroutine(endScreenViewPresenter.RevealEndScreen(isHighScoreSet));
    }

    // Funkcja obliczająca, ile kontenerów trzeba rozmieścić na ekranie
    // Będziemy rozmieszczać po 7 kontenerów w rzędzie, liczymy ile rzędów mieści się na ekranie
    // oraz ustawiamy odpowiedni padding dla grida
    int countCircleContainers()
    {
        // Oblicz rzeczywistą wysokość canvasa (przydatne w przypadku różnych rozdzielczości)
        int height = Screen.height * 720 / Screen.width;

        // 110px jest już zajęte przez HUD
        height -= 110;

        // Padding grida z góry i dołu jest przynajmniej 10px
        height -= 2 * 10;

        // Rozwiązuję nierówność: height = 95*n + 5*(n-1)
        // n to maksymalna liczba naturalna spełniająca nierówność (szukana liczba rzędów)
        // Odstęp między kontenerami w pionie wynosi 5px
        // Wysokość pojedynczego kontenera wynosi 95px
        int n = (height + 5) / 100;

        int gridHeight = n * 95 + 5 * (n - 1);

        // Oblicz dodatkowy padding z góry i dołu
        int addPadding = (height - gridHeight) / 2;

        grid.padding.bottom = 10 + addPadding;
        grid.padding.top = 10 + addPadding;

        return 7 * n;
    }

    // Funkcja aktualizująca wynik
    public void updateScore()
    {
        if (!hudcontroller.IsTutorial)
            currentScore++;

        // Zaktualizuj HUD
        hudcontroller.setScore(currentScore);
    }

    // Funkcja uruchamiająca dany kontroler
    private void activateController(int id, float lifetime, bool isTap)
    {
        if (isTap)
            circleContainers[id].activateTap(lifetime);
        else
            circleContainers[id].activateDeath(lifetime);
    }

    public void LoadMainMenu()
    {
        appManager.LoadMenuScreen();
    }

    // Funkcja zwracająca identyfikator niezajętego kontenera
    int randomContainer()
    {
        List<int> freeContainers = new List<int>();

        for (int i = 0; i < noOfCircleContainers; i++)
            if (!circleContainers[i].isActive)
                freeContainers.Add(i);

        if (freeContainers.Count == 0)
            return -1;

        int id = rand.Next(0, freeContainers.Count);

        return freeContainers[id];
    }

    // Korutyna uruchamiająca podstawowy samouczek
    IEnumerator tutorialStage()
    {
        // Pierwsze zielone kółko
        int id = randomContainer();
        activateController(id, 3.0f, true);

        while (!circleContainers[id].isClicked)
            yield return null;

        yield return new WaitForSeconds(0.5f);

        // Drugie zielone kółko
        id = randomContainer();
        activateController(id, 3.0f, true);

        while (!circleContainers[id].isClicked)
            yield return null;

        yield return new WaitForSeconds(0.5f);

        // Trzecie kółko jest czarne
        id = randomContainer();
        activateController(id, 4.0f, false);

        while (circleContainers[id].isActive)
            yield return null;

        yield return new WaitForSeconds(1.0f);

        Debug.Log("Tutorial has ended");
        hudcontroller.IsTutorial = false;
        gameModel.DoneTutorial = true;
    }
}
