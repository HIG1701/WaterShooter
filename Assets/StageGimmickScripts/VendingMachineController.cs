using Photon.Pun;
using System.Collections;
using UnityEngine;

public class VendingMachineController : MonoBehaviour
{
    [Header("仮の値段(MF仕様)"), SerializeField] private int num = 100;
    [Header("仮の弾数(MF仕様)"), SerializeField] private int ammo = 120;
    [Header("仮の購入クールタイム(MF仕様)"), SerializeField] private int time = 1;
    private bool isBuy;

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController))

        if (!PhotonNetwork.IsConnected || GetComponent<PhotonView>().IsMine && playerController)
        {
            Debug.Log("解除a");
            if (Input.GetKey(KeyCode.F))
            {
                //Cursor.lockState = CursorLockMode.None;
                //Cursor.visible = true;
            }
            else { return; }

            if (ParameterDate.SendCharacterCoin(playerController.CharacterIndex) >= num && !isBuy)
            {
                isBuy = true;
                StartCoroutine(Buy(playerController));
            }
        }
    }

    private IEnumerator Buy(PlayerController playerController)
    {
        SetDateManager.AddCoin(playerController.CharacterIndex, -num);
        SetDateManager.AddTotalAmmo(playerController.CharacterIndex, ammo);
        yield return new WaitForSeconds(time);
        isBuy = false;
    }

    private void OnTriggerExit(Collider collision)
    {
        if (!PhotonNetwork.IsConnected || GetComponent<PhotonView>().IsMine)
        {
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }
    }
}
