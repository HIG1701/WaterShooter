using UnityEngine;

[System.Serializable]
public class BulletDate
{
    [Header("ˆù—¿–¼"), SerializeField] public string drinkName;
    [Header("’e”"), SerializeField] private int drinkCapacity;
    [Header("’l’i"), SerializeField] private int drinkCost;
    [Header("e‚Ìí—Ş"), SerializeField] private GunType gunType;


    private enum GunType
    {
        ShotGun,
        AssaultRifle,
        SniperRifle,
        DesignatedMarksmanRifle,
        SubMachineGun,
        HundGun,
    }
}


