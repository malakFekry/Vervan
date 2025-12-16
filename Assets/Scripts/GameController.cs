using UnityEngine;
using UnityEngine.AI;

public class GameController : MonoBehaviour
{
    public GameObject losePanel;
    public GameObject winPanel;
    public int mapsCollected = 0;
    public GameObject guides;
    // This class can be expanded to manage overall game state, levels, etc.
    public static GameController instance;
    void Awake()
    {
        instance = this;
        guides.SetActive(false);
    }
    public void ShowLosePanel()
    {
        Time.timeScale = 0f;
        losePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ShowWinPanel()
    {
        Time.timeScale = 0f;
        winPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void CollectMap()
    {
        mapsCollected++;
        if (mapsCollected >= 2) // Assuming 2 maps are needed to win
        {
            guides.SetActive(true);
        }
    }
}