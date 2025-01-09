using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetPlayerName : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField playerNameInput;        //Playerの名前を入れる
    [SerializeField] Text roomName;                         //ルームの名前を入れる
    [SerializeField] Transform playerListContent;           //ルーム画面の参加プレイヤーの表示位置
    [SerializeField] GameObject playerListPrefab;           //参加しているプレイヤーを表示するPrefab

    public override void OnJoinedRoom()
    {
        Debug.Log("部屋に参加しました");
    }
    public void PlayerName()
    {
        string name = playerNameInput.text;

        //ルームカスタムプロパティから既存のニックネームリストを取得
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("Nicknames", out var existingNicknamesObj))
        {
            HashSet<string> existingNicknames = new HashSet<string>((string[])existingNicknamesObj);

            //入力された名前がすでに存在する場合、一意な名前を生成
            while (existingNicknames.Contains(name) || string.IsNullOrEmpty(name))
            {
                name = $"{name}_{Random.Range(1000, 9999)}";
            }
            //ニックネームを設定
            PhotonNetwork.NickName = name;
            //ニックネームリストを更新して同期
            existingNicknames.Add(name);
            PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
            {
                { "Nicknames", existingNicknames.ToArray() }
            });
        }
        else
        {
            //初回設定の場合
            HashSet<string> newNicknames = new HashSet<string> { name };
            PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
            {
                { "Nicknames", newNicknames.ToArray() }
            });

            PhotonNetwork.NickName = name;
        }
        MenuMng.Instance.OpenMenu("room");
        //入力フィールドをクリア
        playerNameInput.text = "";

        roomName.text = PhotonNetwork.CurrentRoom.Name;
        //プレイヤーリストを更新
        Player[] players = PhotonNetwork.PlayerList;
        //既存のプレイヤーリストをクリア
        foreach (Transform trans in playerListContent)
        {
            Destroy(trans.gameObject);
        }
        //新しいプレイヤーリストを作成
        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerList>().SetUp(players[i]);
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerList>().SetUp(newPlayer);
    }
}
