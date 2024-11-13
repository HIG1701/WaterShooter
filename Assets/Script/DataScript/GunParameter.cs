using UnityEngine;

//‚±‚ÌƒXƒNƒŠƒvƒ^ƒuƒ‹ƒIƒuƒWƒFƒNƒg‚ÉAe‚ÌParameter‚ð‘‚«‚Ü‚·
[CreateAssetMenu(menuName = "ScriptableObject/GunParameter")]

public class GunParameter : ScriptableObject
{
    [Header("e‚ÌÝ’è‚ÉŠÖ‚·‚éParameter")]
    [SerializeField] private string weaponName;               //•Ší–¼

    [Header("e‚ÌInGame‚É‰e‹¿‚·‚éParameter")]
    [SerializeField] private float gunPower;                  //UŒ‚—Í
    [SerializeField] private float fireRate;                  //UŒ‚‘¬“x
    [SerializeField] private float bulletSpeed;               //’e‘¬
    [SerializeField] private float attackRange;               //ŽË’ö‹——£
    [SerializeField] private float reloadTime;                //ƒŠƒ[ƒhŽžŠÔ
    [SerializeField] private int maxAmmo;                     //Å‘å’e”

    public string WeaponName => weaponName;
    public float GunPower => gunPower;
    public float FireRate => fireRate;
    public float BulletSpeed => bulletSpeed;
    public float AttackRange => attackRange;
    public float ReloadTime => reloadTime;
    public int MaxAmmo => maxAmmo;
}