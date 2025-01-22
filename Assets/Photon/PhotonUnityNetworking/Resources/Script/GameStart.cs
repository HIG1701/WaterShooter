using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Photon.Realtime;

public class GameStart : MonoBehaviour
{
    [SerializeField] private ShuffleSpawn shuffleSpawn; //スポーンのシャッフルを行う
    [SerializeField] private CharBoxList charBoxList;   //キャラとスポーン位置格納リスt    
    [SerializeField] private Camera respawnCamera;      //リスポーン中に使用するカメラ
    
    private void Start()
    {
        RetrievePlayerChar();
    }
    //プレイヤーを探す
    private void RetrievePlayerChar()
    {
        //自分のキャラクタのみ選択：LocalPlayer
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("CharSelect", out object charIndexObj))
        {
            Debug.Log("確認 charIdx：" + (int)charIndexObj);
            int charIdx = (int)charIndexObj;
            ApplyCharToGame(charIdx);
        }
        else　{ Debug.LogWarning("キャラクタが選択されていません"); }
    }
    //選んだキャラを設定していく
    private void ApplyCharToGame(int charIdx)
    {
        //スポーン地点の数だけインデックスを作成
        List<int> SpawnIndices = new List<int>();
        for (int i = 0; i < charBoxList.posBox.Count; i++)　{ SpawnIndices.Add(i); }

        //シャッフルメソッドを呼び出し、シャッフル
        
        //ShuffleIndex(SpawnIndices);
        int SpawnIndex = shuffleSpawn.ShuffleSpawnIdx()[charIdx];

        //キャラの選択状況確認
        //PhotonNetwork.CurrentRoom.CustomProperties.:現在の部屋に関連付けられているカスタムプロパティを取得
        //TryGetValue("CharSelection", out object charSelectionObj):部屋のカスタムプロパティに "CharSelection" というキーが存在するかを確認
        //      あったら true 無かったら false
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("CharSelection", out object charSelectionObj))
        {
            Debug.Log("部屋のカスタムプロパティを確認中...");
            //キー: プレイヤーのニックネーム（string）
            //値  : プレイヤーが選択したキャラクターのインデックス（int）
            Dictionary<string, int> charSelection = (Dictionary<string, int>)charSelectionObj;

            //自分が選択したキャラクターの情報を探す
            if (charSelection.TryGetValue(PhotonNetwork.NickName, out int selectedCharIdx))
            {
                Debug.Log($"{PhotonNetwork.NickName} が選んだキャラ: {selectedCharIdx}");
                //プレイヤーのスポーン
                GameObject selectChar = PhotonNetwork.Instantiate(charBoxList.charBox[selectedCharIdx].name,
                    charBoxList.posBox[SpawnIndex].position, charBoxList.posBox[SpawnIndex].rotation);
                //プレイヤーの子オブジェクト「カメラ」を探す
                Transform childTransform = selectChar.transform.Find("PlayerCamera");
                GameObject selectCharChil = childTransform != null ? childTransform.gameObject : null;

                //カメラを設定
                if (selectCharChil != null)
                {
                    Camera camera = selectCharChil.GetComponent<Camera>();
                    CameraFollow cameraFollow = selectCharChil.GetComponent<CameraFollow>();
                    if (camera != null && cameraFollow != null)
                    {
                        //カメラを追従するスクリプトを有効化
                        camera.enabled = true;
                        cameraFollow.enabled = true;
                    }
                }
                //プレイヤー操作スクリプトを設定
                PlayerController playerController = selectChar.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    //プレイヤーの移動処理のスクリプトを有効化
                    playerController.enabled = true;
                }
            }
        }
        else　{ Debug.LogWarning("CharSelectionのカスタムプロパティが見つかりませんでした"); }
    }

    //プレイヤーのプロパティ更新時のイベント
    public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("CharSelect"))
        {
            int charIdx = (int)changedProps["CharSelect"];
            Debug.Log($"{targetPlayer.NickName} がキャラクター {charIdx} を選択しました。");
            ApplyCharToGame(charIdx);
        }
    }

    //部屋のプロパティ更新時のイベント
    public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("CharSelection"))
        {
            Debug.Log("部屋のキャラクター選択状況を更新中...");
            ApplyUpdatedCharSelection(propertiesThatChanged["CharSelection"]);
        }
    }
    private void ApplyUpdatedCharSelection(object charSelectionObj)
    {
        // キャラクター選択情報の更新を処理
        Dictionary<string, int> charSelection = (Dictionary<string, int>)charSelectionObj;

        if (charSelection.TryGetValue(PhotonNetwork.NickName, out int selectedCharIdx))
        {
            Debug.Log($"{PhotonNetwork.NickName} が選んだキャラ: {selectedCharIdx}");
            ApplyCharToGame(selectedCharIdx);
        }
    }

    //リストの内容をシャッフルする
    //フィッシャーイェーツのシャッフルアルゴリズムについて、以下リンクが参考。
    //参考リンク：https://qiita.com/nkojima/items/c734f786b61a366de831
    private void ShuffleIndex(List<int> Index)
    {
        for (int i = 0; i < Index.Count; i++)
        {
            int Temp = Index[i];
            int RandomIndex = Random.Range(i, Index.Count);

            //入れ替え処理
            Index[i] = Index[RandomIndex];
            Index[RandomIndex] = Temp;
        }
    }

    //死亡後のリスポーンタイマーを開始するメソッド
    public void StartRespawnTimer(GameObject player)
    {
        //5秒後にリスポーン
        StartCoroutine(RespawnPlayer(player, 5f));
    }

    //プレイヤーをリスポーンさせるコルーチン
    private IEnumerator RespawnPlayer(GameObject player, float delay)
    {
        //リスポーンカメラをアクティブにする
        respawnCamera.gameObject.SetActive(true);

        yield return new WaitForSeconds(delay);
        int randomIndex = Random.Range(0, charBoxList.posBox.Count);                   //ランダムなスポーン地点を選択
        Transform respawnPoint = charBoxList.posBox[randomIndex];
        player.transform.position = respawnPoint.position;                      //プレイヤーの位置をリスポーン地点に設定
        player.SetActive(true);                                                 //プレイヤーをアクティブにする
        respawnCamera.gameObject.SetActive(false);                              //リスポーンカメラを非アクティブにする
    }
    public void QuitGame()
    {
        //ゲーム終了
        Application.Quit();
    }
}
