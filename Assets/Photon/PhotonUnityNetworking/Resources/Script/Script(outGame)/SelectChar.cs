using Photon.Pun;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SelectChar : MonoBehaviourPunCallbacks
{
    public Button startGameButton;
    public Button[] charBtn;        //キャラ選択ボタン
    private bool[] charSelect;      //キャラが選択されているか
    private int maxplyer = 6;       //最大PlayerはPlayer数

    private void Start()
    {
        //配列の要素数だけ初期化
        //for分を利用して、各ボタンにクリックリスナーを追加
        charSelect = new bool[charBtn.Length];
        for (int i = 0; i < charSelect.Length; i++)
        {
            int idx = i;
            charBtn[i].onClick.AddListener(() => OnCharSelect(idx));
        }
    }
    //キャラクタ選択時に呼び出し
    public void OnCharSelect(int idx)
    {
        if (!charSelect[idx])
        {
            //キャラクタ選択状態を更新、記録
            //選択されたキャラクターの情報をカスタムプロパティを格納するためのハッシュテーブル作成し保存
            //選択キャラクターのidxをカスタムプロパティに設定
            //プレイヤーのカスタムプロパティとして設定した情報をPhotonネットワーク上に保存
            //RPCメソッドを使って全プレイヤーに選択情報を通知、新たなプレイヤーにも反映されるようにバッファリング
            charSelect[idx] = true;
            ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
            playerProperties["CharSelect"] = idx;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
            //photonView.RPC("CharSelect", RpcTarget.AllBuffered, idx, PhotonNetwork.LocalPlayer.ActorNumber);
            Debug.Log(RpcTarget.AllBuffered);
            Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }
    //RPCメソッド：キャラクタが選択されたことを全プレイヤーに通知
    //Photonネットワークを通じてリモートで呼び出し
    // idx      : 選択されたキャラクターのインデックス
    // playerID : プレイヤーのID
    [PunRPC]
    public void CharSelect(int idx, int playerID)
    {
        if (!charSelect[idx])
        {
            //選択されたキャラクターのボタンを無効化
            charSelect[idx] = true;
            charBtn[idx].interactable = false;
            //ホストかつ全プレイヤーがキャラを選択しているかどうか確認
            //もし選択していたらゲーム開始ボタンを有効化
            if (PhotonNetwork.IsMasterClient && AllPlayerSelect())
            {
                EnableStartGameButton();
            }
        }
    }

    //全プレーヤーがキャラクタを選択したかどうか
    private bool AllPlayerSelect()
    {
        return PhotonNetwork.PlayerList.Length == maxplyer && charSelect.All(selected => selected);
    }
    // ゲーム開始ボタンを有効化
    private void EnableStartGameButton()
    {
        //ボタンクリック可能にする
        //onClick.AddListenerメソッドを使うことで特定の処理を実行
        startGameButton.interactable = true;
        startGameButton.onClick.AddListener(StartGame);
    }
    // ゲームシーンへ移行
    private void StartGame() 
    {
        Debug.Log("ゲーム画面へ移行");
    }
}
