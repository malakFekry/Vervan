using TMPro;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public GameObject miniGamePrefab;
    public TMP_Text boxLabel;
    public bool isPlayerWinner = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isPlayerWinner)
        {
            boxLabel.text = "Press E to Open Mini-Game";
        }
    }
    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && !isPlayerWinner)
        {
            GameObject miniGameInstance = Instantiate(miniGamePrefab);
            miniGameInstance.GetComponentInChildren<GameManager>().boxController = this;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //boxLabel.text = "";
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && !isPlayerWinner)
        {
            boxLabel.text = "";
        }
    }
    public void WinMiniGame()
    {
        isPlayerWinner = true;
        boxLabel.text = "";
        GetComponent<Animator>().SetTrigger("Win");
    }
}
