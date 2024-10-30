using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] CharBoxList charBoxList;
    public string charKind = null;
    //���[�������O�ɌĂяo��
    public override void OnConnectedToMaster()
    {
        //"room1"�Ƃ������O�̃��[���ɎQ��(���[�����Ȃ��Ȃ�쐬���Ă���Q��)
        Debug.Log("���[�����o�O");
        PhotonNetwork.JoinOrCreateRoom("room1",new RoomOptions(),TypedLobby.Default);
    }
    //���[��������ɌĂяo��
    public override void OnJoinedRoom()
    {
        Debug.Log("���[�����o��");
        //�L�����L���[����
        if(charKind != null)
        {
            GameObject myChar = PhotonNetwork.Instantiate(charKind, Vector3.zero, Quaternion.identity, 0);
            //�����̂ݑ���\�ɂ���
            PlayerController playerController = myChar.GetComponent<PlayerController>();
            playerController.enabled = true;
        }
    }
}
