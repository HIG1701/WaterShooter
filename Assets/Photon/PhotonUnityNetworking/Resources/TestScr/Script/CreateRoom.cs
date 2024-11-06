using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoom : MonoBehaviourPunCallbacks
{

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    //ルーム入室前に呼び出し
    public override void OnConnectedToMaster()
    {
        //"room1"という名前のルームに参加(ルームがないなら作成してから参加)
        Debug.Log("ルーム入出前");
        //ルームのオプション設定
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 6;             //最大参加可能人数

        PhotonNetwork.JoinOrCreateRoom("room1", roomOptions, TypedLobby.Default);
    }
    //ルーム入室後に呼び出し
    public override void OnJoinedRoom()
    {
        Debug.Log("ルーム入出後");
        //キャラキラー生成
    }

}
