using UnityEngine;

//���̃X�N���v�^�u���I�u�W�F�N�g�ɁA�v���C���[��Parameter�������܂�
[CreateAssetMenu(menuName = "ScriptableObject/PlayerParameter")]
public class PlayerParameter : ScriptableObject
{
    //���x�Ɋւ���Parameter
    public float PlayerSpeed;               //�ʏ푬�x
    public float SprintSpeed;               //����
    public float DownSpeed;                 //�ǂɓ������Ă��鎞
    public float JumpVelocity;              //�W�����v���x

    //�d�͂Ɋւ���Parameter
    public float GravityMultiplier;         //���̃I�u�W�F�N�g�̂݁A�d�͂̉e�������߂�

}
