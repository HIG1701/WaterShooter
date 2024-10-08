using UnityEngine;

//基底クラスにします（暫定）

public class PlayerController : MonoBehaviour
{
    private float speed = 30.0f;            //移動速度
    private float jumpForce = 20f;          //ジャンプ
    private Rigidbody rb;
    private bool isGrounded;                //設置判定

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        PlayerMove();                       //移動
        PlayerJump();                       //ジャンプ
        PlayerDash();                       //ダッシュ
        MoveCamera();                       //カメラ
    }

    private void PlayerMove()
    {
        // Playerの前後左右の移動
        float xMovement = Input.GetAxis("Horizontal") * speed * Time.deltaTime;             //左右の移動
        float zMovement = Input.GetAxis("Vertical") * speed * Time.deltaTime;               //前後の移動
        transform.Translate(xMovement, 0, zMovement);                                       //オブジェクトの位置を更新
    }


    private void PlayerJump()
    {
        //地面と触れてたら
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void PlayerDash()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 50.0f;
        }
        else
        {
            speed = 50.0f;
        }
    }

    private void MoveCamera()
    {
        float mx = Input.GetAxis("Mouse X");                            //カーソルの横の移動量を取得
        float my = Input.GetAxis("Mouse Y");                            //カーソルの縦の移動量を取得
        if (Mathf.Abs(mx) > 0.001f)                                     //X方向に一定量移動していれば横回転
        {
            //transform.RotateAround(回転の中心, 回転の軸（Vector3.upは(0,1,0)のことなのでｙ軸を軸としている）, 変化量); 
            transform.RotateAround(transform.position, Vector3.up, mx); //回転軸はplayerオブジェクトのワールド座標Y軸
        }
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
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}