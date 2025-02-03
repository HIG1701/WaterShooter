using UnityEngine;

/// <summary>
/// プレイヤーに関するクラス
/// </summary>

//TODO:死亡時のカメラの処理をここに書くな。今後設計を考え直せ

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerParameter parameter;
    [SerializeField] public int CharacterIndex = 0;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float currentSpeed;                        //現在Speed
    [SerializeField] private GunManager gunManager;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera deathCamera;
    private Rigidbody rb;
    private bool isGrounded;
    private BotAnimation characterAnimation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        characterAnimation = GetComponent<BotAnimation>();

    }

    private void Start()
    {
        currentSpeed = parameter.PlayerSpeed;

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
        //PlayerSpeedControl();                                   //ダッシュ処理
        PlayerMove();                                           //プレイヤーの移動
        //PlayerShift();                                          //しゃがむ処理(内容未実装)
        Playerfire();                                           //プレイヤーの銃撃(バグ未解決)
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
        characterAnimation.CharaAnimation(moveHorizontal, moveVertical);
    }

    //private void PlayerSpeedControl()
    //{
    //    if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) currentSpeed = parameter.SprintSpeed;
    //    else currentSpeed = parameter.PlayerSpeed;
    //}

    private void PlayerShift()
    {
        //if (Input.GetKey(KeyCode.LeftShift))
        //{
        //    characterAnimation.ShiftAnimation();
        //}
        //else if (!Input.GetKey(KeyCode.LeftShift))
        //{
        //    characterAnimation.AnimationReset();
        //}
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

    private void OnCollisionEnter(Collision collision)
    {
        //プレイヤーコイン量追加
        if (collision.gameObject.TryGetComponent<CoinScript>(out var coinManager))
        {
            SetDateManager.AddCoin(CharacterIndex, coinManager.Coin);
            Destroy(collision.gameObject);
        }
        //Debug.Log(coin);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = false;
    }
}