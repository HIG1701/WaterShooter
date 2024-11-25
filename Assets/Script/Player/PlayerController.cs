using UnityEngine;

/// <summary>
/// プレイヤーに関するクラス
/// </summary>

//TODO:死亡時のカメラの処理をここに書くな。今後設計を考え直せ

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
    private bool isGrounded;
    private int coin;
    private float currentHealth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();
    }


    private void Start()
    {
        currentSpeed = parameter.PlayerSpeed;
        currentHealth = parameter.PlayerHP;


        //このコメントは記述者が書いていて分からなくなったので、計算メモとして残してます
        //参考リンク：https://qiita.com/kaku0710/items/fdf5bab18b65f6f9dcb4
        //Physics.gravity：デフォルト：(0, -9.81, 0)
        //GravityMultiplier = 2fに設定後：(0, -19.62, 0)
        if (Physics.gravity.y * parameter.GravityMultiplier > -20) Physics.gravity *= parameter.GravityMultiplier;
        deathCamera.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        PlayerJump();                                           //ジャンプ処理
        PlayerSpeedControl();                                   //ダッシュ処理
        PlayerMove();                                           //プレイヤーの移動
        PlayerShift();                                          //しゃがむ処理（内容未実装）
        Playerfire();                                           //プレイヤーの銃撃（バグ未解決）
        PlayerReload();                                         //リロード
        PlayerAbility();                                        //アビリティ（内容未実装）
        //マウスScrollで飲料選択。数字でも可
    }
    public void DrinkInInventory(GameObject drinkDate)
    {
        parameter.playerInventory.drinkType.Add(drinkDate);
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

        //Rayを使用し、壁抜け対策する
        //参考リンク：https://note.com/ryuryu_game/n/ncf259eb5f044
        //Rayの開始位置とRayの方向を設定
        Vector3 rayStart = transform.position;
        Vector3 rayDirection = desiredMoveDirection;

        //移動方向にRayを放射
        RaycastHit hit;
        if (!Physics.Raycast(rayStart, rayDirection, out hit, 0.5f))
        {
            if (desiredMoveDirection != Vector3.zero)
            {
                if (moveVertical != 0)
                {
                    rb.MovePosition(transform.position + desiredMoveDirection * currentSpeed * Time.fixedDeltaTime);
                }
            }
        }
        else
        {
            //TODO:ダッシュ壁抜け
            //ダッシュ中にすり抜ける問題を解決できていない
            currentSpeed = parameter.DownSpeed;

            //TODO:壁のぼり時がたつく
            //Rayが壁にヒットしていれば、壁を上る
            if (hit.collider.CompareTag("Wall"))
            {
                float climbSpeed = 5f;
                if (moveVertical > 0) desiredMoveDirection = Vector3.up * climbSpeed;
            }
        }
        rb.MovePosition(transform.position + desiredMoveDirection * currentSpeed * Time.fixedDeltaTime);
    }

    private void PlayerSpeedControl()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) currentSpeed = parameter.SprintSpeed;
        else currentSpeed = parameter.PlayerSpeed;
    }

    private void PlayerJump()
    {
        //AddForceだと何故かうまくいかなかったので、ベロシティでやってます
        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) rb.velocity = new Vector3(rb.velocity.x, parameter.JumpVelocity, rb.velocity.z);
    }

    private void PlayerShift()
    {
        //TODO:しゃがむ
        //シフトでしゃがむ
    }

    private void Playerfire()
    {
        if (Input.GetMouseButton(0)) gunManager.Shoot();
        //TODO:エイム
        //右マウスでエイム
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
        Debug.Log(coin);
    }

    private void OnCollisionStay(Collision collision)
    {
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
    }
}
