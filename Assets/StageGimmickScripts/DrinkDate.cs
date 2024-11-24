using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEditor;
using System;
using static UnityEditor.Progress;

[System.Serializable]
public class DrinkDate
{
    [SerializeField] public string drinkName;
    [SerializeField] private int drinkCapacity;
    [SerializeField] private int drinkCost;
    [SerializeField] private GunType gunType;


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


