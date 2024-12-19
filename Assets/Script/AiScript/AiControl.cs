using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour
{
    [Header(""), SerializeField] private Transform player; 
    [Header(""), SerializeField] private LayerMask obstacleLayer;
    [Header(""), SerializeField] private float sightRange = 20f;
    [Header(""), SerializeField] private float shootRange = 15f;
    [Header(""), SerializeField] private float healthFight = 30f;
    private PlayerController playerController;
    [Header(""), SerializeField] private PlayerParameter playerParameter;
    [Header(""), SerializeField] private GunManager gunManager;
    [Header(""), SerializeField] private float viewAngle;
    private Transform coinTarget;
    private NavMeshAgent agent;
    private Vector3 lastKnownPosition;
    private int coin;
    private bool playerInSight = false;
    private bool coinInSight;
    private bool isHide = false;

    public PlayerParameter SendPlayerParameter { get { return playerParameter; } }

    public Transform SendInSightPlayerObject { get { return player; } }

    public bool SendPlayerInSightCheck() { return playerInSight; }

    public float SendViewAngle() { return viewAngle; }


    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //CharacterMoveCheck();
        Debug.Log(coinInSight);
        Debug.Log(playerInSight);
        WalkingSimulator();
        CheckPlayerInSight();
        OnPlayerInSight();
        CharacterLifeCycle();
        CharacterHide();
    }

    //botÇÃà⁄ìÆäƒéãé¿ëïÇ∑ÇÈâ¬î\ê´Ç†ÇËÅBåªç›ÇÕÇ»ÇµÇ≈é¿ëïíÜ
    //private void CharacterMoveCheck()
    //{
    //    Vector3 move = this.transform.position;
    //    Vector3 lastMove = move;

    //    if (lastMove.y)

    //}

    private void OnPlayerInSight()
    {
        if (coinInSight && !playerInSight && coinTarget != null)
        {
            Debug.Log("cointuisewki");
            agent.SetDestination(coinTarget.position);
        }

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

                //à»â∫èeåÇèàóù
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
        else if (lastKnownPosition != Vector3.zero && !coinInSight && lastKnownPosition == this.transform.position)
        {
            agent.SetDestination(lastKnownPosition);
        }
    }

    private void CheckPlayerInSight()
    {
        GameObject thisCoinTarget = null;
        Collider[] targetsViewCoins = Physics.OverlapSphere(transform.position, sightRange);
        foreach (Collider target in targetsViewCoins)
        {
            if (target.CompareTag("Coin"))
            {
                Debug.Log("coin");
                thisCoinTarget = target.gameObject;
                coinTarget = thisCoinTarget.transform;
            }
        }

        GameObject thisTargetPlayerObject = null;
        Collider[] targetsViewPlayer = Physics.OverlapSphere(transform.position, sightRange);
        foreach (Collider target in targetsViewPlayer)
        {
            if (target.CompareTag("Player"))
            {
                Debug.Log("Player");
                thisTargetPlayerObject = target.gameObject;
                coinTarget = thisTargetPlayerObject.transform;
            }
        }
        if (thisTargetPlayerObject != null)
        {
            Vector3 directionToTarget = (thisTargetPlayerObject.transform.position - transform.position).normalized;

            float viewAngleTarget = Vector3.Angle(transform.forward, directionToTarget);
            if (viewAngleTarget < viewAngle / 2)
            {
                player = thisTargetPlayerObject.transform;
            }
        }

        if (player != null)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            var targetPlayer = Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayer);


            if (
                distanceToPlayer <= sightRange
                && !Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayer)
                && player != null
                )
            {
                coinInSight = false;
                playerInSight = true;
            }
            else if (distanceToPlayer > sightRange || Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayer))
            {
                playerInSight = false;
            }
            else
            {
                Debug.Log("aaa");
                agent.enabled = false;
            }
        }

        if (thisCoinTarget != null)
        {
            Debug.Log("Ç±Ç¢ÇÒÇ®Ç¢Ç©ÇØÇÈ");
            coinInSight = true;
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

    //Ç†Ç∆Ç≈Ç¬Ç¬Ç¨ÇÇ‚ÇÈÅBHPí·â∫ÇÃâÒîèàóù
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
            coinTarget = null;
            Destroy(collision.gameObject);
        }
    }
}
