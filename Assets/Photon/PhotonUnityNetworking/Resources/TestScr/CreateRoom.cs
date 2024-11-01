using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Collections;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] CharBoxList charBoxList;
    [SerializeField] private Camera respawnCamera;                  //リスポーン中に使用するカメラ

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    //ルーム入室前に呼び出し
    public override void OnConnectedToMaster()
    {
        //"room1"という名前のルームに参加(ルームがないなら作成してから参加)
        Debug.Log("ルーム入出前");
        PhotonNetwork.JoinOrCreateRoom("room1", new RoomOptions(), TypedLobby.Default);
    }
    //ルーム入室後に呼び出し
    public override void OnJoinedRoom()
    {
        Debug.Log("ルーム入出後");
        //キャラキラー生成
        SpawnPlayers();
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
    //プレイヤーをスポーンさせるメソッド
    private void SpawnPlayers()
    {
        //スポーン地点の数だけインデックスを作成
        List<int> SpawnIndices = new List<int>();
        for (int i = 0; i < charBoxList.posBox.Count; i++)
        {
            SpawnIndices.Add(i);
        }

        //シャッフルメソッドを呼び出し、シャッフル。
        ShuffleIndex(SpawnIndices);

        //プレイヤーをスポーン
        for (int i = 0; i < charBoxList.charBox.Count; i++)
        {
            int SpawnIndex = SpawnIndices[i];
            //charBoxList.charBox[i]：現在のプレイヤー
            //charBoxList.posBox[spawnIndex].position：対応するスポーン位置
            //charBoxList.posBox[spawnIndex].rotation：対応するスポーンの向き
            //Instantiate(charBoxList.charBox[i], charBoxList.posBox[SpawnIndex].position, charBoxList.posBox[SpawnIndex].rotation);
            if (charBoxList.charBox[i].charPrefab.tag == "Player")
            {
                GameObject myChar = PhotonNetwork.Instantiate(charBoxList.charBox[i].charName,
                    charBoxList.posBox[SpawnIndex].position, charBoxList.posBox[SpawnIndex].rotation);
                //自分のみ操作可能にする
                PlayerController playerController = myChar.GetComponent<PlayerController>();
                playerController.enabled = true;
                Transform childTransform = myChar.transform.Find("PlayerCamera");
                GameObject myCharChil = childTransform.gameObject;
                CameraFollow cameraFollow = myCharChil.GetComponent<CameraFollow>();
                cameraFollow.enabled = true;
                //GameObject myCharChil = gameObject.transform.Find("PlayerCamera")?.gameObject;
                if (myCharChil == null)
                {
                    Debug.LogError("PlayerCamera not found or is inactive.");
                }

                //CameraFollow cameraFollow = myCharChil.GetComponent<CameraFollow>();
                //cameraFollow.enabled = true;
            }
            else if (charBoxList.charBox[i].charPrefab.tag == "NPC")
            {
                Instantiate(charBoxList.charBox[i].charPrefab, charBoxList.posBox[SpawnIndex].position, charBoxList.posBox[SpawnIndex].rotation);
            }
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
