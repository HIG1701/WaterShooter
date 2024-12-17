using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// やってることほぼゲームマネージャ
/// </summary>
public class HitScan
{
    private PlayerDate playerDate;
    private static List<PlayerDate> parameters = new List<PlayerDate>();

    public void ParametersReset()
    {
        parameters.Clear();
    }

    /// <summary>
    /// 可変のパラメータとしてコビー。
    /// </summary>
    /// <param name="playerParameter">受け取ったマスターデータをリストに格納</param>
    public void AddPlayerParameter(PlayerParameter playerParameter)
    {
        playerDate = new PlayerDate();
        playerDate.date.playerID = playerParameter.PlayerID;
        playerDate.date.playerName = playerParameter.PlayerName;
        playerDate.date.playerHeight = playerParameter.PlayerHeight;
        playerDate.date.playerHP = playerParameter.PlayerHP;
        playerDate.date.playerShield = playerParameter.PlayerShield;
        playerDate.date.initialVelocity = playerParameter.InitialVelocity;
        playerDate.date.playerSpeed = playerParameter.PlayerSpeed;
        playerDate.date.sprintSpeed = playerParameter.SprintSpeed;
        playerDate.date.jumpVelocity = playerParameter.JumpVelocity;
        playerDate.date.playerInventory = playerParameter.playerInventory;
        playerDate.date.lastPlayerToDamage = null;
        playerDate.date.killCount = 0;
        playerDate.date.damageCount = 0;
        playerDate.date.deathCount = 0;
        parameters.Add(playerDate);
    }

    public void Test()
    {
        Debug.Log(parameters[0].date.playerID);
        Debug.Log(parameters[1].date.playerID);
    }

    /// <summary>
    /// 銃くらったときの処理
    /// </summary>
    /// <param name="playerController">この中に入ってるパラメータがほしい</param>
    /// <param name="gunParameter">銃のパラメータももらいます</param>
    public void HitToCharacter(PlayerController playerController, GunParameter gunParameter)
    {
        //例外をあとでいれる
        PlayerParameter thisParameter = playerController.SendPlayerParameter;
        for (int i = 0; i < parameters.Count; i++)
        {
            if (thisParameter.PlayerID == parameters[i].date.playerID)
            {
                //テスト用なので後々シールド処理や距離減衰、当たり所のDPS変動を追加
                parameters[i].date.playerHP = parameters[i].date.playerHP - gunParameter.GunPower;
                Debug.Log(parameters[i].date.playerHP);
                break;
            }
        }
        
    }

    /// <summary>
    /// 上の処理のBOT用オーバーロード
    /// </summary>
    /// <param name="playerAiController"></param>
    /// <param name="gunParameter"></param>
    public void HitToCharacter(PlayerAIController playerAiController, GunParameter gunParameter)
    {
        //例外をあとでいれる
        PlayerParameter thisParameter = playerAiController.SendPlayerParameter;
        for (int i = 0; i < parameters.Count; i++)
        {
            if (thisParameter.PlayerID == parameters[i].date.playerID)
            {
                //テスト用なので後々シールド処理や距離減衰、当たり所のDPS変動を追加
                parameters[i].date.playerHP = parameters[i].date.playerHP - gunParameter.GunPower;
                Debug.Log(parameters[i].date.playerHP);
                break;
            }
        }

    }
    
    /// <summary>
    /// キル、デスを返す
    /// </summary>
    /// <param name="killPlayerDate">キルしたプレイヤーのデータ</param>
    /// <param name="deathPlayerDate">デスしたプレイヤーのデータ</param>
    /// <returns>名前やらIDが帰ってくる</returns>
    public SendKillDeathLog AddKill(PlayerDate killPlayerDate, PlayerDate deathPlayerDate)
    {
        killPlayerDate.date.killCount++;
        deathPlayerDate.date.deathCount++;
        return new SendKillDeathLog
        (
             killPlayerDate.date.playerName,
             killPlayerDate.date.playerID,
             deathPlayerDate.date.playerName,
             deathPlayerDate.date.playerID
        );
    }
}

/// <summary>
/// AddKill用の戻り値
/// </summary>
public class SendKillDeathLog
{
    public string KillPlayerName { get; }
    public int KillPlayerID { get; }
    public string DeathPlayerName { get; }
    public int DeathPlayerID { get; }

    public SendKillDeathLog(string killPlayerName, int killPlayerID, string deathPlayerName, int deathPlayerID)
    {
        KillPlayerName = killPlayerName;
        KillPlayerID = killPlayerID;
        DeathPlayerName = deathPlayerName;
        DeathPlayerID = deathPlayerID;
    }
}

/// <summary>
/// 中身が自由にいじれるようになったPlayerParameter.
/// </summary>
public class PlayerDate
{
    public PlayerDateStruct date;

    public PlayerDate()
    {
        date = new PlayerDateStruct();
    }

    public PlayerDate(PlayerDate playerDate)
    {
        this.date = playerDate.date;
    }
}

public struct PlayerDateStruct
{
    public int playerID;                    //プレイヤーID
    public string playerName;               //プレイヤー名
    public float playerHeight;              //プレイヤー身長

    public float playerHP;                  //体力
    public float playerShield;              //シールド耐久力
    public float initialVelocity;           //ジャンプ力

    public float playerSpeed;               //通常速度
    public float sprintSpeed;               //加速
    public float jumpVelocity;              //ジャンプ速度
    
    public int killCount;
    public int damageCount;
    public int deathCount;

    public PlayerDate lastPlayerToDamage;

    public PlayerInventory playerInventory;
}