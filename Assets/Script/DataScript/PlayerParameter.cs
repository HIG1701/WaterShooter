using UnityEngine;

//���̃X�N���v�^�u���I�u�W�F�N�g�ɁA�v���C���[��Parameter�������܂�
[CreateAssetMenu(menuName = "ScriptableObject/PlayerParameter")]
public class PlayerParameter : ScriptableObject
{
    [Header("�v���C���[�̐ݒ�Ɋւ���Parameter")]
    [SerializeField] private int playerID;                    //�v���C���[ID
    [SerializeField] private string playerName;               //�v���C���[��
    [SerializeField] private float playerHeight;              //�v���C���[�g��

    [Header("�v���C���[��InGame�ɉe������Parameter")]
    [SerializeField] private float playerHP;                  //�̗�
    [SerializeField] private float playerShield;              //�V�[���h�ϋv��
    [SerializeField] private float initialVelocity;           //�W�����v��

    [Header("���x�Ɋւ���Parameter")]
    [SerializeField] private float playerSpeed;               //�ʏ푬�x
    [SerializeField] private float sprintSpeed;               //����
    [SerializeField] private float climbSpeed;                //�ǂ���鑬�x
    [SerializeField] private float jumpVelocity;              //�W�����v���x

    public int PlayerID => playerID;
    public string PlayerName => playerName;
    public float PlayerHeight => playerHeight;
    public float PlayerHP => playerHP;
    public float PlayerShield => playerShield;
    public float InitialVelocity => initialVelocity;
    public float PlayerSpeed => playerSpeed;
    public float SprintSpeed => sprintSpeed;
    public float ClimbSpeed => climbSpeed;
    public float JumpVelocity => jumpVelocity;
}