using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class ZihankiSc : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private DrinkDate drinkDate = new DrinkDate();
    [SerializeField] private GameObject playerObject;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"�w��{drinkDate.drinkName}");
        if (eventData.pointerCurrentRaycast.gameObject == this.gameObject)
        {
            //�����ɍw������������
            Debug.Log($"�w��{drinkDate.drinkName}");
            eventData.pointerCurrentRaycast.gameObject.GetComponent<PlayerController>().DrinkInInventory(this.gameObject);
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
            return;
        }
    }
}
