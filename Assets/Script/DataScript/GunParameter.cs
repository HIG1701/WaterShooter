using UnityEngine;

//このスクリプタブルオブジェクトに、銃のParameterを書きます
[CreateAssetMenu(menuName = "ScriptableObject/GunParameter")]

public class GunParameter : ScriptableObject
{
    [Header("銃の設定に関するParameter")]
    public string WeaponName;               //武器名

    [Header("銃のInGameに影響するParameter")]
    public float GunPower;                  //攻撃力
    public float FireRate;                  //攻撃速度
    public float BulletSpeed;               //弾速
    public float AttackRange;               //射程距離
    public float ReloadTime;                //リロード時間
    public int MaxAmmo;                     //最大弾数
}