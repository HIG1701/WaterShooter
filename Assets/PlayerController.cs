using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float PlayerSpeed = 50f;                   //通常速度
    [SerializeField] float SprintSpeed = 100f;                  //加速
    [SerializeField] float DownSpeed = 10f;                     //壁に当たっている時
    [SerializeField] float JumpForce = 50f;                     //ジャンプ力
    [SerializeField] float jumpVelocity = 10f;                  //ジャンプ速度
    [SerializeField] Transform CameraTransform;                 //カメラのTransform

    private float GravityMultiplier = 2f;                       //このオブジェクトのみ、重力の影響を強める
    [SerializeField] private float CurrentSpeed;
    private bool IsGrounded;                                    //地面と触れているか
    private Rigidbody Rb;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        CurrentSpeed = PlayerSpeed;

        //このコメントは記述者が書いていて分からなくなったので、計算メモとして残してます
        //参考リンク：https://qiita.com/kaku0710/items/fdf5bab18b65f6f9dcb4
        //Physics.gravity デフォルト：(0, -9.81, 0)
        //GravityMultiplier = 2fに設定後：(0, -19.62, 0)
        Physics.gravity *= GravityMultiplier;                   //重力を強める
    }

    private void FixedUpdate()
    {
        PlayerJump();
        PlayerSpeedControl();
        PlayerMove();
        PlayerShift();
        Playerfire();
        //リロードRキー
        //マウスScrollで飲料選択。数字でも可
        //壁に向かってWSキー
        //アビリティQキー
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
            if (DesiredMoveDirection != Vector3.zero)
            {
                Quaternion TargetRotation = Quaternion.LookRotation(DesiredMoveDirection);
                Rb.MoveRotation(Quaternion.Slerp(transform.rotation, TargetRotation, Time.fixedDeltaTime * 10f));

                //メモ
                //Quaternion.LookRotation(DesiredMoveDirection):DesiredMoveDirectionの方向を向く回転を計算
                //Quaternion.Slerp(a,b,c):aからbへの回転を補完
            }
        }
        else
        {
            //ダッシュ中にすり抜ける問題を解決できていない
            CurrentSpeed = DownSpeed;
        }

        Rb.MovePosition(transform.position + DesiredMoveDirection * CurrentSpeed * Time.fixedDeltaTime);
    }

    private void PlayerSpeedControl()
    {
        //コントロールキーが押されていたら速度アップ
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) CurrentSpeed = SprintSpeed;
        else CurrentSpeed = PlayerSpeed;
    }

    private void PlayerJump()
    {
        //AddForceだと何故かうまくいかなかったので、ベロシティでやってます
        if (IsGrounded && Input.GetKeyDown(KeyCode.Space)) Rb.velocity = new Vector3(Rb.velocity.x, jumpVelocity, Rb.velocity.z);
    }
    private void PlayerShift()
    {
        //シフトでしゃがむ
    }

    private void Playerfire()
    {
        //左マウスで発射
        //右マウスでエイム
    }
    private void OnCollisionStay(Collision collision)
    {
        //地面に接触しているかどうかをチェック
        if (collision.gameObject.CompareTag("Ground")) IsGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        //地面から離れたとき
        if (collision.gameObject.CompareTag("Ground")) IsGrounded = false;
    }
}