using UnityEngine;
using UnityEngine.SceneManagement;

public class SignalScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Signal()
    {
              
        SceneManager.LoadScene(1);
        if (UIManager.Instance == null) Debug.Log("null zeft");
        else
        {
            Debug.Log("not null zeft");
            UIManager.Instance.OnTimelineSignal();
        }
    }
}
