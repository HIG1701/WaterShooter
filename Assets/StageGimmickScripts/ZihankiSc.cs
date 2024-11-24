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

    void Start()
    {
        Debug.Log("aaaa");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"w“ü{drinkDate.drinkName}");
        if (eventData.pointerCurrentRaycast.gameObject == this.gameObject)
        {
            //‚±‚±‚Éw“üˆ—‚ğ‘‚­
            Debug.Log($"w“ü{drinkDate.drinkName}");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //ƒJ[ƒ\ƒ‹ƒƒbƒN‰ğœˆ—‚È‚Ç‚ğ•`‚­
    }
}
