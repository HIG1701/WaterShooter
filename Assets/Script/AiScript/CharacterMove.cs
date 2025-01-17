using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] private PlayerParameter parameter;
    [SerializeField] private float currentSpeed;
    private Rigidbody rb;


    public float moveSpeed = 3f; // 移動速度
    public float stoppingDistance = 0.5f; // 停止距離

    private NavMeshPath path; // 経路
    private int currentPathIndex; // 現在の経路インデックス
    private bool pathCalculated; // 経路計算済みフラグ


    void Start()
    {
        path = new NavMeshPath();
    }

    public void CalculatePath(Vector3 target)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(target, out hit, 10.0f, NavMesh.AllAreas))
        {
            target = hit.position;
        }
        else
        {
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

    private void IsMove()
    {
        if (currentPathIndex < path.corners.Length)
        {
            Debug.Log("移動中");
            // 現在の経路ポイントを取得
            Vector3 targetPosition = path.corners[currentPathIndex];

            // プレイヤーをターゲット位置に向ける
            Vector3 direction = (targetPosition - transform.position).normalized;


            Quaternion currentRotation = transform.rotation;
            //Quaternion targetRotation = Quaternion.LookRotation(targetPosition);
            //transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime *  * 1);

            var aim = targetPosition - this.transform.position;
            var look = Quaternion.LookRotation(aim);
            transform.rotation = Quaternion.Slerp(currentRotation, look, Time.deltaTime);

            transform.position += direction * moveSpeed * Time.deltaTime;

            // ターゲット位置に到達したら次のポイントに移動
            if (Vector3.Distance(transform.position, targetPosition) < stoppingDistance)
            {
                currentPathIndex++;
            }

            // 最後のポイントまで到達したら計算をリセット
            if (currentPathIndex >= path.corners.Length)
            {
                pathCalculated = false;
            }
            Debug.Log(targetPosition);
        }
    }

    private void FixedUpdate()
    {
        IsMove();
    }

    //private void Awake()
    //{
    //    rb = GetComponent<Rigidbody>();
    //}

    //private void FixedUpdate()
    //{
    //    PlayerSpeedControl();                                   //ダッシュ処理
    //    PlayerMove();                                           //プレイヤーの移動
    //    PlayerShift();                                          //しゃがむ処理（内容未実装）
    //}

    //public virtual void PlayerMove()
    //{
    //    float moveHorizontal = Input.GetAxis("Horizontal");
    //    float moveVertical = Input.GetAxis("Vertical");
    //    Vector3 forward = cameraTransform.forward;
    //    Vector3 right = cameraTransform.right;
    //    forward.y = 0f;                                             //水平面上の前方向のみを制御
    //    right.y = 0f;                                               //水平面上の右方向のみを制御
    //    forward.Normalize();
    //    right.Normalize();

    //    //forward * moveVertical：カメラ前ベクトルに垂直方向を掛け、前後の移動方向を計算
    //    //right * moveHorizontal：カメラ右ベクトルに水平方向を掛け、左右の移動方向を計算
    //    Vector3 desiredMoveDirection = forward * moveVertical + right * moveHorizontal;

    //    Vector3 move = desiredMoveDirection * currentSpeed;
    //    move.y = rb.velocity.y;
    //    rb.velocity = move;
    //}

    //private void PlayerSpeedControl()
    //{
    //    if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) currentSpeed = parameter.SprintSpeed;
    //    else currentSpeed = parameter.PlayerSpeed;
    //}

    //private void PlayerShift()
    //{
    //    //TODO:しゃがむ
    //    //シフトでしゃがむ
    //}
}
