using UnityEngine;

//���N���X�ɂ��܂��i�b��j

public class PlayerController : MonoBehaviour
{
    private float speed = 30.0f;            //�ړ����x
    private float jumpForce = 20f;          //�W�����v
    private Rigidbody rb;
    private bool isGrounded;                //�ݒu����

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        PlayerMove();                       //�ړ�
        PlayerJump();                       //�W�����v
        PlayerDash();                       //�_�b�V��
    }

    private void PlayerMove()
    {
        // Player�̑O�㍶�E�̈ړ�
        float xMovement = Input.GetAxis("Horizontal") * speed * Time.deltaTime;             //���E�̈ړ�
        float zMovement = Input.GetAxis("Vertical") * speed * Time.deltaTime;               //�O��̈ړ�
        transform.Translate(xMovement, 0, zMovement);                                       //�I�u�W�F�N�g�̈ʒu���X�V
    }


    private void PlayerJump()
    {
        //�n�ʂƐG��Ă���
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