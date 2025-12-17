using UnityEngine;

public class MagicBall : MonoBehaviour
{
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
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player hit by magic ball!");
            // Here you can add logic to handle what happens when the player is hit by the magic ball
            GameController.instance.ShowWinPanel();
        }
        
    }
}
