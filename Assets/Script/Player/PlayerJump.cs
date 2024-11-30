using UnityEngine;

/// <summary>
/// �v���C���[�̃W�����v�Ɋւ���X�N���v�g
/// </summary>
public class PlayerJump : MonoBehaviour
{
    private Rigidbody rb;
    private Status playerStatus = Status.GROUND;        //�v���C���[�̏��
    private float firstSpeed = 16.0f;                   //����
    private float gravity = 30.0f;                      //�d�͉����x
    private float timer = 0f;                           //�o�ߎ���
    private bool jumpKey = false;                       //�W�����v�L�[

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
            //�ڒn��
            case Status.GROUND:
                if (jumpKey) playerStatus = Status.UP;
                break;
            //�㏸��
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
            //������
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