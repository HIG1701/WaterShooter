using UnityEngine;
using UnityEngine.AI;

public class PlayerAIController : MonoBehaviour
{
    [Header(""), SerializeField] private Transform player; 
    [Header(""), SerializeField] private LayerMask obstacleLayer;
    [Header(""), SerializeField] private float sightRange = 20f;
    [Header(""), SerializeField] private float shootRange = 15f;
    [Header(""), SerializeField] private float healthFight = 30f;
    private Animator animator;
    private PlayerController playerController;
    [Header(""), SerializeField] private PlayerParameter playerParameter;
    [Header(""), SerializeField] private GunManager gunManager;
    private NavMeshAgent agent;
    private Vector3 lastKnownPosition;
    private int coin;
    private bool playerInSight = false;
    private bool isHide = false;
    private bool isWalkingSimulator = false;
    private Rigidbody rb;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        WalkingSimulator();
        CheckPlayerInSight();
        OnPlayerInSight();
        CharacterLifeCycle();
        CharacterHide();
    }



    private void OnPlayerInSight()
    {
        if (playerInSight)
        {
            lastKnownPosition = player.position;
            if (Vector3.Distance(transform.position, player.position) <= shootRange)
            {
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
                Debug.Log("Check");
                agent.isStopped = true;
                Vector3 sideStep = transform.right * Mathf.Sin(Time.time * 6f);
                agent.Move(sideStep * Time.deltaTime);

                //ˆÈ‰ºeŒ‚ˆ—
                if (!gunManager.CanShoot())
                {
                    Debug.Log("a");
                    gunManager.StartReload();
                }
                gunManager.Shoot();
            }
            else
            {
                agent.isStopped = false;
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

    private void WalkingSimulator()
    {
        if(!agent.hasPath)
        {
            Vector3 randomDirection = Random.insideUnitSphere * sightRange;
            randomDirection += transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, sightRange, 1))
            {
                agent.SetDestination(hit.position);
            }
        }
    }

    //‚ ‚Æ‚Å‚Â‚Â‚¬‚ð‚â‚éBHP’á‰º‚Ì‰ñ”ðˆ—
    private void CharacterHide()
    {
        if (isHide)
        {

        }
    }

    private void CharacterLifeCycle()
    {
        if (playerParameter.PlayerHP >= 0)
        {
            isHide = true;
        }
        else
        {
            isHide = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<CoinScript>(out var coinManager))
        {
            coin += coinManager.Coin;
            Destroy(collision.gameObject);
        }
    }
}
