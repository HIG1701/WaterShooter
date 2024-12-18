using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour
{
    [SerializeField] private GameObject cmr;
    [SerializeField] private CharBoxList charBoxList;   //キャラとスポーン位置格納リスt    
    [SerializeField] private Camera respawnCamera;      //リスポーン中に使用するカメラ

    private void Start()
    {
        RetrievePlayerChar();
        if (cmr != null)
        {
            //Destroy(cmr);
        }
    }
    //プレイヤーを探す
    private void RetrievePlayerChar()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue("CharSelect", out object charIndexObj))
            {
                int charIdx = (int)charIndexObj;
                ApplyCharToGame(charIdx);
                Debug.Log("charIdx : " + charIdx);
            }
            else
            {
                Debug.LogWarning("キャラクタが選択されていません");
            }
        }
    }
    //選んだキャラを設定していく
    private void ApplyCharToGame(int charIdx)
    {
        //スポーン地点の数だけインデックスを作成
        List<int> SpawnIndices = new List<int>();
        for (int i = 0; i < charBoxList.posBox.Count; i++)
        {
            SpawnIndices.Add(i);
        }

        //シャッフルメソッドを呼び出し、シャッフル。
        ShuffleIndex(SpawnIndices);
        
        int SpawnIndex = SpawnIndices[charIdx];

        //プレイヤーのスポーン
        GameObject selectChar = PhotonNetwork.Instantiate(charBoxList.charBox[charIdx].name,
            charBoxList.posBox[SpawnIndex].position, charBoxList.posBox[SpawnIndex].rotation);
        bool isMine = selectChar.GetComponent<PhotonView>().IsMine;
        SetChar(selectChar, isMine);
    }
    //キャラの固有情報をセット
    private static void SetChar(GameObject selectChar, bool isMine)
    {
        //キャラの選択状況確認
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("CharSelection", out object charSelectionObj))
        {
            Dictionary<string, int> charSelection = (Dictionary<string, int>)charSelectionObj;

            foreach (var entry in charSelection)
            {
                Debug.Log($"{entry.Key} がキャラ{entry.Value} を選択しました。");
            }
        }
        //カメラを設定
        Transform childTransform = selectChar.transform.Find("PlayerCamera");
        GameObject selectCharChil = childTransform != null ? childTransform.gameObject : null;
        //if (isMine)
        //{
        //    Camera cameraComponent = selectCharChil.AddComponent<Camera>();
        //}
        //if (selectCharChil != null)
        //{
        //    CameraFollow cameraFollow = selectCharChil.GetComponent<CameraFollow>();
        //    if (cameraFollow != null)
        //    {
        //        cameraFollow.enabled = isMine;
        //    }
        //}

        //プレイヤー操作スクリプトを設定
        PlayerController playerController = selectChar.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = isMine;
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
}
