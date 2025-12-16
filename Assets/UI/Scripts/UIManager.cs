using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private VisualElement root;

    private VisualElement mainMenu;
    Button startButton;

    private VisualElement hud;
    Button pauseButton;
    ProgressBar healthBar;
    ProgressBar skillBar;

    private VisualElement pauseMenu;
    Button resumeGame;
    Button QuitPause;

    public static bool IsPaused { get; private set; }
    public static bool gameStarted { get; private set; }

    void Awake()
    {
        Time.timeScale = 0f;
        IsPaused = true;
        gameStarted = false;
        Debug.Log("Game Paused now");

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        var doc = GetComponent<UIDocument>();
        root = doc.rootVisualElement;

        mainMenu = root.Q<VisualElement>("Panel");
        hud = root.Q<VisualElement>("TopBar");
        pauseMenu = root.Q<VisualElement>("PauseMenu");

        

        //main menu buttons
        startButton = root.Q<Button>("StartButton");
        startButton.clicked += startGame; //ShowHUD;

        //hud menu buttons
        pauseButton = root.Q<Button>("Pause");
        pauseButton.clicked += ShowPauseMenu;
        healthBar = root.Q<ProgressBar>("HealthBar");
        skillBar = root.Q<ProgressBar>("SkillBar");
        healthBar.value = 100;
        skillBar.value = 100;

        //pause menu buttons
        resumeGame = root.Q<Button>("Resume");
        resumeGame.clicked += Resume;

        QuitPause = root.Q<Button>("Quit");
        QuitPause.clicked += ShowMainMenu;
        ShowMainMenu();

    }
    void SetCursorForMenu()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
    }

    void SetCursorForGameplay()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }

    public void OnTimelineSignal()
    {
        Debug.Log("Signal reached UIManager");
        skillBar.value = 0;
        
        ShowHUD();
    }
    public void OnWinningGame()
    {
        ShowHUD();
        skillBar.value += 50;
    }
    public void OnEnteringGame()
    {
        hud.RemoveFromClassList("active");
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        //pauseMenu.AddToClassList("active");
        //skillBar.value = 10;
    }
    void HideAll()
    {
        Debug.Log("nehideee ger");
        mainMenu.RemoveFromClassList("active");
        hud.RemoveFromClassList("active");
        pauseMenu.RemoveFromClassList("active");

    }

    public void ShowMainMenu()
    {
        Debug.Log("ner manager");
        HideAll();
        SetCursorForMenu();
        SceneManager.LoadScene(0);
        healthBar.value = 100;
        skillBar.value = 100;
        mainMenu.AddToClassList("active");
    }
    public void ShowPauseMenu()
    {
        Debug.Log("pause menu");
        if (IsPaused) return;

        Time.timeScale = 0f;
        IsPaused = true;

        Debug.Log("Game Paused");
        HideAll();
        SetCursorForMenu();
        pauseMenu.AddToClassList("active");
    }
    public void Resume()
    {
        
        if (!IsPaused) return;
        IsPaused = false;
        ShowHUD();
        Time.timeScale = 1f;
        Debug.Log("Game Resumed");
    }
    public void ShowHUD()
    {
        Debug.Log("neww manager");
        HideAll();
        //SetCursorForMenu();
        hud.AddToClassList("active");   
    }
    public void startGame()
    {
        gameStarted = true;
        IsPaused = false;
        ShowHUD();
        Time.timeScale = 1f;
        Debug.Log("Game started");
        
    }
    public void TogglePause()
    {
        if (IsPaused)
            Resume();
        else
            ShowPauseMenu();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
  
}

























//using UnityEngine;
//using UnityEngine.UIElements;

//public class UIManager : MonoBehaviour
//{

//    public enum UIState
//    {
//        MainMenu,
//        HUD,
//        Pause,
//        Map,
//        EndGame
//    }


//    private static UIManager _instance;
//    public static UIManager Instance
//    {
//        get
//        {
//            if (_instance == null)
//            {
//                _instance = FindObjectOfType<UIManager>();
//            }
//            return _instance;
//        }
//    }

//    [Header("UI Documents")]
//    public UIDocument mainMenu;
//    public UIDocument hud;
//    public UIDocument pauseMenu;
//    public UIDocument miniGameMenu;
//    public UIDocument endGameMenu;

//    private void Awake()
//    {

//        if (_instance == null)
//        {
//            Debug.Log("nullllll");
//            _instance = this;
//            if (_instance != null) Debug.Log("assignd");
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Debug.Log("not null inst");
//            Destroy(gameObject);
//        }

//        //DontDestroyOnLoad(gameObject); // optional, keeps UIManager alive across scenes


//        // Initial state
//        ShowMainMenu();
//    }

//    // -------------------------
//    // Public API (Used by gameplay)
//    // -------------------------

//    public void ShowMainMenu()
//    {
//        Debug.Log("enterShow menu");
//        DisableAll();
//        if (mainMenu != null) mainMenu.enabled = true;
//    }

//    public void ShowHUD()
//    {
//        Debug.Log("switch to hud");
//        //if (Instance == null || hud == null)
//        //{
//        //    Debug.Log("UIManager or HUD not assigned!");
//        //    return;
//        //}
//        DisableAll();
//        if (hud != null) hud.enabled = true;
//    }

//    public void ShowPauseMenu()
//    {
//        DisableAll();
//        if (pauseMenu != null) pauseMenu.enabled = true;
//    }

//    public void ShowGame()
//    {
//        DisableAll();
//        if (miniGameMenu != null) miniGameMenu.enabled = true;
//    }

//    public void ShowEndGame()
//    {
//        DisableAll();
//        if (endGameMenu != null) endGameMenu.enabled = true;
//    }

//    // -------------------------
//    // Internal helpers
//    // -------------------------

//    private void DisableAll()
//    {
//        /*if (mainMenu != null)*/ mainMenu.enabled = false;
//        /*if (hud != null)*/ hud.enabled = false;
//        /*if (pauseMenu != null)*/ pauseMenu.enabled = false;
//        /*if (miniGameMenu != null)*/ miniGameMenu.enabled = false;
//        /*if (endGameMenu != null)*/ endGameMenu.enabled = false;
//    }
//}
