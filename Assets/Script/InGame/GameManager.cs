using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO:プレイヤーのメソッドを読んだりしてるので、設計を見直そう
/* このスクリプトに必要なこと
 * 
 * ゲーム開始時、６つのスポーン地点に移動。これらをランダムに決める。
 * ゲーム開始から１５分測り、１５分後ゲームを終了する。
 * マップ外に、ダメージウォールを表示
 * プレイヤー死亡時に蘇生する処理を行う。
 * コインの量などで勝敗を決めるのはこのクラス？
 */

/// <summary>
/// ゲームシステム全体に関するクラス
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] public List<Transform> spawnPoints;
    [SerializeField] public List<GameObject> players;

    private void Start()
    {
        //StartSpawnPlayers(true);
        PlayerManager.ParametersReset();
        SetParameterDate();
        PlayerManager.CopyMasterDateToRuntimeDate();
    }

    private void Update()
    {
        //スポーン地点が動くか確認するためのロードシーン
        if (Input.GetKey(KeyCode.P)) SceneManager.LoadScene("SampleScene");
    }

    private void FixedUpdate()
    {
        DeathProcessing();
    }

    private void SetParameterDate()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].GetComponentInChildren<PlayerController>() != null)
            {
                players[i].GetComponentInChildren<PlayerController>().CharacterIndex = i;
                PlayerManager.SetPlayerDate(players[i].GetComponentInChildren<PlayerController>());
                PlayerManager.parameters[i].playerRuntimeDate.date.playerID = i;
            }
            else if (players[i].GetComponentInChildren<AIControl>()  != null)
            {
                players[i].GetComponentInChildren<AIControl>().CharacterIndex = i;
                PlayerManager.SetPlayerDate(players[i].GetComponentInChildren<AIControl>());
                PlayerManager.parameters[i].playerRuntimeDate.date.playerID = i;
            }
        }
    }

    private void DeathProcessing()
    {
        Debug.LogWarning(PlayerManager.parameters.Count);
        for (int i = 0; i < PlayerManager.parameters.Count; i++)
        {
            if (PlayerManager.parameters[i].playerRuntimeDate.date.playerHP <= 0 && players[i].activeSelf)
            {
                SetDateManager.DeathCount(i);
                players[i].gameObject.SetActive(false);
                StartRespawnTimer(players[i]);
                if (PlayerManager.parameters[i].playerManagementDate.date.lastPlayerToDamage != null) SetDateManager.KillCount(PlayerManager.parameters[i].playerManagementDate.date.lastPlayerToDamage.date.playerID);
            }
        }
    }

    /// <summary>
    /// リストのシャッフルを行うメソッド
    /// </summary>
    //参考リンク：https://qiita.com/nkojima/items/c734f786b61a366de831
    private void ShuffleIndex(List<int> index)
    {
        for (int i = 0; i < index.Count; i++)
        {
            int temp = index[i];
            int randomIndex = Random.Range(i, index.Count);

            //入れ替え処理
            index[i] = index[randomIndex];
            index[randomIndex] = temp;
        }
    }

    private void StartSpawnPlayers()
    {
        //スポーン地点の数だけインデックスを作成
        List<int> spawnIndices = new List<int>();
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            spawnIndices.Add(i);
        }

        ShuffleIndex(spawnIndices);

        for (int i = 0; i < players.Count; i++)
        {
            int spawnIndex = spawnIndices[i];

            //spawnPoints[spawnIndex].position：対応するスポーン位置
            //spawnPoints[spawnIndex].rotation：対応するスポーンの向き
            Instantiate(players[i], spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
        }
    }

    private void StartSpawnPlayers(bool check)
    {
        //スポーン地点の数だけインデックスを作成
        List<int> spawnIndices = new List<int>();
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            spawnIndices.Add(i);
        }

        ShuffleIndex(spawnIndices);

        for (int i = 0; i < players.Count; i++)
        {
            int spawnIndex = spawnIndices[i];

            //spawnPoints[spawnIndex].position：対応するスポーン位置
            //spawnPoints[spawnIndex].rotation：対応するスポーンの向き
            if (players[i].GetComponentInChildren<AIControl>() != null)
            {
                Instantiate(players[i], spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
            }
        }
    }
    public void StartRespawnTimer(GameObject player)
    {
        StartCoroutine(RespawnPlayer(player, 5f));
    }

    /// <summary>
    /// プレイヤーをスポーンさせるメソッド
    /// </summary>
    private IEnumerator RespawnPlayer(GameObject player, float delay)
    {
        Collider col = player.GetComponentInChildren<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
        Rigidbody rb = player.GetComponentInChildren<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        yield return new WaitForSeconds(delay);
        player.SetActive(true);
        int randomIndex = Random.Range(0, spawnPoints.Count);                   //ランダムなスポーン地点を選択
        Transform respawnPoint = spawnPoints[randomIndex];
        player.transform.position = respawnPoint.position;                      //プレイヤーの位置をリスポーン地点に設定
        if (col != null)
        {
            col.enabled = true;
        }

        Debug.Log(respawnPoint.transform.position);
        //プレイヤーをアクティブにする

        //player.GetComponent<PlayerController>().Respawn();
    }
}