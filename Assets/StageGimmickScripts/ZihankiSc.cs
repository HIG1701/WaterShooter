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
    [Header("PlayerTagオブジェクト"), SerializeField] private GameObject playerTagObject;

    public void OnPointerClick(PointerEventData eventData)
    {
        var childPlayerObject = playerObject.transform.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.CompareTag(playerTagObject.tag));
        Debug.Log($"購入{drinkDate.drinkName}");
        if (eventData.pointerClick.gameObject == this.gameObject.CompareTag(tag))
        {
            //ここに購入処理を書く
            Debug.Log($"購入完了{eventData.pointerClick}");
            Debug.Log(childPlayerObject);
            childPlayerObject.gameObject.GetComponent<PlayerController>().DrinkInInventory(eventData.pointerClick.gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //カーソルロック解除処理などを描く
        Debug.Log("解除");
        if (collision.gameObject == playerObject && Input.GetKey(KeyCode.F))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            playerObject = collision.gameObject;

            return;
        }
    }

    /// <summary>
    /// デバッグ用なので削除します。
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision)
    {
        var childPlayerObject = playerObject.transform.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.CompareTag(playerTagObject.tag));
        var obj = childPlayerObject.gameObject.GetComponent<PlayerController>().ReturnValue();
        if (obj.playerInventory.drinkType[0] == this.gameObject && obj.playerInventory.drinkType != null)
        {
            Debug.Log(obj.playerInventory.drinkType[0]);
            Debug.Log("成功");
        }
    }
}
