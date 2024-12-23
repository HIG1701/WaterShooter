using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectChar : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text msgText;

    [SerializeField] private Button confirmButton;      //準備完了ボタン
    [SerializeField] private Button backButton;         //選択解除ボタン
    [SerializeField] private Button startGameButton;    //ゲーム開始ボタン
    [SerializeField] private Button exitBtn;            //退出ボタン

    [SerializeField] private Button[] charBtn;          //キャラ選択ボタン
    [SerializeField] private bool[] isCharSelect;       //キャラが選択されているか

    private bool initialize = false;

    private void Awake()
    {
        if (isCharSelect == null || isCharSelect.Length == 0)
        {
            Debug.LogError("isCharSelectが初期化されていません。");
        }

        if (charBtn == null || charBtn.Length == 0)
        {
            Debug.LogError("charBtnが初期化されていません。");
        }
    }

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
            if (PhotonNetwork.IsMasterClient)
            {
                startGameButton.interactable = false;
            }
            else
            {
                startGameButton.gameObject.SetActive(false);
            }
            //charBtn[i].onClick.AddListener(() => OnCharSelect(idx));
            //既存のキャラクター選択状態を同期
            //UpdateCharSelectFromProperties();
        }
        initialize = true;
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
        photonView.RPC("CheckGameStartAvailability", RpcTarget.AllBuffered);
    }

    //RPCメソッド：キャラクタが選択されたことを全プレイヤーに通知
    //Photonネットワークを通じてリモートで呼び出し
    // idx      : 選択されたキャラクターのインデックス
    // playerID : プレイヤーのID
    [PunRPC]
    public void CharSelect(int idx, int playerID)
    {
        if (!initialize)
        {
            Debug.LogWarning("初期化が完了していないため、RPCをスキップします");
            return;
        }
        if (isCharSelect == null || charBtn == null || idx < 0 || idx >= isCharSelect.Length)
        {
            Debug.LogError($"不正なデータまたはインデックス: {idx}");
            return;
        }
        if (!isCharSelect[idx])
        {
            //選択されたキャラクターのボタンを無効化
            isCharSelect[idx] = true;
            charBtn[idx].interactable = false;
            EnableStartGameButton();
        }
    }

    //選択解除の状態を全プレイヤーに通知
    [PunRPC]
    public void UpdateCharSelectState(int idx, PhotonMessageInfo info)
    {
        if (isCharSelect == null || charBtn == null)
        {
            Debug.LogError("isCharSelect または charBtn が初期化されていません");
            return;
        }
        if (idx >= 0 && idx < isCharSelect.Length)
        {
            isCharSelect[idx] = true;
            charBtn[idx].interactable = false;
            Debug.Log($"Player {info.Sender.NickName} selected character {idx}");
        }
        else if (idx == -1)
        {
            //全プレイヤー状態を再チェック
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties.TryGetValue("CharSelect", out object charIdxObj))
                {
                    int charIdx = (int)charIdxObj;
                    if (charIdx >= 0 && charIdx < isCharSelect.Length)
                    {
                        isCharSelect[charIdx] = true;
                        charBtn[charIdx].interactable = false;
                    }
                }
            }
        }
        else
        {
            Debug.LogError($"無効なインデックス: {idx}");
        }
    }

    //選択解除後に選択されていたキャラを再度有効化
    [PunRPC]
    public void ResetCharSelectionForAll(int idx)
    {
        if (idx >= 0 && idx < isCharSelect.Length)
        {
            //選択状態を解除
            isCharSelect[idx] = false;
            //対応するボタンを再有効化
            charBtn[idx].interactable = confirmButton.interactable ? true : false;
        }
        else
        {
            //エラー処理
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
        //ゲーム開始ボタンが有効かどうかのチェック
        EnableStartGameButton();
        DisplaySelectedCharacters();
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

            //他のプレイヤーと同期
            photonView.RPC("ResetCharSelectionForAll", RpcTarget.AllBuffered, selectedIdx);
            
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
        photonView.RPC("CheckGameStartAvailability", RpcTarget.AllBuffered);
        EnableStartGameButton();
    }
    //すべてのプレイヤーがキャラクターを選択しているか確認
    [PunRPC]
    public bool CheckGameStartAvailability()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("CharSelect") || (int)player.CustomProperties["CharSelect"] == -1)
            {
                //未選択のプレイヤーがいる
                return false;
            }
        }
        //すべてのプレイヤーが選択済み
        return true;
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
            //キャラクターが選択されていない場合のデフォルト値
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

    //ゲーム開始ボタンを有効化
    private void EnableStartGameButton()
    {
        if (!PhotonNetwork.IsMasterClient)  return;
        //ボタンクリック可能にする
        //onClick.AddListenerメソッドを使うことで特定の処理を実行
        if (CheckGameStartAvailability() && PhotonNetwork.CurrentRoom.PlayerCount != 1)
        {
            startGameButton.onClick.RemoveAllListeners();
            startGameButton.onClick.AddListener(StartGame);
            startGameButton.interactable = true;
        }
        else
        {
            startGameButton.interactable = false;
        }
    }

    //カスタムプロパティからキャラクター選択状態を更新するメソッド
    private void UpdateCharSelectFromProperties()
    {
        if (charBtn == null || isCharSelect == null)
        {
            Debug.LogError("isCharSelect または charBtn が初期化されていません");
            return;
        }
        if (PhotonNetwork.PlayerList == null || PhotonNetwork.PlayerList.Length == 0)
        {
            return;
        }
        //初期化
        foreach (Button btn in charBtn)
        {
            btn.interactable = true;
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

    //プレイヤー名と選択キャラクターのマッピングを取得する
    public void DisplaySelectedCharacters()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            //TryGetValue("CharSelect", out object charIdxObj) は、指定したキー "CharSelect" が存在するかどうかを確認
            //ある場合はその値を charIdxObj に格納
            if (player.CustomProperties.TryGetValue("CharSelect", out object charIdxObj))
            {
                int charIdx = (int)charIdxObj;
                Debug.Log($"{player.NickName} がキャラクター {charIdx} を選択しました。");
                UpdateRoomCharacterSelection(charIdx, player.NickName);
            }
            else
            {
                Debug.Log($"{player.NickName} はまだキャラクターを選択していません。");
            }
        }
    }

    //キャラクターが選択された際に、部屋のカスタムプロパティに状態を保存
    private void UpdateRoomCharacterSelection(int idx, string playerName)
    {
        //ExitGames.Client.Photon.Hashtable
        //      Photonのルームやプレイヤーごとのカスタムプロパティを格納するためのデータ構造
        ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable();

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("CharSelection", out object charSelectionObj))
        {
            //既存の状態を取得
            //Dictionary<string, int> の構造
            //  キー (string)： プレイヤーの名前（NickName とか）を表す
            //  値(int)      ： 選択されたキャラクターのインデックスを表す
            //(Dictionary<string, int>)charSelectionObj
            //  charSelectionObj をキャストして Dictionary<string, int> 型として扱えるようにする
            Dictionary<string, int> charSelection = (Dictionary<string, int>)charSelectionObj;
            //プレイヤーの選択を更新
            charSelection[playerName] = idx; 
            roomProperties["CharSelection"] = charSelection;
        }
        else
        {
            //初めての選択
            Dictionary<string, int> charSelection = new Dictionary<string, int>
            {
                { playerName, idx }
            };
            roomProperties["CharSelection"] = charSelection;
        }
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
    }

    //部屋全体の選択状態を確認
    private void DisplayRoomCharacterSelections()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("CharSelection", out object charSelectionObj))
        {
            Dictionary<string, int> charSelection = (Dictionary<string, int>)charSelectionObj;

            foreach (var entry in charSelection)
            {
                Debug.Log($"{entry.Key} がキャラ{entry.Value} を選択しました。");
            }
        }
        else
        {
            Debug.Log("まだキャラクター選択状態はありません。");
        }
    }

    //プレイヤーが参加してきた時に呼び出し
    public override void OnJoinedRoom()
    {
        Debug.Log("部屋に参加しました");
        StartCoroutine(DelayedSync());
    }
    //RPC 呼び出しのタイミングを遅らせる
    private IEnumerator DelayedSync()
    {
        yield return new WaitForSeconds(0.2f); //短い遅延を設ける
        UpdateCharSelectFromProperties();
    }

    //プレイヤーが部屋に入ったタイミングで、初期化されているか確認
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"新しいプレイヤーが参加しました: {newPlayer.NickName}");
        EnableStartGameButton();
        CheckGameStartAvailability();
        if (PhotonNetwork.IsMasterClient)
        {
            //新しいプレイヤーに既存のキャラ選択状態を同期
            StartCoroutine(SyncWithNewPlayer(newPlayer));
        }
    }
    private IEnumerator SyncWithNewPlayer(Player newPlayer)
    {
        yield return new WaitForSeconds(0.2f); //少し遅延を設ける
        photonView.RPC("SyncCharSelectState", newPlayer, PhotonNetwork.LocalPlayer.ActorNumber);
    }

    //レイヤーが新規に部屋に参加したときに、既存プレイヤーが選択したキャラクターの状態を新しいプレイヤーに同期
    [PunRPC]
    public void SyncCharSelectState(int requestingPlayerID)
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue("CharSelect", out object charIdxObj))
            {
                int charIdx = (int)charIdxObj;
                if (charIdx >= 0 && charIdx < isCharSelect.Length)
                {
                    // 全プレイヤーにキャラ選択情報をブロードキャスト
                    photonView.RPC("UpdateCharSelectState", RpcTarget.AllBuffered, charIdx);
                }
            }
        }
    }

    //プレイヤー退出時に呼び出し
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        EnableStartGameButton();
        CheckGameStartAvailability();
        int selectedIdx = GetSelectedCharIndex(otherPlayer);
        if (selectedIdx >= 0 && selectedIdx < isCharSelect.Length)
        {
            // 選択状態をリセット
            isCharSelect[selectedIdx] = false;
            charBtn[selectedIdx].interactable = true;

            // 全プレイヤーに更新を通知
            photonView.RPC("UpdateCharSelectState", RpcTarget.AllBuffered, -1);
        }
    }

    //ゲームシーンへ移行
    public void StartGame()
    {
        Debug.Log("ゲーム画面へ移行");
        DisplayRoomCharacterSelections();
        //部屋にいるキャラを全員ゲーム画面に移行
        PhotonNetwork.LoadLevel("3_GameScene");
    }
}