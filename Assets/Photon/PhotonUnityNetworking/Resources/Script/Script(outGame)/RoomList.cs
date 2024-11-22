using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomList : MonoBehaviour
{
    [SerializeField] Text label;        //ルーム名を表示するためのUIテキスト 
    RoomInfo info;                      //現在のルームの情報を保持するための変数 

    //ルーム情報を設定するメソッド 
    public void SetUp(RoomInfo _info)
    {
        //ルーム名をUIテキストに設定
        info = _info; label.text = _info.Name;
    }
    //ルームリストアイテムがクリックされたときの処理
    public void OnClick()
    {
        //Launcherのインスタンスを使ってルームに参加
        Launcher.Instance.JoinRoom(info);
    }
}
