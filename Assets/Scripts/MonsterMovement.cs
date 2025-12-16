using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");    
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.transform.position);
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player caught by monster!");
            // Here you can add logic to handle what happens when the player is caught
            GameController.instance.ShowLosePanel();
        }
        
    }
}
