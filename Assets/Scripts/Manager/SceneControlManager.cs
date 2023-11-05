using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControlManager : SingletonMonobehaviour<SceneControlManager>
{
    [SerializeField] private CanvasGroup loadingScreenCanvasGroup;
    [SerializeField] private Image loadingScreenImage;

    [SerializeField] private GameObject player;

    private readonly float loadingScreenDuration = 0.75f;

    private bool isLoadingScreenActive = default;
    public bool IsLoadingScreenActive => isLoadingScreenActive;

    private bool isLoadingScene = default;
    public bool IsLoadingScene => isLoadingScene;

    public SceneName CurrentActiveScene = default;
    public GameplayState CurrentGameplayState = default;

    //===========================================================================
    private void Start()
    {
        // Initial loading sequences
        loadingScreenImage.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        loadingScreenCanvasGroup.alpha = 1.0f;

        MainMenuGUI.Instance.SetContentActive(true);
        CurrentActiveScene = SceneName.MainMenu;
        CurrentGameplayState = GameplayState.Pause;

        StartCoroutine(LoadingScreen(0.0f));
    }

    //===========================================================================
    private IEnumerator UnloadAndSwitchScene(string sceneName, Vector3 spawnPosition)
    {
        EventManager.CallBeforeSceneUnloadLoadingScreenEvent();
        yield return StartCoroutine(LoadingScreen(1.0f));

        GameOverMenuGUI.Instance.SetContentActive(false);

        EventManager.CallBeforeSceneUnloadEvent();
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));
        EventManager.CallAfterSceneLoadEvent();

        Player.Instance.transform.position = spawnPosition;
        Player.Instance.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(1.0f);
        yield return StartCoroutine(LoadingScreen(0.0f));
        EventManager.CallAfterSceneLoadedLoadingScreenEvent();

        isLoadingScene = false;
        CurrentGameplayState = GameplayState.Ongoing;
    }

    //===========================================================================
    private IEnumerator LoadingScreen(float targetAlpha)
    {
        isLoadingScreenActive = true;

        loadingScreenCanvasGroup.blocksRaycasts = true;

        float _loadSpeed = Mathf.Abs(loadingScreenCanvasGroup.alpha - targetAlpha) / loadingScreenDuration;

        while (Mathf.Approximately(loadingScreenCanvasGroup.alpha, targetAlpha) == false)
        {
            loadingScreenCanvasGroup.alpha = Mathf.MoveTowards(loadingScreenCanvasGroup.alpha, targetAlpha, _loadSpeed * Time.deltaTime);
            yield return null;
        }

        isLoadingScreenActive = false;
        loadingScreenCanvasGroup.blocksRaycasts = false;
    }

    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

        SceneManager.SetActiveScene(newlyLoadedScene);
    }

    //===========================================================================
    private IEnumerator LoadDemoSceneHub()
    {
        EventManager.CallBeforeSceneUnloadLoadingScreenEvent();
        yield return StartCoroutine(LoadingScreen(1.0f));

        MainMenuGUI.Instance.SetContentActive(false);
        SaveSelectMenuGUI.Instance.SetContentActive(false);
        DemoOverMenuGUI.Instance.SetContentActive(false);

        yield return StartCoroutine(LoadSceneAndSetActive(SceneName.DemoSceneHub.ToString()));
        EventManager.CallAfterSceneLoadEvent();
        CurrentActiveScene = SceneName.DemoSceneHub;

        Player.Instance.transform.position = Vector3.zero;
        Player.Instance.gameObject.SetActive(true);

        StartCoroutine(LoadingScreen(0.0f));
        EventManager.CallAfterSceneLoadedLoadingScreenEvent();

        CurrentGameplayState = GameplayState.Ongoing;
    }

    private IEnumerator LoadMainMenu()
    {
        EventManager.CallBeforeSceneUnloadLoadingScreenEvent();
        yield return StartCoroutine(LoadingScreen(1.0f));

        PauseMenuGUI.Instance.SetContentActive(false);
        GameOverMenuGUI.Instance.SetContentActive(false);
        DemoOverMenuGUI.Instance.SetContentActive(false);

        EventManager.CallBeforeSceneUnloadEvent();
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        
        MainMenuGUI.Instance.SetContentActive(true);
        CurrentActiveScene = SceneName.MainMenu;

        yield return StartCoroutine(LoadingScreen(0.0f));
        EventManager.CallAfterSceneLoadedLoadingScreenEvent();

        CurrentGameplayState = GameplayState.Ongoing;
    }

    private IEnumerator LoadDemoDungeon()
    {
        EventManager.CallBeforeSceneUnloadLoadingScreenEvent();
        yield return StartCoroutine(LoadingScreen(1.0f));

        MainMenuGUI.Instance.SetContentActive(false);
        SaveSelectMenuGUI.Instance.SetContentActive(false);

        EventManager.CallBeforeSceneUnloadEvent();
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        yield return StartCoroutine(LoadSceneAndSetActive(SceneName.DemoSceneDungeon.ToString()));
        EventManager.CallAfterSceneLoadEvent();

        CurrentActiveScene = SceneName.DemoSceneDungeon;

        Player.Instance.transform.position = Vector3.zero;

        // yield return StartCoroutine(LoadMap());

        yield return StartCoroutine(LoadingScreen(0.0f));
        EventManager.CallAfterSceneLoadedLoadingScreenEvent();

        isLoadingScene = false;
        CurrentGameplayState = GameplayState.Ongoing;
    }

    //===========================================================================
    //===== LIST OF SCENE LOAD WRAPPER ==========================================
    //===========================================================================
    public void LoadScene(string sceneName, Vector3 spawnPosition)
    {
        if (isLoadingScreenActive == false)
        {
            isLoadingScene = true;
            CurrentGameplayState = GameplayState.Pause;
            StartCoroutine(UnloadAndSwitchScene(sceneName, spawnPosition));
        }
    }

    public void LoadMainMenuWrapper()
    {
        if (isLoadingScreenActive == false)
        {
            CurrentGameplayState = GameplayState.Pause;

            StartCoroutine(LoadMainMenu());
        }
    }

    public void LoadDemoSceneHubWrapper()
    {
        if (isLoadingScreenActive == false)
        {
            CurrentGameplayState = GameplayState.Pause;

            StartCoroutine(LoadDemoSceneHub());
        }
    }

    public void LoadDemoDungeonWrapper()
    {
        if (isLoadingScreenActive == false)
        {
            CurrentGameplayState = GameplayState.Pause;

            StartCoroutine(LoadDemoDungeon());
        }
    }

    public void RespawnPlayerAtHub()
    {
        if (isLoadingScreenActive == false)
        {
            CurrentGameplayState = GameplayState.Pause;

            MainMenuGUI.Instance.SetContentActive(false);
            PauseMenuGUI.Instance.SetContentActive(false);
            GameOverMenuGUI.Instance.SetContentActive(false);
            DemoOverMenuGUI.Instance.SetContentActive(false);

            SaveDataManager.Instance.LoadPlayerDataToRuntimeData(SaveDataSlot.save01);

            StartCoroutine(UnloadAndSwitchScene(SceneName.DemoSceneHub.ToString(), Vector3.zero));
        }
    }
}