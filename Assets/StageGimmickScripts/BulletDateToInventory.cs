using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;  // Photon.Pun�̖��O��Ԃ�ǉ�

[Serializable]
public class BulletDateToInventory : MonoBehaviour, IPointerClickHandler
{
    [Header("�����f�[�^�^�u"), SerializeField] private BulletDate bulletDate = new BulletDate();

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
        Debug.Log($"�w��{bulletDate.drinkName}");

        if (eventData.pointerClick.gameObject.CompareTag(tag))
        {
            // �w������
            Debug.Log($"�w������{eventData.pointerClick}");
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
    //    //�J�[�\�����b�N���������Ȃǂ�`��
    //    Debug.Log("����");
    //    if (collision.gameObject == playerObject && Input.GetKey(KeyCode.F))
    //    {
    //        Cursor.lockState = CursorLockMode.None;
    //        Cursor.visible = true;
    //        playerObject = collision.gameObject;

    //        return;
    //    }
    //}

    ///// <summary>
    ///// �f�o�b�O�p�Ȃ̂ō폜���܂��B
    ///// </summary>
    ///// <param name="collision"></param>
    //private void OnCollisionExit(Collision collision)
    //{
    //    var childPlayerObject = playerObject.transform.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.CompareTag(playerTagObject.tag));
    //    var obj = childPlayerObject.gameObject.GetComponent<PlayerController>().ReturnValue();
    //    if (obj.playerInventory.drinkType[0] == this.gameObject && obj.playerInventory.drinkType != null)
    //    {
    //        Debug.Log(obj.playerInventory.drinkType[0]);
    //        Debug.Log("����");
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
