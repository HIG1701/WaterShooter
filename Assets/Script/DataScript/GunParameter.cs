using UnityEngine;

//���̃X�N���v�^�u���I�u�W�F�N�g�ɁA�e��Parameter�������܂�
[CreateAssetMenu(menuName = "ScriptableObject/GunParameter")]

public class GunParameter : ScriptableObject
{
    [Header("�e�̐ݒ�Ɋւ���Parameter")]
    [SerializeField] private string weaponName;               //���햼

    [Header("�e��InGame�ɉe������Parameter")]
    [SerializeField] private float gunPower;                  //�U����
    [SerializeField] private float fireRate;                  //�U�����x
    [SerializeField] private float bulletSpeed;               //�e��
    [SerializeField] private float attackRange;               //�˒�����
    [SerializeField] private float reloadTime;                //�����[�h����
    [SerializeField] private int maxAmmo;                     //�ő�e��

    public string WeaponName => weaponName;
    public float GunPower => gunPower;
    public float FireRate => fireRate;
    public float BulletSpeed => bulletSpeed;
    public float AttackRange => attackRange;
    public float ReloadTime => reloadTime;
    public int MaxAmmo => maxAmmo;
}