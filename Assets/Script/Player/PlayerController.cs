using UnityEngine;

/// <summary>
/// プレイヤーに関するクラス
/// </summary>
//TODO:死亡時のカメラの処理をここに書くな。今後設計を考え直せ
//TODO:Animation実装
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerParameter parameter;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float currentSpeed;                        //現在Speed
    [SerializeField] private GunManager gunManager;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera deathCamera;
    private GameManager gameManager;
    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded;
    private bool isClimbing;
    private int coin;
    private float currentHealth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
    }
    private void Start()
    {
        currentSpeed = parameter.PlayerSpeed;
        currentHealth = parameter.PlayerHP;

        ////このコメントは記述者が書いていて分からなくなったので、計算メモとして残してます
        ////参考リンク：https://qiita.com/kaku0710/items/fdf5bab18b65f6f9dcb4
        deathCamera.gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        PlayerSpeedControl();                                   //ダッシュ処理
        PlayerMove();                                           //プレイヤーの移動
        //PlayerClimbing();                                       //壁登り処理（内容未実装）
        PlayerShift();                                          //しゃがむ処理（内容未実装）
        Playerfire();                                           //プレイヤーの銃撃（バグ未解決）
        PlayerReload();                                         //リロード
        PlayerAbility();                                        //アビリティ（内容未実装）
        //マウスScrollで飲料選択。数字でも可
        Debug.Log("接地" + isGrounded);
        Debug.Log("登る" + isClimbing);
    }
    private void PlayerMove()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;                                             //水平面上の前方向のみを制御
        right.y = 0f;                                               //水平面上の右方向のみを制御
        forward.Normalize();
        right.Normalize();

        //forward * moveVertical：カメラ前ベクトルに垂直方向を掛け、前後の移動方向を計算
        //right * moveHorizontal：カメラ右ベクトルに水平方向を掛け、左右の移動方向を計算
        Vector3 desiredMoveDirection = forward * moveVertical + right * moveHorizontal;

        if (isGrounded)
        {
            Vector3 move = desiredMoveDirection * currentSpeed;
            move.y = rb.velocity.y;
            rb.velocity = move;
            if (animator != null) animator.SetFloat("Walk", desiredMoveDirection.magnitude);
        }
    }

    private void PlayerClimbing()
    {
        float CheckOffset = 1.0f;       //壁判定のオフセット
        float upperCheckOffset = 12.5f; //上部壁判定のオフセット
        float wallCheckDistance = 5.0f; //壁判定の距離
        Ray wallCheckRay = new Ray(transform.position + Vector3.up * CheckOffset, transform.forward);
        Ray upperCheckRay = new Ray(transform.position + Vector3.up * upperCheckOffset, transform.up);

        bool isForwardWall = Physics.Raycast(wallCheckRay, wallCheckDistance);
        bool isUpperWall = Physics.Raycast(upperCheckRay, wallCheckDistance);
        Debug.DrawRay(wallCheckRay.origin, wallCheckRay.direction * wallCheckDistance, Color.red);
        Debug.DrawRay(upperCheckRay.origin, upperCheckRay.direction * wallCheckDistance, Color.blue);
        //壁があるかどうかをログに出力
        Debug.Log("Forward Wall: " + isForwardWall);
        Debug.Log("Upper Wall: " + isUpperWall);

        if (isForwardWall && !isUpperWall) isClimbing = true;
        else isClimbing = false;
        if (isClimbing)
        {
            //入力を取得
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            //移動ベクトルを計算
            Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);

            //Rigidbodyのvelocityを設定
            rb.velocity = movement * currentSpeed;
        }
    }
    private void PlayerSpeedControl()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) currentSpeed = parameter.SprintSpeed;
        else currentSpeed = parameter.PlayerSpeed;
    }
    private void PlayerShift()
    {
        //TODO:シフトでしゃがむ
    }
    private void Playerfire()
    {
        if (Input.GetMouseButton(0)) gunManager.Shoot();
        //TODO:右マウスでエイム
    }
    private void PlayerReload()
    {
        if (Input.GetKeyDown(KeyCode.R)) gunManager.StartReload();
    }
    private void PlayerAbility()
    {
    }
    private void Die()
    {
        gameObject.SetActive(false);
        //リスポーンタイマーを開始させる
        gameManager.StartRespawnTimer(gameObject);
        //メインカメラを無効化し、死亡時カメラを有効化
        mainCamera.gameObject.SetActive(false);
        deathCamera.gameObject.SetActive(true);
        deathCamera.transform.position = transform.position + new Vector3(0, 10, 0); //上空に配置
        deathCamera.transform.LookAt(transform.position);
    }
    public void Respawn()
    {
        gameObject.SetActive(true);

        //死亡時カメラを無効化し、メインカメラを有効化
        deathCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
    }
    private void OnCollisionEnter(Collision collision)
    {
        //プレイヤーコイン量追加
        if (collision.gameObject.TryGetComponent<CoinScript>(out var coinManager))
        {
            coin += coinManager.Coin;
            Destroy(collision.gameObject);
        }
        //Debug.Log(coin);
    }
    private void OnCollisionStay(Collision collision)
    {
        //TODO:ゲーム開始時、接地Flagが数秒falseになるバグを解決する
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;

        if (collision.gameObject.CompareTag("DamageArea"))
        {
            currentHealth -= 9999 * Time.deltaTime;
            if (currentHealth <= 0) Die();
        }

        ////給水エリア場で、常に玉がフル装填される
        //if (collision.gameObject.CompareTag("WaterArea")) gunManager.WaterReload();
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = false;
        if (collision.gameObject.CompareTag("Wall")) isClimbing = false;
    }
}