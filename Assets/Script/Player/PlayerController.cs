using UnityEngine;

/// <summary>
/// プレイヤーに関するクラス
/// </summary>

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerParameter parameter;
    [SerializeField] public int CharacterIndex = 0;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float currentSpeed;                        //現在Speed
    [SerializeField] private GunManager gunManager;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera deathCamera;
    private GameManager gameManager;
    private Rigidbody rb;
    private bool isGrounded;
    private int coin;
    private float currentHealth;
    private Animator animator;
    private bool isSyagamu;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentSpeed = parameter.PlayerSpeed;
        currentHealth = parameter.PlayerHP;
        isSyagamu = false;
        ////このコメントは記述者が書いていて分からなくなったので、計算メモとして残してます
        ////参考リンク：https://qiita.com/kaku0710/items/fdf5bab18b65f6f9dcb4
        deathCamera.gameObject.SetActive(false);
    }

    private void Update()
    {
        PlayerReload();
    }

    private void FixedUpdate()
    {
        PlayerSpeedControl();                                   //ダッシュ処理
        PlayerMove();                                           //プレイヤーの移動
        PlayerShift();                                          //しゃがむ処理（内容未実装）
        Playerfire();                                           //プレイヤーの銃撃（バグ未解決）
        if (!isSyagamu)
        {
            animator.SetTrigger("Tatu");
            animator.ResetTrigger("Syagamu");
        }
        else
        {
            animator.SetTrigger("Syagamu");
            animator.ResetTrigger("Tatu");
        }
    }

    private void OnEnable()
    {
        transform.localPosition = Vector3.zero;
        rb.velocity = Vector3.zero;
    }

    public PlayerParameter SendPlayerParameter { get { return parameter; } }

    public void DrinkInInventory(GameObject drinkDate)
    {
        Debug.Log(drinkDate);
        //parameter.playerInventory.drinkType.Add(drinkDate);
    }

    public virtual void PlayerMove()
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

        //Rayを使用し、壁抜け対策する
        //参考リンク：https://note.com/ryuryu_game/n/ncf259eb5f044
        //Rayの開始位置とRayの方向を設定
        Vector3 rayStart = transform.position;
        Vector3 rayDirection = desiredMoveDirection;

        //TODO:壁の設計を考える
        ////移動方向にRayを放射
        //RaycastHit hit;
        //if (!Physics.Raycast(rayStart, rayDirection, out hit, 0.5f))
        //{
        //    if (desiredMoveDirection != Vector3.zero)
        //    {
        //        if (moveVertical != 0) rb.MovePosition(transform.position + desiredMoveDirection * currentSpeed * Time.fixedDeltaTime);
        //    }
        //}
        //else
        //{
        //    //Rayが壁にヒットしていれば、壁を上る
        //    if (hit.collider.CompareTag("Wall"))
        //    {
        //        float climbSpeed = 5f;
        //        if (moveVertical > 0) desiredMoveDirection = Vector3.up * climbSpeed;
        //    }
        //}
        Vector3 move = desiredMoveDirection * currentSpeed;
        move.y = rb.velocity.y;
        rb.velocity = move;
        if (!isSyagamu && !Input.GetKey(KeyCode.Space))
        {
            animator.SetFloat("Walk", move.magnitude);
        }
        else if (isSyagamu && !Input.GetKey(KeyCode.Space))
        {
            animator.SetFloat("SyagamiWalk", move.magnitude);
        }
    }

    private void PlayerSpeedControl()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) currentSpeed = parameter.SprintSpeed;
        else currentSpeed = parameter.PlayerSpeed;
    }

    private void PlayerShift()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!isSyagamu)
            {
                isSyagamu = true;
                Debug.Log(isSyagamu);
            }
            else
            {
                isSyagamu = false;
                Debug.Log(isSyagamu);
            }
        }
    }

    private void Playerfire()
    {
        if (Input.GetMouseButton(0)) gunManager.Shoot();
    }

    private void PlayerReload()
    {
        if (Input.GetKeyDown(KeyCode.R)) gunManager.StartReload();
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
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

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
    }
}