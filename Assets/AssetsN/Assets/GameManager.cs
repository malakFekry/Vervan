using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace AssetsN{
public class GameManager : MonoBehaviour
{
    [Header("Elements")]
    public GameObject tilePrefab;
    public Transform gridContainer;
    public CanvasGroup gridCanvasGroup; 
    
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI trialText;
    public TextMeshProUGUI mistakesText;
    public TextMeshProUGUI correctTilesText;
    //public TextMeshProUGUI finalMessageText; 
    public GameObject WinPanel;
    public GameObject LosePanel;
    [Header("Game Settings")]
    public int totalTiles = 36;       
    public int totalTrials = 6;       
    public float showDuration = 3.0f; // وقت عرض الباترن للحفظ
    public float playDuration = 60.0f; // وقت اللعب (دقيقتين) <-- الجديد هنا
    public int maxMistakes = 4;       
    public int scoreToWin = 3000;

    private List<TileController> allTiles = new List<TileController>();
    private List<TileController> currentPattern = new List<TileController>();
    
    private int currentTrial = 0;
    private int currentScore = 0;
    private int currentMistakes = 0;
    private int patternSize = 5; 
    private int correctTilesClicked = 0;
    private Coroutine turnTimer; // متغير عشان نتحكم في العداد
    public BoxController boxController;

    void Start()
    {
        if (gridCanvasGroup == null && gridContainer != null)
             gridCanvasGroup = gridContainer.GetComponent<CanvasGroup>();

        //if(finalMessageText != null) finalMessageText.gameObject.SetActive(false);
        WinPanel.SetActive(false);
        LosePanel.SetActive(false);
        GenerateMatrix();
        StartCoroutine(StartNewTrial());
    }
    public void QuitGame()
    {   Time.timeScale = 1; // IMPORTANT: Unfreeze time before quitting!
        Debug.Log("Quit Game Requested"); // Shows in Editor Console
        Destroy(transform.parent.gameObject); // Clean up GameManager
    }
    public void WinGame()
    {
        //boxController.isPlayerWinner = true;
        boxController.WinMiniGame();
        Destroy(transform.parent.gameObject, 0.1f); // Clean up GameManager
    }
    void GenerateMatrix()
    {
        foreach (Transform child in gridContainer) Destroy(child.gameObject);
        allTiles.Clear();

        for (int i = 0; i < totalTiles; i++)
        {
            GameObject newTileObj = Instantiate(tilePrefab, gridContainer);
            TileController tile = newTileObj.GetComponent<TileController>();
            tile.gameManager = this;
            allTiles.Add(tile);
        }
    }

    IEnumerator StartNewTrial()
    {
        yield return null;
        if (gridCanvasGroup != null) gridCanvasGroup.alpha = 1f;

        currentTrial++;
        UpdateUI();

        if (currentTrial > totalTrials)
        {
            ShowFinalResult();
            yield break;
        }

        currentMistakes = 0;
        correctTilesClicked = 0;
        foreach (var tile in allTiles) tile.ResetTile();
        UpdateUI(); 

        yield return new WaitForSeconds(1.0f); 

        GenerateRandomPattern();

        // مرحلة الحفظ
        foreach (var tile in currentPattern) tile.HighlightTile();
        yield return new WaitForSeconds(showDuration);
        foreach (var tile in currentPattern) tile.HideTile();

        // مرحلة اللعب
        foreach (var tile in allTiles) tile.EnableInteraction(true);
        
        // تشغيل العداد (دقيقتين) أول ما يبدأ اللعب
        if (turnTimer != null) StopCoroutine(turnTimer);
        turnTimer = StartCoroutine(PlayTimer());
    }

    // العداد الجديد
    IEnumerator PlayTimer()
    {
        yield return new WaitForSeconds(playDuration);
        // لو الوقت خلص واللاعب لسه مخلصش، اعتبرها خسارة وانقل للي بعده
        EndTrial(false);
    }

    void GenerateRandomPattern()
    {
        currentPattern.Clear();
        List<TileController> shuffledTiles = new List<TileController>(allTiles);
        for (int i = 0; i < shuffledTiles.Count; i++) {
            TileController temp = shuffledTiles[i];
            int randomIndex = Random.Range(i, shuffledTiles.Count);
            shuffledTiles[i] = shuffledTiles[randomIndex];
            shuffledTiles[randomIndex] = temp;
        }

        for (int i = 0; i < patternSize; i++)
        {
            shuffledTiles[i].isTarget = true;
            currentPattern.Add(shuffledTiles[i]);
        }
    }
    public IEnumerator WaitForEnd(bool success)
    {
        foreach (var tile in allTiles) tile.EnableInteraction(false);
        yield return new WaitForSeconds(1.0f);
        EndTrial(success);
    }
    public void CheckTile(TileController clickedTile)
    {
        // if (clickedTile.wasClicked && clickedTile.isTarget)
        // {
        //     EndTrial(false); 
        //     return;
        // }
        if(clickedTile.wasClicked) return;
        clickedTile.wasClicked = true;

        if (clickedTile.isTarget)
        {
            currentScore += 100;
            clickedTile.SetColor(true); 

            if (CheckIfPatternComplete())
            {   
                
                StartCoroutine(WaitForEnd(true));
                
            }
        }
        else
        {
            clickedTile.SetColor(false); 
            currentMistakes++;
            
            if (currentMistakes >= maxMistakes)
            {
                StartCoroutine(WaitForEnd(false));              
            }
        }
        UpdateUI();
    }

        void LateUpdate()
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        bool CheckIfPatternComplete()
    {
        correctTilesClicked = 0;
        foreach (var tile in currentPattern)
        {
            if (tile.wasClicked) correctTilesClicked++;
        }
        return correctTilesClicked == currentPattern.Count;
    }

    void EndTrial(bool success)
    {
        // نوقف العداد عشان مينقلش مرتين بالغلط
        if (turnTimer != null) StopCoroutine(turnTimer);

        foreach (var tile in allTiles) tile.EnableInteraction(false);
        if (success) patternSize++; 
        StartCoroutine(StartNewTrial());
    }

    void UpdateUI()
    {
        if (scoreText) scoreText.text = "SCORE: " + currentScore;
        if (trialText && currentTrial <= totalTrials) trialText.text = "TRIAL: " + currentTrial + " / " + totalTrials;
        if (mistakesText) mistakesText.text = "MISTAKES: " + currentMistakes + " / " + maxMistakes;
        if (correctTilesText) correctTilesText.text = "CORRECT TILES: " + correctTilesClicked + " / " + patternSize;
    }

    void ShowFinalResult()
    {
        if (gridCanvasGroup != null) 
            gridCanvasGroup.alpha = 0.3f; 

        
            
            if (currentScore >= scoreToWin)
            {
                WinPanel.SetActive(true);
            }
            else
            {
                LosePanel.SetActive(true);
            }
        
    }
}
}