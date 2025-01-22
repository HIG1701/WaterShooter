using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;  // Photon.Punの名前空間を追加

[Serializable]
public class BulletDateToInventory : MonoBehaviour, IPointerClickHandler
{
    [Header("飲料データタブ"), SerializeField] private BulletDate bulletDate = new BulletDate();

    public BulletDate readonlyBulletData => bulletDate;

    private GameObject playerObject;

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            playerObject = PhotonNetwork.LocalPlayer.TagObject as GameObject;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var childPlayerObject = playerObject.transform.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.CompareTag("Player"));
        Debug.Log($"購入{bulletDate.drinkName}");

        if (eventData.pointerClick.gameObject.CompareTag(tag))
        {
            // 購入処理
            Debug.Log($"購入完了{eventData.pointerClick}");
            Debug.Log(childPlayerObject);
            if (childPlayerObject != null)
            {
                childPlayerObject.gameObject.GetComponent<PlayerController>().DrinkInInventory(eventData.pointerClick.gameObject);
            }
        }
    }
}


    //private void OnCollisionStay(Collision collision)
    //{
    //    //カーソルロック解除処理などを描く
    //    Debug.Log("解除");
    //    if (collision.gameObject == playerObject && Input.GetKey(KeyCode.F))
    //    {
    //        Cursor.lockState = CursorLockMode.None;
    //        Cursor.visible = true;
    //        playerObject = collision.gameObject;

    //        return;
    //    }
    //}

    ///// <summary>
    ///// デバッグ用なので削除します。
    ///// </summary>
    ///// <param name="collision"></param>
    //private void OnCollisionExit(Collision collision)
    //{
    //    var childPlayerObject = playerObject.transform.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.CompareTag(playerTagObject.tag));
    //    var obj = childPlayerObject.gameObject.GetComponent<PlayerController>().ReturnValue();
    //    if (obj.playerInventory.drinkType[0] == this.gameObject && obj.playerInventory.drinkType != null)
    //    {
    //        Debug.Log(obj.playerInventory.drinkType[0]);
    //        Debug.Log("成功");
    //    }

    //    if (collision.gameObject == playerObject)
    //    {
    //        Cursor.lockState = CursorLockMode.None;
    //        Cursor.visible = true;
    //        playerObject = collision.gameObject;

    //        return;
    //    }
    //}
//}
