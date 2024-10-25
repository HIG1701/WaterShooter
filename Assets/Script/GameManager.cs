using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//ゲームシステム全体について記載する
/*
 * このスクリプトに必要なこと
 * 
 * ゲーム開始時、６つのスポーン地点に移動。これらをランダムに決める。
 * ゲーム開始から１５分測り、１５分後ゲームを終了する。
 * マップ外に、ダメージウォールを表示
 * プレイヤー死亡時に蘇生する処理を行う。
 */

public class GameManager : MonoBehaviour
{
    [SerializeField] public List<Transform> SpawnPoints;
    [SerializeField] public List<GameObject> Players;

    private void Start()
    {
        SpawnPlayers();
    }

    private void Update()
    {
        //スポーン地点が動くか確認するためのロードシーン
        if (Input.GetKey(KeyCode.P))
        {
            SceneManager.LoadScene("SampleScene");
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

    //プレイヤーをスポーンさせるメソッド
    private void SpawnPlayers()
    {
        //スポーン地点の数だけインデックスを作成
        List<int> SpawnIndices = new List<int>();
        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            SpawnIndices.Add(i);
        }

        //シャッフルメソッドを呼び出し、シャッフル。
        ShuffleIndex(SpawnIndices);

        //プレイヤーをスポーン
        for (int i = 0; i < Players.Count; i++)
        {
            int SpawnIndex = SpawnIndices[i];

            //players[i]：現在のプレイヤー
            //spawnPoints[spawnIndex].position：対応するスポーン位置
            //spawnPoints[spawnIndex].rotation：対応するスポーンの向き
            Instantiate(Players[i], SpawnPoints[SpawnIndex].position, SpawnPoints[SpawnIndex].rotation);
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
        yield return new WaitForSeconds(delay);
        int randomIndex = Random.Range(0, SpawnPoints.Count);                   //ランダムなスポーン地点を選択
        Transform respawnPoint = SpawnPoints[randomIndex];
        player.transform.position = respawnPoint.position;                      //プレイヤーの位置をリスポーン地点に設定
        player.SetActive(true);                                                 //プレイヤーをアクティブにする
    }
}