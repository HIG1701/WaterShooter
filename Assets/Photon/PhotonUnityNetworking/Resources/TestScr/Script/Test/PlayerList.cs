using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerList : MonoBehaviourPunCallbacks
{
    [SerializeField] Text text;             //プレイヤー名を表示するためのUIテキスト
    Player player;                          //現在のプレイヤー情報を保持するための変数
    public void SetUp(Player _player)
    {
        player = _player;
        //プレイヤーのニックネームをUIテキストに設定 
        text.text = _player.NickName;
    }
    //ルーム内の他のプレイヤーが退出したときの処理
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            //退出したプレイヤーのリストアイテムを破棄
            Destroy(gameObject);
        }
    }
    //自身がルームを退出したときの処理
    public override void OnLeftRoom()
    {
        //自身のリストアイテムを破棄
        Destroy(gameObject);
    }
}
