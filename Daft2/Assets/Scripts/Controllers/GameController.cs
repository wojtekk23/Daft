using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

/// <summary>
/// Obsługuje logikę obecnie trwającej rozgrywki
/// 
/// Rozmieszcza koła na ekranie, rozporządza aktualizowaniem wyniku, itp.
/// </summary>
public class GameController : MonoBehaviour
{
    /// <summary>
    /// Obiekt menadżera aplikacji
    /// </summary>
    public AppManager appManager;

    /// <summary>
    /// Obiekt Game Modela
    /// </summary>
    public GameModel gameModel;

    /// <summary>
    /// Obiekt kontrolera HUD-a
    /// </summary>
    public HUDController hudcontroller;

    /// <summary>
    /// View presenter dla ekranu końcowego
    /// </summary>
    public EndScreenViewPresenter endScreenViewPresenter;

    /// <summary>
    /// Obiekt grida z kontenerami dla kółek
    /// </summary>
    public GridLayoutGroup grid;

    /// <summary>
    /// Prefab kontenera dla kółek
    /// </summary>
    public GameObject containerPrefab;

    /// <summary>
    /// Szybkość kurczenia się kółek
    /// </summary>
    public float shrinkSpeed = 2.0f;

    /// <summary>
    /// Tablica kontrolerów kontenerów dla kółek
    /// </summary>
    private CircleContainerController[] circleContainers;

    /// <summary>
    /// Liczba kontenerów na ekranie
    /// </summary>
    private int noOfCircleContainers;

    /// <summary>
    /// Czy w obecnej rozgrywce osiągnęliśmy najlepszy czas
    /// </summary>
    private bool isHighScoreSet;

    /// <summary>
    /// Własność mówiąca, czy nastąpił koniec obecnej rozgrywki
    /// </summary>
    public bool IsGameOver { get; private set; }

    /// <summary>
    /// Generator losowości
    /// </summary>
    Random rand;

    /// <summary>
    /// obecna liczba punktów (zebranych kół)
    /// </summary>
    private int currentScore = 0;

    void Awake()
    {
        rand = new Random();

        IsGameOver = false;

        isHighScoreSet = false;

        if (shrinkSpeed == 0.0f)
            Debug.LogError("Shrink speed set to 0");

        // Policz, ile kontenerów ma być na planszy
        noOfCircleContainers = countCircleContainers();
        Debug.Log(noOfCircleContainers);

        // Zainicjuj tablicę kontenerów
        circleContainers = new CircleContainerController[noOfCircleContainers];

        // Znajdź Game Arena
        GameObject gameArena = GameObject.FindGameObjectWithTag("GameArena");

        // Stwórz kontenery i przypisz je do tablicy
        for (int i = 0; i < noOfCircleContainers; i++)
        {
            GameObject containerObject = Instantiate(containerPrefab, gameArena.transform);

            circleContainers[i] = containerObject.GetComponent<CircleContainerController>();
        }
    }

    void Start()
    {
        // Rozpocznij grę
        StartCoroutine(StartGame());
    }

    /// <summary>
    /// Korutyna przeprowadzająca rozgrywkę
    /// </summary>
    /// <returns></returns>
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
        float timeBetweenSpawns = 0.7f;
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

                    // Wylosuj kulkę (czarna z prawdopodobieństwem 1/10)
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

    /// <summary>
    /// Funkcja obsługująca "przegranie" gry
    /// </summary>
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

    /// <summary>
    /// Korutyna uruchamiająca panel końcowy (po zakończonej rozgrywce)
    /// </summary>
    /// <returns></returns>
    IEnumerator RevealEndPanel()
    {
        yield return new WaitForSeconds(1.0f);

        // Pokaż Panel końcowy
        StartCoroutine(endScreenViewPresenter.RevealEndScreen(isHighScoreSet));
    }

    /// <summary>
    /// Funkcja obliczająca, ile kontenerów trzeba rozmieścić na ekranie
    /// 
    /// Będziemy rozmieszczać po 7 kontenerów w rzędzie, liczymy ile rzędów mieści się na ekranie
    /// oraz ustawiamy odpowiedni padding dla grida
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Funkcja aktualizująca wynik obecnej gry
    /// </summary>
    public void updateScore()
    {
        if (!hudcontroller.IsTutorial)
            currentScore++;

        // Zaktualizuj HUD
        hudcontroller.SetScore(currentScore);
    }

    /// <summary>
    /// Funkcja uruchamiająca kontroler danego kółka
    /// 
    /// Na ekranie, w kontenerze o podanym ID, pojawia się kółko określonego koloru
    /// </summary>
    /// <param name="id">ID kontenera, które ma przetrzymywać nowe koło</param>
    /// <param name="lifetime">Czas życia nowego koła</param>
    /// <param name="isTap">Czy koło ma być zielone</param>
    private void activateController(int id, float lifetime, bool isTap)
    {
        if (isTap)
            circleContainers[id].activateTap(lifetime);
        else
            circleContainers[id].activateDeath(lifetime);
    }

    /// <summary>
    /// Wyślij do menadżera aplikacji prośbę o załadowanie ekranu Menu
    /// </summary>
    public void LoadMainMenu()
    {
        appManager.LoadMenuScreen();
    }

    /// <summary>
    /// Funkcja zwracająca identyfikator losowego, niezajętego kontenera
    /// </summary>
    /// <returns>ID niezajętego kontenera</returns>
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

    /// <summary>
    /// Korutyna uruchamiająca podstawowy samouczek
    /// </summary>
    /// <returns></returns>
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
