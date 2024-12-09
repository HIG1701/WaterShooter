using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class SelectChar : MonoBehaviourPunCallbacks
{
    [SerializeField] Text msgText;

    [SerializeField] Button confirmButton;
    [SerializeField] Button backButton;
    [SerializeField] Button startGameButton;
    [SerializeField] Button exitBtn;

    [SerializeField] Button[] charBtn;        //キャラ選択ボタン
    private bool[] isCharSelect;      //キャラが選択されているか

    private void Start()
    {
        Debug.Log("初期化処理");
        msgText.text = "キャラを選択してくだい";

        //配列の要素数だけ初期化
        //for分を利用して、各ボタンにクリックリスナーを追加
        isCharSelect = new bool[charBtn.Length];
        for (int i = 0; i < isCharSelect.Length; i++)
        {
            int idx = i;
            if (charBtn[i] != null)
            {
                charBtn[i].onClick.AddListener(() => OnCharSelect(idx));
            }
            else
            {
                Debug.LogError("charBtn[" + i + "] is not set.");
            }
            //選択完了ボタンと戻るボタンにリスナーを追加
            confirmButton.onClick.AddListener(OnConfirmSelection);
            backButton.onClick.AddListener(OnBack);
            confirmButton.interactable = false;
            backButton.gameObject.SetActive(false);
            startGameButton.interactable = false;
            //charBtn[i].onClick.AddListener(() => OnCharSelect(idx));
            //既存のキャラクター選択状態を同期
            //UpdateCharSelectFromProperties();
        }
    }

    public void SelectCharInitialize()
    {
        //初期化
        confirmButton.interactable = false;
        backButton.gameObject.SetActive(false);
        foreach (Button btn in charBtn)
        {
            btn.interactable = true;
        }
    }

    //キャラクタ選択時に呼び出し
    public void OnCharSelect(int idx)
    {
        if (isCharSelect == null || charBtn == null) 
        {
            Debug.LogError("charSelect または charBtn が初期化されてない");
            return;
        }
        if (idx < 0 || idx >= isCharSelect.Length) 
        {
            Debug.LogError("Index out of bounds: " + idx); 
            return;
        }
        if (!isCharSelect[idx])
        {
            Debug.Log("キャラクタナンバー" + idx + "を選択");
            msgText.text = "用意ができたら準部完了ボタンをクリック";

            isCharSelect[idx] = true;
            //ボタンを無効
            //選択完了ボタンを有効
            //戻るボタンを表示
            charBtn[idx].interactable = false;    　
            confirmButton.interactable = true;
            exitBtn.interactable = false;
            backButton.gameObject.SetActive(true);
            //キャラクタ選択状態でほかのキャラクタは選べない
            //他のキャラクターボタンを無効化
            foreach (Button btn in charBtn) 
            {
                if (btn != charBtn[idx]) 
                {
                    btn.interactable = false; 
                }
            }
            //キャラクタ選択状態を更新、記録
            //選択されたキャラクターの情報をカスタムプロパティを格納するためのハッシュテーブル作成し保存
            //選択キャラクターのidxをカスタムプロパティに設定
            //プレイヤーのカスタムプロパティとして設定した情報をPhotonネットワーク上に保存
            //RPCメソッドを使って全プレイヤーに選択情報を通知、新たなプレイヤーにも反映されるようにバッファリング
            //isCharSelect[idx] = true;
            ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
            playerProperties["CharSelect"] = idx;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
            photonView.RPC("CharSelect", RpcTarget.AllBuffered, idx, PhotonNetwork.LocalPlayer.ActorNumber);
            photonView.RPC("UpdateCharSelectState", RpcTarget.AllBuffered, idx);
        }
    }
    //RPCメソッド：キャラクタが選択されたことを全プレイヤーに通知
    //Photonネットワークを通じてリモートで呼び出し
    // idx      : 選択されたキャラクターのインデックス
    // playerID : プレイヤーのID
    [PunRPC]
    public void CharSelect(int idx, int playerID)
    {
        if (isCharSelect == null || charBtn == null)
        {
            return;
        }
        if (idx < 0 || idx >= isCharSelect.Length)
        {
            return;
        }
        if (!isCharSelect[idx])
        {
            //選択されたキャラクターのボタンを無効化
            isCharSelect[idx] = true;
            charBtn[idx].interactable = false;
            //ホストかつ全プレイヤーがキャラを選択しているかどうか確認
            //もし選択していたらゲーム開始ボタンを有効化
            if (PhotonNetwork.IsMasterClient && AllPlayerSelect())
            {
                EnableStartGameButton();
            }
        }
    }
    //ほかのプレイヤーからの選択解除情報を受け散るRPCメソッド
    [PunRPC]
    public void ResetCharSelect(int playerID)
    {
        //指定されたプレイヤーの選択解除のみ処理
        if(PhotonNetwork.LocalPlayer.ActorNumber == playerID)
        {
            int selectedIdx = GetSelectedCharIndex(PhotonNetwork.LocalPlayer);
            if (selectedIdx >= 0 && selectedIdx < isCharSelect.Length)
            {
                // キャラクターの選択状態を解除
                isCharSelect[selectedIdx] = false;
                // ボタンを有効化
                charBtn[selectedIdx].interactable = true; 
            }
        }
    }

    //選択解除の状態を全プレイヤーに通知
    [PunRPC]
    public void UpdateCharSelectState(int idx, PhotonMessageInfo info)
    {
        Debug.Log($"Player {info.Sender.NickName} has updated their selection.");
        if (idx >= 0 && idx < isCharSelect.Length)
        {
            isCharSelect[idx] = true;
            charBtn[idx].interactable = false;
        }
        else if (idx == -1)
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties.TryGetValue("CharSelect", out object charIdxObj))
                {
                    int charIdx = (int)charIdxObj;
                    if (charIdx >= 0 && charIdx < isCharSelect.Length)
                    {
                        isCharSelect[charIdx] = false;
                        charBtn[charIdx].interactable = true;
                    }
                }
            }
        }
    }
    //選択完了ボタンを押下後に呼び出し
    public void OnConfirmSelection()
    {
        msgText.text = "準備完了！";

        //選択完了ボタン無効
        //戻るボタン表示
        confirmButton.interactable = false;
        backButton.gameObject.SetActive(true);
        // ゲーム開始ボタンが有効かどうかのチェック
        if (PhotonNetwork.IsMasterClient && AllPlayerSelect())
        {
            EnableStartGameButton();
        }
    }
    //戻るボタン押下後に呼び出し
    public void OnBack()
    {
        //カスタムプロパティから選択されたキャラクターのインデックスを取得
        //自分が選択していたキャラクターのインデックスを取得
        int selectedIdx = GetSelectedCharIndex(PhotonNetwork.LocalPlayer);
        if (selectedIdx >= 0 && selectedIdx < isCharSelect.Length)
        {
            //選択解除
            //解除したキャラクターを再選択可能に
            isCharSelect[selectedIdx] = false;
            charBtn[selectedIdx].interactable = true; 

            //自分のカスタムプロパティをリセット
            ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable { ["CharSelect"] = -1 };
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

            //UIをリセット
            confirmButton.interactable = false;
            backButton.gameObject.SetActive(false);
            exitBtn.interactable = true;
            foreach (Button btn in charBtn)
            {
                int idx = System.Array.IndexOf(charBtn, btn);
                //未選択キャラを有効化
                btn.interactable = !isCharSelect[idx];
            }

            //メッセージ更新
            msgText.text = "キャラを選択してください";
        }
        //マスタークライアントの場合、全プレイヤーが選択しているか確認
        if (PhotonNetwork.IsMasterClient && !AllPlayerSelect())
        {
            startGameButton.interactable = false;
        }
    }

    //カスタムプロパティからプレイヤーが選んだキャラクターのインデックスを取得する
    public int GetSelectedCharIndex(Player player)
    {
        if (player.CustomProperties.TryGetValue("CharSelect", out object charIndex)) 
        {
            return (int)charIndex;
        }
        else
        { 
            // キャラクターが選択されていない場合のデフォルト値
            return -1; 
        }
    }
    //全プレーヤーがキャラクタを選択したかどうか
    private bool AllPlayerSelect()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            msgText.text = "参加人数が１の場合は開始できません";
            //参加人数が一人の場合ゲーム開始はできない
            return false;
        }
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("CharSelect"))
            {
                //いずれかのPlayerが未選択
                return false;
            }
        }
        return true;
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
    public void StartGame() 
    {
        Debug.Log("ゲーム画面へ移行");
    }
    //カスタムプロパティからキャラクター選択状態を更新するメソッド
    private void UpdateCharSelectFromProperties()
    {
        if (charBtn == null || isCharSelect == null)
        {
            return;
        }
        if (PhotonNetwork.PlayerList == null || PhotonNetwork.PlayerList.Length == 0)
        {
            return;
        }
        //初期化
        for (int i = 0; i < isCharSelect.Length; i++)
        {
            isCharSelect[i] = false;
            //一度すべてのボタンを有効化
            charBtn[i].interactable = true;
        }
        //各プレイヤーの選択状態を確認し、選択済みのキャラクターのボタンを無効化
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue("CharSelect", out object charIdxObj))
            {
                int charIdx = (int)charIdxObj;

                if (charIdx >= 0 && charIdx < isCharSelect.Length)
                {
                    isCharSelect[charIdx] = true;
                    //選択済みのキャラを無効化
                    charBtn[charIdx].interactable = false;
                }
            }
            
        }
        //自分が選択していたキャラクターを再び選択可能に設定
        int myCharIdx = GetSelectedCharIndex(PhotonNetwork.LocalPlayer);
        if (myCharIdx >= 0 && myCharIdx < isCharSelect.Length)
        {
            //再選択を許可
            charBtn[myCharIdx].interactable = true; //再選択を許可
        }
    }
    //プレイヤーが部屋に入ったタイミングで、初期化されているか確認
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        if (!newPlayer.CustomProperties.ContainsKey("CharSelect"))
        {
            //未選択状態を示す
            properties["CharSelect"] = -1; 
            newPlayer.SetCustomProperties(properties);
        }
    }
    //プレイヤーが参加してきた時に呼び出し
    public override void OnJoinedRoom()
    {
        UpdateCharSelectFromProperties();
    }
}