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
        Debug.Log($"w“ü{drinkDate.drinkName}");
        if (eventData.pointerCurrentRaycast.gameObject == this.gameObject)
        {
            //‚±‚±‚Éw“üˆ—‚ğ‘‚­
            Debug.Log($"w“ü{drinkDate.drinkName}");
            eventData.pointerCurrentRaycast.gameObject.GetComponent<PlayerController>().DrinkInInventory(this.gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //ƒJ[ƒ\ƒ‹ƒƒbƒN‰ğœˆ—‚È‚Ç‚ğ•`‚­
        Debug.Log("‰ğœ");
        if (collision.gameObject == playerObject && Input.GetKey(KeyCode.F))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }
    }
}
