using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] TMP_InputField playerNameInput;
    [SerializeField] TMP_Text titleWelcomeText;
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

    }
}
