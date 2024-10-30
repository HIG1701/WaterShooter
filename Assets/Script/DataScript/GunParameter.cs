using UnityEngine;

//���̃X�N���v�^�u���I�u�W�F�N�g�ɁA�e��Parameter�������܂�
[CreateAssetMenu(menuName = "ScriptableObject/GunParameter")]

public class GunParameter : ScriptableObject
{
    [Header("�e�̐ݒ�Ɋւ���Parameter")]
    public string WeaponName;               //���햼

    [Header("�e��InGame�ɉe������Parameter")]
    public float GunPower;                  //�U����
    public float AttackSpeed;               //�U�����x
    public float AttackRange;               //�˒�����
    public float ReloadSpeed;               //�����[�h���x
    public int MaxAmmo;                     //�ő�e��
}