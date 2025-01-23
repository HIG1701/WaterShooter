using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

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
    private BotAnimation botAnimation;
    private BotAim botAim;
    private Transform coinTarget;
    private NavMeshAgent agent;
    private CharacterMove characterMove;
    private Vector3 lastKnownPosition;
    private int coin;
    private bool playerInSight = false;
    private bool coinInSight;
    private bool isHide = false;

    public float stoppingDistance = 0.5f; // 停止距離

    private NavMeshPath path; // 経路
    private int currentPathIndex; // 現在の経路インデックス
    private bool pathCalculated; // 経路計算済みフラグ
    private Vector3 targetPosition;

    public PlayerParameter SendPlayerParameter { get { return playerParameter; } }

    public Transform SendInSightPlayerObject { get { return player; } }

    public bool SendPlayerInSightCheck() { return playerInSight; }

    public float SendViewAngle() { return viewAngle; }

    private Vector3 moveDirection;
    private void Start()
    {
        botAnimation = GetComponent<BotAnimation>();
        botAim = GetComponent<BotAim>();
        playerController = GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        characterMove = GetComponent<CharacterMove>();
        path = new NavMeshPath();
    }

    private void Update()
    {
        //CharacterMoveCheck();
        Debug.Log(coinInSight);
        Debug.Log(playerInSight);
        //artCoroutine(CharacterHide());
        WalkingSimulator();
        CheckPlayerInSight();
        OnPlayerInSight();
        CharacterLifeCycle();
        //aracterHide();
        ForwardMove();
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
            CalculatePath(coinTarget.position);
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
                pathCalculated = false; //BOTの移動停止.
                Vector3 sideStep = transform.right * Mathf.Sin(Time.time * 6f);
                agent.Move(sideStep * Time.deltaTime);

                //以下銃撃処理
                if (!gunManager.CanShoot())
                {
                    Debug.Log("a");
                    gunManager.StartReload();
                }
                botAnimation.aimTarget = player.transform;

                gunManager.Shoot();
            }
            else
            {
                pathCalculated = false; //BOTの移動停止.
                //agent.SetDestination(player.position);
                CalculatePath(player.position);
            }
        }
        else if (lastKnownPosition != Vector3.zero && !coinInSight && lastKnownPosition == this.transform.position)
        {
            //agent.SetDestination(lastKnownPosition);
            CalculatePath(lastKnownPosition);
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
        if(!pathCalculated)
        {
            Vector3 randomDirection = Random.insideUnitSphere * sightRange;
            randomDirection += transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, sightRange, 1))
            {
                //    agent.SetDestination(hit.position);
                CalculatePath(hit.position);
            }
        }
    }


    public LayerMask coverLayer; // 遮蔽物のレイヤー
    public float checkRadius = 30f; // 遮蔽物を探す範囲
    Transform bestCover = null;
    //あとでつつぎをやる。HP低下の回避処理
    //private IEnumerator CharacterHide()
    //{
    //    if (!isHide && bestCover == null)
    //    {
    //        Collider[] coverPoints = Physics.OverlapSphere(transform.position, checkRadius, coverLayer);
    //        bestCover = null;
    //        float bestScore = float.MaxValue;
    //        int bestCoverPointIndex = 0;
    //        foreach (Collider cover in coverPoints)
    //        {
    //            Vector3 coverPosition = cover.transform.position;
    //            Debug.Log("ss");
    //            // プレイヤーから遮蔽物への射線チェック
    //            if (Physics.Linecast(player.position, coverPosition, out RaycastHit hit))
    //            {
    //                if (hit.collider == cover)
    //                {
    //                    // 距離をスコアとして採用
    //                    float score = Vector3.Distance(transform.position, coverPosition);
    //                    Debug.Log(coverPoints.Length);
    //                    // 最良の遮蔽物を更新
    //                    if (score < bestScore)
    //                    {
    //                        bestScore = score;
    //                        bestCover = cover.transform;
    //                        Debug.Log(bestCover.gameObject.transform.localScale);
    //                        //agent.SetDestination(bestCover.position); // 遮蔽物に移動
    //                        CalculatePath(bestCover.position);
    //                        break;
    //                    }
    //                }
    //            }
    //            Debug.Log(bestCoverPointIndex);
    //            bestCoverPointIndex++;
    //        }
    //    }

    //    while (true) //ある程度の誤差を許容するためのループ
    //    {
    //        Ray ray = new Ray(transform.position, transform.forward);
    //        RaycastHit hit;
    //        Debug.Log("誤差check");
    //        if (Physics.Raycast(ray, out hit, 5f, coverLayer)) //ここの5fを変数にあとでかえる
    //        {
    //            Debug.Log("誤差調整");
    //            //agent.SetDestination(transform.position);
    //            CalculatePath(transform.position);
    //            bestCover = null;
    //            break;
    //        }
    //        Debug.DrawRay(transform.position, transform.forward * 5f, Color.red);//あとでけす
    //        yield return null;
    //    }



    //    yield break;
    //}

    

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

    private void CalculatePath(Vector3 target)
    {
        Debug.Log("StartPathCheck");
        NavMeshHit hit;
        if (NavMesh.SamplePosition(target, out hit, 5.0f, NavMesh.AllAreas))
        {
            target = hit.position;
        }
        else
        {
            Debug.Log("CheckReturn");
            return;
        }

        // NavMeshAgentを使って経路を計算
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.CalculatePath(target, path);
        Debug.Log(path.status);
        Debug.Log(NavMeshPathStatus.PathComplete);



        // 経路が成功した場合のみフラグを設定
        if (path.status == NavMeshPathStatus.PathComplete || path.status == NavMeshPathStatus.PathPartial)
        {
            Debug.Log("経路発見");
            pathCalculated = true;
            currentPathIndex = 0;
        }
    }

    private void ForwardMove()
    {
        if (currentPathIndex < path.corners.Length && pathCalculated)
        {
            Debug.Log("移動中");
            targetPosition = path.corners[currentPathIndex];

            Quaternion currentRotation = transform.rotation;

            // ターゲットの方向を向くための回転
            Vector3 aim = targetPosition - transform.position;
            if (aim != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(aim);

                transform.rotation = Quaternion.Slerp(currentRotation, lookRotation, 4f * Time.deltaTime);
            }

            float angle = Vector3.Angle(transform.forward, aim.normalized);



            if (Vector3.Distance(transform.position, targetPosition) < stoppingDistance)
            {
                currentPathIndex++;
            }

            // 最後のポイントまで到達したら計算をリセット
            if (currentPathIndex >= path.corners.Length)
            {
                Debug.Log("Leave");
                pathCalculated = false;
                characterMove.ISMove(0f, 0f);
            }
            if (angle < 10f && pathCalculated) //あとでフィールドにつくる.
            {
                characterMove.ISMove(0f, 1f);
            }
        }
        else if (!pathCalculated)
        {
            characterMove.ISMove(0f, 0f);
        }
    }
}
