using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] CharBoxList charBoxList;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
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
        GameObject myChar = PhotonNetwork.Instantiate(charBoxList.setCharName, 
            new Vector3(transform.position.x,transform.position.y + 5f), 
            Quaternion.identity, 0);
        //�����̂ݑ���\�ɂ���
        PlayerController playerController = myChar.GetComponent<PlayerController>();
        playerController.enabled = true;
    }
}
