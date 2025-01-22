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

    [SerializeField] private TMP_Text titleWelcomeText;             //タイトルのメッセージ
    [SerializeField] private TMP_InputField roomNameInput;          //作成するルームの名前を入れる
    [SerializeField] private Transform roomListContent;             //ルーム検索画面のルーム表示位置
    [SerializeField] private Text roomName;                         //ルームの名前を入れる
    [SerializeField] private GameObject roomListPrefab;             //参加可能のルームを表示するPrefab
    [SerializeField] private GameObject startGameBtn;               //ゲームマスタが押せるゲーム開始ボタン
    [SerializeField] private Text errorMsg;                         //エラーメッセージ格納
    [SerializeField] private GameObject error;                      //エラー表示画面
    [SerializeField] private GameObject errorSever;                 //サーバ接続ミスエラー表示画面

    private void Awake()
    {
        //現在のインスタンスが静的プロパティに割り当てられる
        Instance = this;
    }
    private void Start()
    {
        ConnectToMasterServer();
    }
    private void ConnectToMasterServer()
    {
        //マスターサーバーに接続
        try
        {
            errorSever.SetActive(false); 
            Debug.Log("マスターサーバーに接続中");
            PhotonNetwork.ConnectUsingSettings();
        }
        catch
        {
            ErrorSeverHandling("マスターサーバーの接続に失敗しました");
        }
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
        MenuMng.Instance.OpenMenu("home");
        Debug.Log("ロビーに参加しました");
    }
    public void CreatRoom()
    {
        //ルームメニューが入力されている場合、新しいルーム作成
        if(!string.IsNullOrEmpty(roomNameInput.text))
        {
            try
            {
                var roomOptions = new RoomOptions();
                roomOptions.MaxPlayers = 6;             //最大参加可能人数
                PhotonNetwork.CreateRoom(roomNameInput.text, roomOptions, TypedLobby.Default);
                MenuMng.Instance.OpenMenu("loadeing");
                roomNameInput.text = "";
            }
            catch
            {
                ErrorHandling("ルームの作成に失敗しました");
            }
        }
        else
        {
            Debug.Log("ルーム名が入力されていません");
        }
    }
    public override void OnJoinedRoom()
    {
        //ルームに参加するとルームUIを設定
        MenuMng.Instance.OpenMenu("name");
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //マスタークライアントが変えあったとき呼び出し
        //スタートボタンの有無を設定
        startGameBtn.SetActive(PhotonNetwork.IsMasterClient);
    }
    public void JoinRoom(RoomInfo info)
    {
        //指定されたルームに参加
        try
        {
            PhotonNetwork.JoinRoom(info.Name);
            MenuMng.Instance.OpenMenu("loading");
        }
        catch
        {
            ErrorHandling("ルーム参加中にエラーが発生しました");
        }
    }
    public override void OnLeftRoom()
    {
        //Playerがroomを退出したとき
        MenuMng.Instance.OpenMenu("home");
    }
    public void LeaveRoom()
    {
        //ルーム退出し、ローディング画面を表示
        try
        {
            PhotonNetwork.LeaveRoom();
            MenuMng.Instance.OpenMenu("loadeing");
        }
        catch
        {
            ErrorHandling("ルームの退出に失敗しました");
        }
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
                continue;
            }
            Instantiate(roomListPrefab, roomListContent).GetComponent<RoomList>().SetUp(roomList[i]);
        }
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        ErrorHandling("ルームの作成に失敗しました");
    }
    public void QuitGame()
    {
        //ゲーム終了
        Application.Quit();
    }
    //エラー処理
    //エラー画面表示後にボタンを押すとタイトル画面へ強制遷移
    private void ErrorHandling(string msg)
    {
        errorMsg.text = msg + "\n" + "ホーム画面へ移動します";
        error.SetActive(true);
    }
    public void HomeTransition()
    {
        MenuMng.Instance.OpenMenu("home");
        error.SetActive(false);
    }
    private void ErrorSeverHandling(string msg)
    {
        errorMsg.text = msg;
        errorSever.SetActive(true);
    }
    public void Reconnect()
    {
        ConnectToMasterServer();
    }
}