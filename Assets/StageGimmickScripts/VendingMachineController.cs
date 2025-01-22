using Photon.Pun;
using UnityEngine;

public class VendingMachineController : MonoBehaviour
{
    private void OnTriggerStay(Collider collision)
    {
        if (!PhotonNetwork.IsConnected || GetComponent<PhotonView>().IsMine)
        {
            Debug.Log("âèúa");
            if (Input.GetKey(KeyCode.F))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (!PhotonNetwork.IsConnected || GetComponent<PhotonView>().IsMine)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
