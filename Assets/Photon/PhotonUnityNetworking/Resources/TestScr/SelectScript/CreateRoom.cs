using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoom : MonoBehaviourPunCallbacks
{

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    //���[�������O�ɌĂяo��
    public override void OnConnectedToMaster()
    {
        //"room1"�Ƃ������O�̃��[���ɎQ��(���[�����Ȃ��Ȃ�쐬���Ă���Q��)
        Debug.Log("���[�����o�O");
        //���[���̃I�v�V�����ݒ�
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 6;             //�ő�Q���\�l��

        PhotonNetwork.JoinOrCreateRoom("room1", roomOptions, TypedLobby.Default);
    }
    //���[��������ɌĂяo��
    public override void OnJoinedRoom()
    {
        Debug.Log("���[�����o��");
        //�L�����L���[����
    }

}
