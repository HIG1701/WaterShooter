using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float PlayerSpeed = 50f;                   //�ʏ푬�x
    [SerializeField] float SprintSpeed = 100f;                  //����
    [SerializeField] float JumpForce = 50f;                     //�W�����v��
    [SerializeField] Transform CameraTransform;                 //�J������Transform

    private float CurrentSpeed;
    private bool IsGrounded;                                    //�n�ʂƐG��Ă��邩
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
        PlayerShift();
        Playerfire();
        //�����[�hR�L�[
        //�}�E�XScroll�ň����I���B�����ł���
        //�ǂɌ�������WS�L�[
        //�A�r���e�BQ�L�[
    }

    private void PlayerMove()
    {
        float MoveHorizontal = Input.GetAxis("Horizontal");
        float MoveVertical = Input.GetAxis("Vertical");

        //�J�����̑O�����ƉE��������Ɉړ��������v�Z
        Vector3 forward = CameraTransform.forward;
        Vector3 right = CameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * MoveVertical + right * MoveHorizontal;

        if (desiredMoveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredMoveDirection);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f));
        }

        rb.MovePosition(transform.position + desiredMoveDirection * CurrentSpeed * Time.fixedDeltaTime);
    }

    private void PlayerDash()
    {
        //�R���g���[���L�[��������Ă����瑬�x�A�b�v
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) CurrentSpeed = SprintSpeed;
        else CurrentSpeed = PlayerSpeed;
    }

    private void PlayerJump()
    {
        if (IsGrounded && Input.GetKeyDown(KeyCode.Space)) rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
    }
    private void PlayerShift()
    {
        //�V�t�g�ł��Ⴊ��
    }

    private void Playerfire()
    {
        //���}�E�X�Ŕ���
        //�E�}�E�X�ŃG�C��
    }
    private void OnCollisionStay(Collision collision)
    {
        //�n�ʂɐڐG���Ă��邩�ǂ������`�F�b�N
        if (collision.gameObject.CompareTag("Ground")) IsGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        //�n�ʂ��痣�ꂽ�Ƃ�
        if (collision.gameObject.CompareTag("Ground")) IsGrounded = false;
    }
}