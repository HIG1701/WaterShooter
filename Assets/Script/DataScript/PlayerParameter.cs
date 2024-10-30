using UnityEngine;

//���̃X�N���v�^�u���I�u�W�F�N�g�ɁA�v���C���[��Parameter�������܂�
[CreateAssetMenu(menuName = "ScriptableObject/PlayerParameter")]

public class PlayerParameter : ScriptableObject
{
    [Header("�v���C���[�̐ݒ�Ɋւ���Parameter")]
    public int PlayerID;                    //�v���C���[ID
    public string PlayerName;               //�v���C���[��
    public float PlayerHeight;              //�v���C���[�g��

    [Header("�v���C���[��InGame�ɉe������Parameter")]
    public float PlayerHP;                  //�̗�
    public float PlayerShield;              //�V�[���h�ϋv��
    public int Coin;                        //�R�C������

    [Header("���x�Ɋւ���Parameter")]
    public float PlayerSpeed;               //�ʏ푬�x
    public float SprintSpeed;               //����
    public float DownSpeed;                 //�ǂɓ������Ă��鎞
    public float JumpVelocity;              //�W�����v���x

    [Header("�d�͂Ɋւ���Parameter")]
    public float GravityMultiplier;         //���̃I�u�W�F�N�g�̂݁A�d�͂̉e�������߂�
}