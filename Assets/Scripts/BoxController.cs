using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class BoxController : MonoBehaviour
{
    public GameObject miniGamePrefab;
    public TMP_Text boxLabel;
    public bool isPlayerWinner = false;
    public bool isGameOpen = false;
    
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
        if(other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && !isPlayerWinner && !isGameOpen)
        {
            // الواجهة بتاعت الميني جيم
            if (UIManager.Instance == null) Debug.Log("null");
            else
            {
                Debug.Log("not null");
                UIManager.Instance.OnEnteringGame();
            }

            // نجيب الجيم ماناجر بتاع كل لعبة من الميني جيمز
            GameObject miniGameInstance = Instantiate(miniGamePrefab);
            if(miniGameInstance.GetComponentInChildren<GameManager>() != null)
            {
                miniGameInstance.GetComponentInChildren<GameManager>().boxController = this;
            }
            else
            {
                miniGameInstance.GetComponentInChildren<AssetsN.GameManager>().boxController = this;
            }

            isGameOpen = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            boxLabel.text = "";
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isGameOpen = false;
        }
    }
    public void WinMiniGame()
    {
        isPlayerWinner = true;
        boxLabel.text = "";
        if (UIManager.Instance == null) Debug.Log("null");
        else
        {
            Debug.Log("not null");
            UIManager.Instance.OnWinningGame();
        }
        GetComponent<Animator>().SetTrigger("Win");
        GameController.instance.CollectMap();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isGameOpen = false;
    }
}
