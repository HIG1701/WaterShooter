using UnityEngine;
using UnityEngine.AI;

public class PlayerAIController : PlayerController
{
    [SerializeField] private Transform player; 
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float sightRange = 20f;
    [SerializeField] private float shootRange = 15f;
    [SerializeField] private float healthFight = 30f;
    private PlayerController playerController;
    private NavMeshAgent agent;
    private Vector3 lastKnownPosition;
    private bool playerInSight = false;


    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        CheckPlayerInSight();

        if (playerInSight)
        {
            lastKnownPosition = player.position;
            if (Vector3.Distance(transform.position, player.position) <= shootRange)
            {
                
            }
            else
            {
                agent.SetDestination(player.position);
            }
        }
        else if (lastKnownPosition != Vector3.zero)
        {
            agent.SetDestination(lastKnownPosition);
        }
    }

    private void CheckPlayerInSight()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= sightRange && !Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayer))
        {
            playerInSight = true;
        }
        else
        {
            playerInSight = false;
        }
    }

    public override void PlayerMove()
    {

    }
}
