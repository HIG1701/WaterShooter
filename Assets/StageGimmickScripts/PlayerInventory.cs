using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/PlayerInventory")]


public class PlayerInventory : ScriptableObject
{
    [Header("�v���C���[�C���x���g��"), SerializeField] public List<GameObject> drinkType = new List<GameObject>();      //�v���C���[���������Ă���h�����N.
}
