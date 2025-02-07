using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Unity.VisualScripting;
using System.Collections;

[Serializable]
public class BulletDateToInventory : MonoBehaviour, IPointerClickHandler
{
    [Header("飲料データタブ"), SerializeField] public BulletDate bulletDate = new BulletDate();
    [Header("仮の購入クールタイム(MF仕様)"), SerializeField] private int time = 1;
    private bool isBuy;

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((gameObject.GetComponentInParent<VendingMachineController>() == null)) { return; }
        if (gameObject.GetComponentInParent<VendingMachineController>().player == null) { return; }
        int charaIndex = gameObject.GetComponentInParent<VendingMachineController>().player.CharacterIndex;

        if (
            (int)bulletDate.gunType
            == 1
            && ParameterDate.SendCharacterCoin(charaIndex) >= bulletDate.drinkCost
            && !isBuy
            )
        {
            StartCoroutine(Buy(charaIndex));
        }
         else if (
                  eventData.pointerClick.gameObject.CompareTag(tag)
                  && ParameterDate.SendCharacterCoin(charaIndex) >= bulletDate.drinkCost
                  && !isBuy
                 )
        {
            StartCoroutine(Buy(charaIndex));
        }
    }

    private IEnumerator Buy(int characterIndex)
    {
        SetDateManager.AddCoin(characterIndex, -bulletDate.drinkCost);
        SetDateManager.AddTotalAmmo(characterIndex, bulletDate.drinkCapacity);
        SetDateManager.SetHP(characterIndex, bulletDate.drinkHeal);
        yield return new WaitForSeconds(time);
        isBuy = false;
    }
}
