using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class AIControl : MonoBehaviour
{
    [Header(""), SerializeField] private Transform player; 
    [Header(""), SerializeField] private LayerMask obstacleLayer;
    [Header(""), SerializeField] private float sightRange = 20f;
    [Header(""), SerializeField] private float shootRange = 15f;
    [Header(""), SerializeField] private float healthFight = 30f;
    [Header(""), SerializeField] private float viewHideObjectRange = 20f;
    private PlayerController playerController;
    [Header(""), SerializeField] private PlayerParameter playerParameter;
    [Header(""), SerializeField] private GunManager gunManager;
    [Header(""), SerializeField] private float viewAngle;
    private Transform coinTarget;
    private NavMeshAgent agent;
    private CharacterMove characterMove;
    private Vector3 lastKnownPosition;
    private int coin;
    private bool playerInSight = false;
    private bool coinInSight;
    private bool isHide = false;

    public PlayerParameter SendPlayerParameter { get { return playerParameter; } }

    public Transform SendInSightPlayerObject { get { return player; } }

    public bool SendPlayerInSightCheck() { return playerInSight; }

    public float SendViewAngle() { return viewAngle; }

    private Vector3 moveDirection;
    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        characterMove = GetComponent<CharacterMove>();
    }

    private void Update()
    {
        //CharacterMoveCheck();
        Debug.Log(coinInSight);
        Debug.Log(playerInSight);
        StartCoroutine(CharacterHide());
        WalkingSimulator();
        CheckPlayerInSight();
        OnPlayerInSight();
        CharacterLifeCycle();
        CharacterHide();
        if (1==1)
        {
            // プレイヤーの正面方向
            Vector3 forward = transform.forward;

            // 正面方向と移動方向の内積を計算
            float dotProduct = Vector3.Dot(forward, moveDirection);

            // 正面に移動しているかどうかを判定
            if (dotProduct > 0.5f) // 0.7は閾値で、1に近いほど完全に正面
            {
                Debug.Log("正面に移動中");
            }
            else
            {
                Debug.Log("正面以外に移動中");
            }
        }
    }

    //botの移動監視実装する可能性あり。現在はなしで実装中
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
            //agent.SetDestination(coinTarget.position);
            characterMove.CalculatePath(coinTarget.position);
        }

        if (playerInSight && isHide)
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

                //以下銃撃処理
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
                //agent.SetDestination(player.position);
                characterMove.CalculatePath(player.position);
            }
        }
        else if (lastKnownPosition != Vector3.zero && !coinInSight && lastKnownPosition == this.transform.position)
        {
            //agent.SetDestination(lastKnownPosition);
            characterMove.CalculatePath(lastKnownPosition);
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
            Debug.Log("こいんおいかける");
            coinInSight = true;
        }

    }

    private void WalkingSimulator()
    {
        if(!agent.hasPath)
        {
            //Vector3 randomDirection = Random.insideUnitSphere * sightRange;
            //randomDirection += transform.position;
            //NavMeshHit hit;
            //if (NavMesh.SamplePosition(randomDirection, out hit, sightRange, 1))
            //{
            ////    agent.SetDestination(hit.position);
            //    characterMove.CalculatePath(hit.position);
            //}
        }
    }


    public LayerMask coverLayer; // 遮蔽物のレイヤー
    public float checkRadius = 30f; // 遮蔽物を探す範囲
    Transform bestCover = null;
    //あとでつつぎをやる。HP低下の回避処理
    private IEnumerator CharacterHide()
    {
        if (!isHide && bestCover == null)
        {
            Collider[] coverPoints = Physics.OverlapSphere(transform.position, checkRadius, coverLayer);
            bestCover = null;
            float bestScore = float.MaxValue;
            int bestCoverPointIndex = 0;
            foreach (Collider cover in coverPoints)
            {
                Vector3 coverPosition = cover.transform.position;
                Debug.Log("ss");
                // プレイヤーから遮蔽物への射線チェック
                if (Physics.Linecast(player.position, coverPosition, out RaycastHit hit))
                {
                    if (hit.collider == cover)
                    {
                        // 距離をスコアとして採用
                        float score = Vector3.Distance(transform.position, coverPosition);
                        Debug.Log(coverPoints.Length);
                        // 最良の遮蔽物を更新
                        if (score < bestScore)
                        {
                            bestScore = score;
                            bestCover = cover.transform;
                            Debug.Log(bestCover.gameObject.transform.localScale);
                            //agent.SetDestination(bestCover.position); // 遮蔽物に移動
                            characterMove.CalculatePath(bestCover.position);
                            break;
                        }
                    }
                }
                Debug.Log(bestCoverPointIndex);
                bestCoverPointIndex++;
            }
        }

        while (true) //ある程度の誤差を許容するためのループ
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 5f, coverLayer)) //ここの5fを変数にあとでかえる
            {
                Debug.Log("誤差調整");
                //agent.SetDestination(transform.position);
                characterMove.CalculatePath(transform.position);
                bestCover = null;
                break;
            }
            Debug.DrawRay(transform.position, transform.forward * 5f, Color.red);//あとでけす
            yield return null;
        }



        yield break;
    }

    

    private void CharacterLifeCycle()
    {
        if (playerParameter.PlayerHP >= 30)
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
