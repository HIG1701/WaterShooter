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