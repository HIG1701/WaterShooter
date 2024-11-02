using UnityEngine;

//このスクリプトでは、プレイヤーの動き全般を実装する
//コードが長くなるので、銃撃とかは分けるかもしれない
//今後プレイヤーのパラメータを実装する際、スクリプタブルオブジェクトを用いるので、このコードももう少しコンパクトになるかも

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerParameter parameter;                 //プレイヤーパラメータ
    [SerializeField] private Transform CameraTransform;                 //カメラのTransform
    [SerializeField] private float CurrentSpeed;                        //現在Speed
    [SerializeField] private GunManager gunManager;
    [SerializeField] private Camera MainCamera;                         //mainカメラ
    [SerializeField] private Camera DeathCamera;                        //死亡時カメラ
    private GameManager gameManager;                                    //ゲームマネージャー
    private AbilityControl ability;                                     //Abilityスクリプト
    private Rigidbody Rb;
    private bool IsGrounded;                                            //地面と触れているか
    private int Coin;                                                   //コイン量
    private float CurrentHealth;                                        //現在HP

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();
        ability = FindObjectOfType<AbilityControl>();
    }

    private void Start()
    {
        CurrentSpeed = parameter.PlayerSpeed;                   //ParameterからSpeedを代入
        CurrentHealth = parameter.PlayerHP;                     //ParameterからHPを代入


        //このコメントは記述者が書いていて分からなくなったので、計算メモとして残してます
        //参考リンク：https://qiita.com/kaku0710/items/fdf5bab18b65f6f9dcb4
        //Physics.gravity：デフォルト：(0, -9.81, 0)
        //GravityMultiplier = 2fに設定後：(0, -19.62, 0)
        if (Physics.gravity.y * parameter.GravityMultiplier > -20) Physics.gravity *= parameter.GravityMultiplier;
        Coin = parameter.Coin;                                  //Parameterコインを代入
        DeathCamera.gameObject.SetActive(false);                //死亡時カメラを無効化
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

    private void PlayerMove()
    {
        float MoveHorizontal = Input.GetAxis("Horizontal");         //水平方向
        float MoveVertical = Input.GetAxis("Vertical");             //垂直方向
        Vector3 Forward = CameraTransform.forward;                  //カメラ前ベクトル
        Vector3 Right = CameraTransform.right;                      //カメラ右ベクトル
        Forward.y = 0f;                                             //水平面上の前方向のみを制御
        Right.y = 0f;                                               //水平面上の右方向のみを制御
        Forward.Normalize();
        Right.Normalize();

        //Forward * MoveVertical：カメラ前ベクトルに垂直方向を掛け、前後の移動方向を計算
        //Right * MoveHorizontal：カメラ右ベクトルに水平方向を掛け、左右の移動方向を計算
        Vector3 DesiredMoveDirection = Forward * MoveVertical + Right * MoveHorizontal;

        //Rayを使用し、壁抜け対策する
        //参考リンク：https://note.com/ryuryu_game/n/ncf259eb5f044
        //Rayの開始位置とRayの方向を設定
        Vector3 RayStart = transform.position;
        Vector3 RayDirection = DesiredMoveDirection;

        //移動方向にRayを放射
        RaycastHit Hit;
        if (!Physics.Raycast(RayStart, RayDirection, out Hit, 0.5f))
        {
            //Rayがヒットせず、かつプレイヤーが移動している場合
            //Vector3.zero：移動していない
            if (DesiredMoveDirection != Vector3.zero)
            {
                //プレイヤーが前進する場合のみ回転
                if (MoveVertical != 0)
                {
                    Rb.MovePosition(transform.position + DesiredMoveDirection * CurrentSpeed * Time.fixedDeltaTime);
                }
            }
        }
        else
        {
            //TODO:ダッシュ壁抜け
            //ダッシュ中にすり抜ける問題を解決できていない
            CurrentSpeed = parameter.DownSpeed;

            //Rayが壁にヒットしていれば、壁を上る
            if (Hit.collider.CompareTag("Wall"))
            {
                float ClimbSpeed = 5f;
                //上下移動の処理
                if (MoveVertical > 0) DesiredMoveDirection = Vector3.up * ClimbSpeed;
            }
        }
        Rb.MovePosition(transform.position + DesiredMoveDirection * CurrentSpeed * Time.fixedDeltaTime);
    }

    private void PlayerSpeedControl()
    {
        //コントロールキーが押されていたら速度アップ
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) CurrentSpeed = parameter.SprintSpeed;
        else CurrentSpeed = parameter.PlayerSpeed;
    }

    private void PlayerJump()
    {
        //AddForceだと何故かうまくいかなかったので、ベロシティでやってます
        if (IsGrounded && Input.GetKeyDown(KeyCode.Space)) Rb.velocity = new Vector3(Rb.velocity.x, parameter.JumpVelocity, Rb.velocity.z);
    }

    private void PlayerShift()
    {
        //シフトでしゃがむ
    }

    private void Playerfire()
    {
        //左クリック時にShootメソッドを呼び出す
        if (Input.GetMouseButton(0)) gunManager.Shoot();
        //右マウスでエイム
    }

    private void PlayerReload()
    {
        if (Input.GetKeyDown(KeyCode.R)) gunManager.StartReload();
    }

    private void PlayerAbility()
    {
        if (ability == null) return;
        ability.Ability();
    }

    private void Die()
    {
        gameObject.SetActive(false);
        //リスポーンタイマーを開始させる
        gameManager.StartRespawnTimer(gameObject);
        //メインカメラを無効化し、死亡時カメラを有効化
        MainCamera.gameObject.SetActive(false);
        DeathCamera.gameObject.SetActive(true);
        DeathCamera.transform.position = transform.position + new Vector3(0, 10, 0); //上空に配置
        DeathCamera.transform.LookAt(transform.position);
    }
    public void Respawn()
    {
        gameObject.SetActive(true);

        //死亡時カメラを無効化し、メインカメラを有効化
        DeathCamera.gameObject.SetActive(false);
        MainCamera.gameObject.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //プレイヤーコイン量追加
        if (collision.gameObject.CompareTag("Coin"))
        {
            CoinManager coinManager = collision.gameObject.GetComponent<CoinManager>();
            if (coinManager != null)
            {
                Coin += coinManager.Coin;
                Destroy(collision.gameObject);
            }
        }
        Debug.Log(Coin);
    }

    private void OnCollisionStay(Collision collision)
    {
        //地面に接触しているかどうかをチェック
        if (collision.gameObject.CompareTag("Ground")) IsGrounded = true;

        //DamageAreaに触れている間にHPを減少させる
        if (collision.gameObject.CompareTag("DamageArea"))
        {
            CurrentHealth -= 9999 * Time.deltaTime;
            if (CurrentHealth <= 0) Die();
        }

        ////給水エリア場で、常に玉がフル装填される
        //if (collision.gameObject.CompareTag("WaterArea")) gunManager.WaterReload();
    }

    private void OnCollisionExit(Collision collision)
    {
        //地面から離れたとき
        if (collision.gameObject.CompareTag("Ground")) IsGrounded = false;
    }
}