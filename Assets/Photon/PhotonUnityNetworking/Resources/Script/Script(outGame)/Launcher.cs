using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] TMP_InputField playerNameInput;        //Playerの名前を入れる
    [SerializeField] TMP_Text titleWelcomeText;             //タイトルのメッセージ
    [SerializeField] TMP_InputField roomNameInput;          //作成するルームの名前を入れる
    [SerializeField] Transform roomListContent;             //ルーム検索画面のルーム表示位置
    [SerializeField] Text roomName;                         //ルームの名前を入れる
    [SerializeField] GameObject roomListPrefab;             //参加可能のルームを表示するPrefab
    [SerializeField] Transform playerListContent;           //ルーム画面の参加プレイヤーの表示位置
    [SerializeField] GameObject playerListPrefab;           //参加しているプレイヤーを表示するPrefab
    [SerializeField] GameObject startGameBtn;               //ゲームマスタが押せるゲーム開始ボタン
    [SerializeField] GameObject readyBtn;                   //ゲームマスタ以外が押せる準備完了ボタン
  

    private void Awake()
    {
        //現在のインスタンスが静的プロパティに割り当てられる
        Instance = this;
    }
    private void Start()
    {
        //マスターサーバーに接続
        Debug.Log("マスターサーバーに接続中");
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        //マスターサーバー接続成功後にロビー参加
        Debug.Log("マスターサーバーに接続完了");
        PhotonNetwork.JoinLobby();
        //trueにするとホストがシーンロードするとほかのプレイヤーも自動的にシーン移動
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnJoinedLobby()
    {
        //ロビー参加後に呼び出し
        if (PhotonNetwork.NickName == "")
        {
            //ニックネームがない場合ランダムなニックネームを設定
            //名前入力メニューを開く
            PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString();
            MenuMng.Instance.OpenMenu("name");
        }
        else
        {
            MenuMng.Instance.OpenMenu("title");
        }
        Debug.Log("ロビーに参加しました");
    }
    public void SetName()
    {
        //Playerのニックネーム設定
        string name = playerNameInput.text;
        //nameがからかどうか
        if (!string.IsNullOrEmpty(name))
        {
            PhotonNetwork.NickName = name;
            titleWelcomeText.text = $"Welcome, {name}!";
            //タイトル画面を開く
            MenuMng.Instance.OpenMenu("title");
            //入力フィールドをクリア
            playerNameInput.text = "";
        }
        else
        {
            Debug.Log("名前がないです");
        }
    }
    public void CreatRoom()
    {
        //ルームメニューが入力されている場合、新しいルーム作成
        if(!string.IsNullOrEmpty(roomNameInput.text))
        {
            var roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 6;             //最大参加可能人数
            PhotonNetwork.CreateRoom(roomNameInput.text, roomOptions, TypedLobby.Default);
            MenuMng.Instance.OpenMenu("loading");
            roomNameInput.text = "";
        }
        else
        {
            Debug.Log("ルーム名が入力されていません");
        }
    }
    public override void OnJoinedRoom()
    {
        //ルームに参加するとルームUIを設定
        MenuMng.Instance.OpenMenu("room");
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        //プレイヤーリストを更新
        Player[] players = PhotonNetwork.PlayerList;
        //既存のプレイヤーリストをクリア
        foreach (Transform trans in playerListContent)
        {
            Destroy(trans.gameObject);
        }
        //新しいプレイヤーリストを作成
        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerList>().SetUp(players[i]);
        }
        //マスタークライアントならゲームスタートボタン有効
        //マスタークライアントでなければ準備完了ボタン有効
        RoomBtn();
        //startGameBtn.SetActive(PhotonNetwork.IsMasterClient);

    }

    //ルームのボタン有効処理
    private void RoomBtn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startGameBtn.SetActive(true);
        }
        else
        {
            readyBtn.SetActive(true);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //マスタークライアントが変えあったとき呼び出し
        //スタートボタンの有無を設定
        RoomBtn();
        //startGameBtn.SetActive(PhotonNetwork.IsMasterClient);
    }
    public void LeaveRoom()
    {
        //ルーム退出し、ローディング画面を表示
        PhotonNetwork.LeaveRoom();
        MenuMng.Instance.OpenMenu("loadeing");
    }
    public void JoinRoom(RoomInfo info)
    {
        //指定されたルームに参加
        PhotonNetwork.JoinRoom(info.Name);
        MenuMng.Instance.OpenMenu("title");
    }
    public override void OnLeftRoom()
    {
        //Playerがroomを退出したとき
        MenuMng.Instance.OpenMenu("title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //ロビーのルームリストが更新んされたときに呼び出し
        //リスト更新
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count(); i++)
        {
            if (roomList[i].RemovedFromList)
            {
                // Don't instantiate stale rooms
                continue;
            }
            Instantiate(roomListPrefab, roomListContent).GetComponent<RoomList>().SetUp(roomList[i]);
        }
        //TODO (3)_キャラ生成
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //TODO (4)_ルーム作成に失敗したとき
        //errorText.text = "Room Creation Failed: " + message;
        //MenuManager.Instance.OpenMenu("error");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerList>().SetUp(newPlayer); 
    }
    public void StartGame()
    {
        //ゲーム開始
        //マスタークライアントがゲームを開始
        //全員がゲームシーンへ移動
        //PhotonNetwork.LoadLevel("3_GameScene");
    }
    public void QuitGame()
    {
        //ゲーム終了
        Application.Quit();
    }
}
