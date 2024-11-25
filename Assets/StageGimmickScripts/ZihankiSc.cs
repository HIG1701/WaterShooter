using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class ZihankiSc : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private DrinkDate drinkDate = new DrinkDate();
    [SerializeField] private GameObject playerObject;
    [Header("PlayerTag�I�u�W�F�N�g"), SerializeField] private GameObject playerTagObject;

    public void OnPointerClick(PointerEventData eventData)
    {
        var childPlayerObject = playerObject.transform.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.CompareTag(playerTagObject.tag));
        Debug.Log($"�w��{drinkDate.drinkName}");
        if (eventData.pointerClick.gameObject == this.gameObject.CompareTag(tag))
        {
            //�����ɍw������������
            Debug.Log($"�w������{eventData.pointerClick}");
            Debug.Log(childPlayerObject);
            childPlayerObject.gameObject.GetComponent<PlayerController>().DrinkInInventory(eventData.pointerClick.gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //�J�[�\�����b�N���������Ȃǂ�`��
        Debug.Log("����");
        if (collision.gameObject == playerObject && Input.GetKey(KeyCode.F))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            playerObject = collision.gameObject;

            return;
        }
    }

    /// <summary>
    /// �f�o�b�O�p�Ȃ̂ō폜���܂��B
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision)
    {
        var childPlayerObject = playerObject.transform.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.CompareTag(playerTagObject.tag));
        var obj = childPlayerObject.gameObject.GetComponent<PlayerController>().ReturnValue();
        if (obj.playerInventory.drinkType[0] == this.gameObject && obj.playerInventory.drinkType != null)
        {
            Debug.Log(obj.playerInventory.drinkType[0]);
            Debug.Log("����");
        }
    }
}
