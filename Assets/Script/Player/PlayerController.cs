using UnityEngine;

//���̃X�N���v�g�ł́A�v���C���[�̓����S�ʂ���������
//����v���C���[�̃p�����[�^����������ہA�X�N���v�^�u���I�u�W�F�N�g��p����̂ŁA���̃R�[�h�����������R���p�N�g�ɂȂ邩��

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerParameter parameter;                 //�v���C���[�p�����[�^
    [SerializeField] Transform CameraTransform;                 //�J������Transform
    [SerializeField] private float CurrentSpeed;                //����Speed
    private Rigidbody Rb;
    private bool IsGrounded;                                    //�n�ʂƐG��Ă��邩
    private CoinManager coinManager;                            //�R�C���}�l�[�W���[�{��
    private int Coin;                                           //�R�C����

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        coinManager = FindObjectOfType<CoinManager>();
    }

    private void Start()
    {
        CurrentSpeed = parameter.PlayerSpeed;

        //���̃R�����g�͋L�q�҂������Ă��ĕ�����Ȃ��Ȃ����̂ŁA�v�Z�����Ƃ��Ďc���Ă܂�
        //�Q�l�����N�Fhttps://qiita.com/kaku0710/items/fdf5bab18b65f6f9dcb4
        //Physics.gravity�F�f�t�H���g�F(0, -9.81, 0)
        //GravityMultiplier = 2f�ɐݒ��F(0, -19.62, 0)
        Physics.gravity *= parameter.GravityMultiplier;                   //�d�͂����߂�
        Coin = parameter.Coin;                                            //Parameter�R�C������
    }

    private void FixedUpdate()
    {
        PlayerJump();
        PlayerSpeedControl();
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
        float MoveHorizontal = Input.GetAxis("Horizontal");         //��������
        float MoveVertical = Input.GetAxis("Vertical");             //��������
        Vector3 Forward = CameraTransform.forward;                  //�J�����O�x�N�g��
        Vector3 Right = CameraTransform.right;                      //�J�����E�x�N�g��
        Forward.y = 0f;                                             //�����ʏ�̑O�����݂̂𐧌�
        Right.y = 0f;                                               //�����ʏ�̉E�����݂̂𐧌�
        Forward.Normalize();
        Right.Normalize();

        //Forward * MoveVertical�F�J�����O�x�N�g���ɐ����������|���A�O��̈ړ��������v�Z
        //Right * MoveHorizontal�F�J�����E�x�N�g���ɐ����������|���A���E�̈ړ��������v�Z
        Vector3 DesiredMoveDirection = Forward * MoveVertical + Right * MoveHorizontal;

        //Ray���g�p���A�ǔ����΍􂷂�
        //�Q�l�����N�Fhttps://note.com/ryuryu_game/n/ncf259eb5f044
        //Ray�̊J�n�ʒu��Ray�̕�����ݒ�
        Vector3 RayStart = transform.position;
        Vector3 RayDirection = DesiredMoveDirection;

        //�ړ�������Ray�����
        RaycastHit Hit;
        if (!Physics.Raycast(RayStart, RayDirection, out Hit, 0.5f))
        {
            //Ray���q�b�g�����A���v���C���[���ړ����Ă���ꍇ
            if (DesiredMoveDirection != Vector3.zero)
            {
                Quaternion TargetRotation = Quaternion.LookRotation(DesiredMoveDirection);
                Rb.MoveRotation(Quaternion.Slerp(transform.rotation, TargetRotation, Time.fixedDeltaTime * 10f));

                //����
                //Quaternion.LookRotation(DesiredMoveDirection)�FDesiredMoveDirection�̕�����������]���v�Z
                //Quaternion.Slerp(a,b,c)�Fa����b�ւ̉�]��⊮
            }
        }
        else
        {
            //�_�b�V�����ɂ��蔲������������ł��Ă��Ȃ�
            CurrentSpeed = parameter.DownSpeed;

            //Ray���ǂɃq�b�g���Ă���΁A�ǂ����
            if (Hit.collider.CompareTag("Wall"))
            {
                float ClimbSpeed = 5f;
                // �㉺�ړ��̏���
                if (MoveVertical > 0)
                {
                    DesiredMoveDirection = Vector3.up * ClimbSpeed;
                }
            }
        }

        Rb.MovePosition(transform.position + DesiredMoveDirection * CurrentSpeed * Time.fixedDeltaTime);
    }

    private void PlayerSpeedControl()
    {
        //�R���g���[���L�[��������Ă����瑬�x�A�b�v
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) CurrentSpeed = parameter.SprintSpeed;
        else CurrentSpeed = parameter.PlayerSpeed;
    }

    private void PlayerJump()
    {
        //AddForce���Ɖ��̂����܂������Ȃ������̂ŁA�x���V�e�B�ł���Ă܂�
        if (IsGrounded && Input.GetKeyDown(KeyCode.Space)) Rb.velocity = new Vector3(Rb.velocity.x, parameter.JumpVelocity, Rb.velocity.z);
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

    private void OnCollisionEnter(Collision collision)
    {
        ////�v���C���[�R�C���ʒǉ�
        //if (collision.gameObject.CompareTag("Coin"))
        //{
        //    collision.gameObject.GetComponents<CoinManager>();
        //    Coin += coinManager.Coin;
        //}
        //Debug.Log(Coin);
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