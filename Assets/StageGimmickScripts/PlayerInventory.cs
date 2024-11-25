using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/PlayerInventory")]


public class PlayerInventory : ScriptableObject
{
    [Header("プレイヤーインベントリ"), SerializeField] public List<GameObject> drinkType = new List<GameObject>();      //プレイヤーが所持しているドリンク.
}
