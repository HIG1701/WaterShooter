using UnityEngine;

/// <summary>
/// �W�����v����N���X
/// </summary>
public class PlayerJump : MonoBehaviour
{
    private Rigidbody rb;
    private Status playerStatus = Status.GROUND;        //���
    private float timeKeeper = 0f;                      //�o�ߎ��Ԍv��
    private bool jumpKey = false;                       //�W�����v�L�[
    private Animator animator;
    readonly float lowerLimit = 0.03f;                  //�W�����v���Ԑ���
    readonly float gravity = 30.0f;
    [SerializeField] private PlayerParameter playerParameter;

    private enum Status
    {
        GROUND,
        UP,
        DOWN
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        jumpKey = Input.GetKey(KeyCode.Space);
    }

    private void FixedUpdate()
    {
        Jump();
    }

    private void Jump()
    {
        //Mathf.Pow�F�ׂ���v�Z
        //Mathf.Pow(2, 3) = 2^3 = 8

        //���x�x�N�g���X�V
        Vector3 newvec = Vector3.zero;

        switch (playerStatus)
        {
            case Status.GROUND:
                if (jumpKey)
                {
                    playerStatus = Status.UP;
                    animator.SetTrigger("Jump");
                    animator.SetBool("isGround", false);
                }
                break;
            case Status.UP:
                timeKeeper += Time.deltaTime;

                if (jumpKey || lowerLimit > timeKeeper)
                {
                    newvec.y = playerParameter.InitialVelocity;
                    newvec.y -= (gravity * Mathf.Pow(timeKeeper, 2));   //���Ԃ��猸�Z�l�Z�o
                }
                else
                {
                    timeKeeper += Time.deltaTime;
                    newvec.y = playerParameter.InitialVelocity;
                    newvec.y -= (gravity * Mathf.Pow(timeKeeper, 2));
                }
                if (0f > newvec.y)
                {
                    playerStatus = Status.DOWN;
                    newvec.y = 0f;
                    timeKeeper = 0.1f;
                }
                break;
            case Status.DOWN:
                timeKeeper += Time.deltaTime;
                newvec.y = 0f;
                newvec.y = -(gravity * timeKeeper);
                break;
        }

        //�����X�V
        rb.velocity = newvec;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (playerStatus == Status.DOWN && collision.gameObject.CompareTag("Ground"))
        {
            playerStatus = Status.GROUND;
            timeKeeper = 0f;
            animator.ResetTrigger("Jump");
            animator.SetBool("isGround", true);
        }
    }
}