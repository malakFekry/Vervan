using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject losePanel;
    public GameObject winPanel;
    public int mapsCollected = 0;
    public GameObject guides;
    public PlayableDirector cutsceneDirector;
    public GameObject guideText;
    // This class can be expanded to manage overall game state, levels, etc.
    public static GameController instance;
    void Awake()
    {
        instance = this;
        guides.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void ShowLosePanel()
    {
        Time.timeScale = 0f;
        losePanel.SetActive(true);
        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;

        StartCoroutine(DelayedCity());
    }
    public void ShowWinPanel()
    {
        Time.timeScale = 0f;
        winPanel.SetActive(true);
        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;

        StartCoroutine(DelayedCity());
    }

    private IEnumerator DelayedCity()
    {
        yield return new WaitForSecondsRealtime(5f);
        SceneManager.LoadScene(0);
    }

    public void CollectMap()
    {
        mapsCollected++;
        if (mapsCollected >= 2) // Assuming 2 maps are needed to win
        {
            guides.SetActive(true);
            guideText.SetActive(true);
            if(cutsceneDirector != null)
            {
                cutsceneDirector.Play();
            }
        }
    }
}