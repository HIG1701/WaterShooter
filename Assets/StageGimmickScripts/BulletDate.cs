using UnityEngine;

[System.Serializable]
public class BulletDate
{
    [Header("ˆù—¿–¼"), SerializeField] public string drinkName;
    [Header("’e”"), SerializeField] public int drinkCapacity = 60;
    [Header("’l’i"), SerializeField] public int drinkCost = 60;
    [Header("ƒq[ƒ‹"), SerializeField] public int drinkHeal = 0;
    [Header("1ƒXƒd‚ËŠ|‚¯"), SerializeField] public int slotField = 120;
    [Header("e‚Ìí—Ş"), SerializeField] public GunType gunType;


    public enum GunType
    {
        Null,
        Heal,
        Gun,
        DesignatedMarksmanRifle,
        SubMachineGun,
        HundGun,
    }
}


