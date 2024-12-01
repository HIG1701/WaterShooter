using UnityEngine;

/// <summary>
/// ジャンプ制御クラス
/// </summary>
public class PlayerJump : MonoBehaviour
{
    private Rigidbody rb;
    private Status playerStatus = Status.GROUND;                         //状態
    readonly float jumpLowerLimit = 0.03f;                               //ジャンプ時間の下限
    readonly float initialVelocity = 16.0f;             //初速
    readonly float gravity = 30.0f;                     //重力加速度
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
            case Status.GROUND:
                if (jumpKey) playerStatus = Status.UP;
                break;
            case Status.UP:
                timer += Time.deltaTime;

                if (jumpKey || jumpLowerLimit > timer)
                {
                    newvec.y = initialVelocity;
                    newvec.y -= (gravity * Mathf.Pow(timer, 2));
                }
                else
                {
                    timer += Time.deltaTime;
                    newvec.y = initialVelocity;
                    newvec.y -= (gravity * Mathf.Pow(timer, 2));
                }
                if (0f > newvec.y)
                {
                    playerStatus = Status.DOWN;
                    newvec.y = 0f;
                    timer = 0.1f;
                }
                break;
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