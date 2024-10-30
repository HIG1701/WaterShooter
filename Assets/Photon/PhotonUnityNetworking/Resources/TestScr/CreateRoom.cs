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
    //ルーム入室前に呼び出し
    public override void OnConnectedToMaster()
    {
        //"room1"という名前のルームに参加(ルームがないなら作成してから参加)
        Debug.Log("ルーム入出前");
        PhotonNetwork.JoinOrCreateRoom("room1",new RoomOptions(),TypedLobby.Default);
    }
    //ルーム入室後に呼び出し
    public override void OnJoinedRoom()
    {
        Debug.Log("ルーム入出後");
        //キャラキラー生成
        GameObject myChar = PhotonNetwork.Instantiate(charBoxList.setCharName, 
            new Vector3(transform.position.x,transform.position.y + 5f), 
            Quaternion.identity, 0);
        //自分のみ操作可能にする
        PlayerController playerController = myChar.GetComponent<PlayerController>();
        playerController.enabled = true;
    }
}
