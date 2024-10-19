using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float PlayerSpeed = 50f;                   //通常速度
    [SerializeField] float SprintSpeed = 100f;                  //加速
    [SerializeField] float JumpForce = 50f;                     //ジャンプ力
    private float CurrentSpeed;
    private bool IsGrounded;                                    //地面と触れているか
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        CurrentSpeed = PlayerSpeed;
    }

    private void FixedUpdate()
    {
        //PlayerJump();
        PlayerDash();
        PlayerMove();
    }

    private void Update()
    {
        PlayerShift();
        Playerfire();
        //リロードRキー
        //マウスScrollで飲料選択。数字でも可
        //壁に向かってWSキー
        //アビリティQキー
    }

    private void PlayerMove()
    {
        float MoveHorizontal = Input.GetAxis("Horizontal");
        float MoveVertical = Input.GetAxis("Vertical");

        Vector3 Movement = new Vector3(MoveHorizontal, 0.0f, MoveVertical);

        if (Movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Movement);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f));
        }

        rb.MovePosition(transform.position + Movement * CurrentSpeed * Time.fixedDeltaTime);
    }

    private void PlayerDash()
    {
        //コントロールキーが押されていたら速度アップ
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) CurrentSpeed = SprintSpeed;
        else CurrentSpeed = PlayerSpeed;
    }

    private void PlayerJump()
    {
        if (IsGrounded && Input.GetKeyDown(KeyCode.Space)) rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
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