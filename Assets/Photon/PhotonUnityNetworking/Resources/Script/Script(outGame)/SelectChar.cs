using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class SelectChar : MonoBehaviourPunCallbacks
{
    public Button confirmButton;
    public Button backButton;
    public Button startGameButton;
    public Button[] charBtn;        //キャラ選択ボタン
    private bool[] charSelect;      //キャラが選択されているか

    private void Start()
    {
        //配列の要素数だけ初期化
        //for分を利用して、各ボタンにクリックリスナーを追加
        charSelect = new bool[charBtn.Length];
        for (int i = 0; i < charSelect.Length; i++)
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
            //charBtn[i].onClick.AddListener(() => OnCharSelect(idx));
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
        if (charSelect == null || charBtn == null) 
        {
            Debug.LogError("charSelect or charBtn is not initialized.");
            return;
        }
        if (idx < 0 || idx >= charSelect.Length) 
        {
            Debug.LogError("Index out of bounds: " + idx); 
            return;
        }
        if (!charSelect[idx])
        {
            charSelect[idx] = true;
            //ボタンを無効
            //選択完了ボタンを有効
            //戻るボタンを表示
            charBtn[idx].interactable = false;    　
            confirmButton.interactable = true;    　
            backButton.gameObject.SetActive(true);
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
            charSelect[idx] = true;
            ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
            playerProperties["CharSelect"] = idx;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
            photonView.RPC("CharSelect", RpcTarget.AllBuffered, idx, PhotonNetwork.LocalPlayer.ActorNumber);
            //Debug.Log(RpcTarget.AllBuffered);
            //Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }
    //RPCメソッド：キャラクタが選択されたことを全プレイヤーに通知
    //Photonネットワークを通じてリモートで呼び出し
    // idx      : 選択されたキャラクターのインデックス
    // playerID : プレイヤーのID
    [PunRPC]
    public void CharSelect(int idx, int playerID)
    {
        if (charSelect == null || charBtn == null)
        {
            Debug.LogError("charSelect or charBtn is not initialized.");
            return;
        }
        if (idx < 0 || idx >= charSelect.Length)
        {
            Debug.LogError("Index out of bounds: " + idx);
            return;
        }
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
    //選択完了ボタンを押下後に呼び出し
    public void OnConfirmSelection()
    {
        //選択完了ボタン無効
        //戻るボタン非表示
        confirmButton.interactable = false;
        backButton.gameObject.SetActive(true);
        // ゲーム開始ボタンが有効かどうかのチェック
        if (PhotonNetwork.IsMasterClient && AllPlayerSelect())
        {
            EnableStartGameButton();
        }
    }
    //戻るボタン押下後に呼び第s
    public void OnBack()
    {
        //カスタムプロパティから選択されたキャラクターのインデックスを取得
        int selectedIdx = (int)PhotonNetwork.LocalPlayer.CustomProperties["CharSelect"];
        if (selectedIdx >= 0 && selectedIdx < charSelect.Length)
        {
            //キャラ選択解除
            //キャラクタボタン有効
            //選択完了ボタン有効
            //戻るボタン非表示
            charSelect[selectedIdx] = false;
            charBtn[selectedIdx].interactable = true;
            confirmButton.interactable = false;
            backButton.gameObject.SetActive(false);
            //他のキャラクターボタンを再度有効化
            foreach (Button btn in charBtn)
            {
                btn.interactable = true;
            }
            //選択を解除したことをカスタムプロパティから削除
            ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
            playerProperties["CharSelect"] = -1;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        }
    }
    //全プレーヤーがキャラクタを選択したかどうか
    private bool AllPlayerSelect()
    {
        foreach (var selected in charSelect) 
        { 
            if (!selected) return false; 
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
    private void StartGame() 
    {
        Debug.Log("ゲーム画面へ移行");
    }
}