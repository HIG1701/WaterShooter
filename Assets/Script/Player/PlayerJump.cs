using UnityEngine;

/// <summary>
/// プレイヤーのジャンプに関するスクリプト
/// </summary>
public class PlayerJump : MonoBehaviour
{
    private Rigidbody rb;
    private Status playerStatus = Status.GROUND;        //プレイヤーの状態
    private float firstSpeed = 16.0f;                   //初速
    private float gravity = 30.0f;                      //重力加速度
    private float timer = 0f;                           //経過時間
    private bool jumpKey = false;                       //ジャンプキー

    private enum Status
    {
        GROUND,
        UP,
        DOWN
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        jumpKey = Input.GetKey(KeyCode.Space);
    }

    private void FixedUpdate()
    {
        Vector2 newvec = Vector2.zero;

        switch (playerStatus)
        {
            //接地時
            case Status.GROUND:
                if (jumpKey) playerStatus = Status.UP;
                break;
            //上昇時
            case Status.UP:
                timer += Time.deltaTime;
                if (jumpKey && rb.velocity.y >= 0f)
                {
                    newvec.y = firstSpeed;
                    newvec.y -= (gravity * timer);
                }
                else
                {
                    playerStatus = Status.DOWN;
                    timer = 0f;
                }
                break;
            //落下時
            case Status.DOWN:
                timer += Time.deltaTime;
                newvec.y = 0f;
                newvec.y = -(gravity * timer);
                break;
            default:
                break;
        }

        rb.velocity = newvec;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (playerStatus == Status.DOWN && collision.gameObject.CompareTag("Ground"))
        {
            playerStatus = Status.GROUND;
            timer = 0f;
        }
    }
}