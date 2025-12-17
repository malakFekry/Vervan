using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public BoxController boxController;
    [Header("Prefabs & Parents")]
    public FloatingBubble bubblePrefab;
    public Transform bubbleContainer;

    [Header("UI References")]
    public TextMeshProUGUI targetLetterText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public Button pauseButton; // Top corner pause button

    [Header("Timer")]
    public Image timerSpinner;
    public float timeLimit = 10f;
    private float currentTimer;

    [Header("Panels")]
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public GameObject pausePanel;

    [Header("Game Settings")]
    public int maxBubblesOnScreen = 8;
    public int totalBubblesToWin = 20;

    private int score = 0;
    private int lives = 3;
    private string currentTargetLetter;
    private bool isGameOver = false;
    private bool isPaused = false;

    private float movementInterval = 1.5f;
    private float moveTimer = 0f;
    private int bubblesToMoveAtOnce = 1;

    private List<FloatingBubble> activeBubbles = new List<FloatingBubble>();
    private string availableChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    void Start()
    {
        
        InitializeGame();
        if (pauseButton) pauseButton.onClick.AddListener(TogglePause);
    }

    void Update()
    {
        if (isGameOver || isPaused) return;

        // Timer Logic
        currentTimer -= Time.deltaTime;
        if (timerSpinner != null) timerSpinner.fillAmount = currentTimer / timeLimit;
        if (currentTimer <= 0) HandleTimeUp();

        // Movement Logic
        moveTimer += Time.deltaTime;
        if (moveTimer >= movementInterval)
        {
            MoveRandomBubbles();
            moveTimer = 0f;
        }
    }

    void LateUpdate()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // --- BUTTON FUNCTIONS (Connect these in Inspector) ---

    // 1. For the "Resume" Button AND the Top Corner Pause Button
    public void TogglePause()
    {
        if (isGameOver) return; // Can't pause if game is over

        isPaused = !isPaused; // Flip the state (True -> False)

        if (isPaused)
        {
            // Time.timeScale = 0; // Freeze
            if (pausePanel) pausePanel.SetActive(true);
        }
        else
        {
            // Time.timeScale = 1; // Unfreeze
            if (pausePanel) pausePanel.SetActive(false);
        }
    }

    // 2. For the "Play Again" Button
    public void RestartGame()
    {
        Time.timeScale = 1; // IMPORTANT: Unfreeze time before reloading!
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 3. For the "Quit" Button (Add this to all panels)
    public void QuitGame()
    {   Time.timeScale = 1; // IMPORTANT: Unfreeze time before quitting!
        Debug.Log("Quit Game Requested"); // Shows in Editor Console
        Destroy(transform.parent.gameObject); // Clean up GameManager
        boxController.isGameOpen = false;
    }
    public void WinGame()
    {
        //boxController.isPlayerWinner = true;
        boxController.WinMiniGame();
        Destroy(transform.parent.gameObject, 0.1f); // Clean up GameManager
    }

    // --- INTERNAL GAME LOGIC ---

    void HandleTimeUp()
    {
        lives--;
        UpdateUI();
        if (lives <= 0) GameOver();
        else SetNewTarget();
    }

    void InitializeGame()
    {
        score = 0;
        lives = 3;
        isGameOver = false;
        isPaused = false;
        bubblesToMoveAtOnce = 1;
        moveTimer = 0f;
        Time.timeScale = 1; // Reset time

        UpdateUI();

        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (winPanel) winPanel.SetActive(false);
        if (pausePanel) pausePanel.SetActive(false);

        foreach (Transform child in bubbleContainer) Destroy(child.gameObject);
        activeBubbles.Clear();

        for (int i = 0; i < maxBubblesOnScreen; i++) SpawnBubble();
        SetNewTarget();
    }

    void SpawnBubble()
    {
        if (isGameOver) return;
        string letter = GenerateUniquePattern();
        if (string.IsNullOrEmpty(letter)) return;

        FloatingBubble newBubble = Instantiate(bubblePrefab, bubbleContainer);
        newBubble.transform.localScale = Vector3.one;
        newBubble.transform.localPosition = Vector3.zero;
        newBubble.GetComponent<RectTransform>().anchoredPosition = GetNonOverlappingPosition(null);
        newBubble.Setup(this, letter);
        activeBubbles.Add(newBubble);
    }

    string GenerateUniquePattern()
    {
        for (int i = 0; i < 100; i++)
        {
            string candidate = "" + availableChars[Random.Range(0, availableChars.Length)] + availableChars[Random.Range(0, availableChars.Length)];
            bool isDuplicate = false;
            foreach (FloatingBubble bubble in activeBubbles) { if (bubble.MyLetter == candidate) { isDuplicate = true; break; } }
            if (!isDuplicate) return candidate;
        }
        return null;
    }

    Vector2 GetNonOverlappingPosition(FloatingBubble ignore)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector2 pos = new Vector2(Random.Range(-900f, 900f), Random.Range(-200f, 200f));
            bool overlap = false;
            foreach (FloatingBubble b in activeBubbles)
            {
                if (b == ignore) continue;
                if (Vector2.Distance(pos, b.GetComponent<RectTransform>().anchoredPosition) < 180f) { overlap = true; break; }
            }
            if (!overlap) return pos;
        }
        return Vector2.zero;
    }

    void MoveRandomBubbles()
    {
        if (activeBubbles.Count == 0) return;
        int count = Mathf.Min(bubblesToMoveAtOnce, activeBubbles.Count);
        List<FloatingBubble> shuffled = new List<FloatingBubble>(activeBubbles);
        for (int i = 0; i < count; i++)
        {
            int rnd = Random.Range(0, shuffled.Count);
            FloatingBubble b = shuffled[rnd]; shuffled.RemoveAt(rnd);
            b.MoveTo(GetNonOverlappingPosition(b), 1.0f);
        }
    }

    void SetNewTarget()
    {
        if (activeBubbles.Count > 0)
        {
            currentTargetLetter = activeBubbles[Random.Range(0, activeBubbles.Count)].MyLetter;
            targetLetterText.text = currentTargetLetter;
            currentTimer = timeLimit;
        }
    }

    public void CheckInput(string clickedLetter, FloatingBubble clickedBubble)
    {
        if (isGameOver || isPaused) return;
        if (clickedLetter == currentTargetLetter)
        {
            score++;
            if (score % 5 == 0) bubblesToMoveAtOnce++;
            activeBubbles.Remove(clickedBubble);
            StartCoroutine(PopBubbleAnimation(clickedBubble));
        }
        else
        {
            lives--; UpdateUI();
            if (lives <= 0) GameOver(); else SetNewTarget();
        }
    }

    IEnumerator PopBubbleAnimation(FloatingBubble bubble)
    {
        float t = 0; Vector3 start = bubble.transform.localScale;
        while (t < 0.2f)
        {
            if (bubble != null) bubble.transform.localScale = Vector3.Lerp(start, Vector3.zero, t / 0.2f);
            t += Time.deltaTime; yield return null;
        }
        if (bubble != null) Destroy(bubble.gameObject);
        SpawnBubble();
        if (score >= totalBubblesToWin) WinGame2(); else { SetNewTarget(); UpdateUI(); }
    }

    void UpdateUI() { scoreText.text = "Score: " + score; if (livesText) livesText.text = "Lives: " + lives; }
    void WinGame2() { isGameOver = true; StartCoroutine(ShowPanelWithZoom(winPanel)); }
    void GameOver() { isGameOver = true; StartCoroutine(ShowPanelWithZoom(gameOverPanel)); }

    IEnumerator ShowPanelWithZoom(GameObject panel)
    {
        panel.SetActive(true); panel.transform.localScale = Vector3.zero;
        float t = 0;
        while (t < 0.5f) { panel.transform.localScale = Vector3.one * Mathf.SmoothStep(0, 1, t / 0.5f); t += Time.unscaledDeltaTime; yield return null; }
        panel.transform.localScale = Vector3.one; //Time.timeScale = 0;
    }
}