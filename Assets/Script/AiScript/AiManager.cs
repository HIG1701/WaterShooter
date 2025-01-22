using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// InGameのプレイヤ、キャラデータベースの更新など.
/// </summary>
public static class PlayerManager
{
    public static List<PlayerDate> parameters = new List<PlayerDate>();

    public static void ParametersReset()
    {
        parameters.Clear();
    }

    public static void CopyMasterDateToRuntimeDate()
    {
        for (int i = 0; i < parameters.Count; i++)
        {
            parameters[i].playerRuntimeDate.date.playerID = parameters[i].playerParameter.PlayerID;
            parameters[i].playerRuntimeDate.date.playerName = parameters[i].playerParameter.PlayerName;
            parameters[i].playerRuntimeDate.date.playerHP = parameters[i].playerParameter.PlayerHP;
            parameters[i].playerRuntimeDate.date.playerShield = parameters[i].playerParameter.PlayerShield;
            parameters[i].playerRuntimeDate.date.initialVelocity = parameters[i].playerParameter.InitialVelocity;
            parameters[i].playerRuntimeDate.date.playerSpeed = parameters[i].playerParameter.PlayerSpeed;
            parameters[i].playerRuntimeDate.date.sprintSpeed = parameters[i].playerParameter.SprintSpeed;
            parameters[i].playerRuntimeDate.date.jumpVelocity = parameters[i].playerParameter.JumpVelocity;
        }
    }

    public static void SetPlayerDate(PlayerController playerController)
    {
        PlayerParameter player = playerController.gameObject.GetComponent<PlayerController>().SendPlayerParameter;
        PlayerDate playerDate = new PlayerDate();
        playerDate.playerParameter = player;
        parameters.Add(playerDate);
    }

    public static void SetPlayerDate(AIControl aiControl)
    {
        PlayerParameter player = aiControl.gameObject.GetComponent<AIControl>().SendPlayerParameter;
        PlayerDate playerDate = new PlayerDate();
        playerDate.playerParameter = player;
        parameters.Add(playerDate);
    }

}

public static class DamageManager
{
    /// <summary>
    /// 銃くらったときの処理
    /// </summary>
    /// <param name="playerController">この中に入ってるパラメータがほしい</param>
    /// <param name="gunParameter">銃のパラメータももらいます</param>
    public static void HitToCharacter(PlayerController playerController, GunParameter gunParameter)
    {
        //例外をあとでいれる
        PlayerParameter thisParameter = playerController.SendPlayerParameter;
        for (int i = 0; i < PlayerManager.parameters.Count; i++)
        {
            if (thisParameter.PlayerID == PlayerManager.parameters[i].playerRuntimeDate.date.playerID)
            {
                //テスト用なので後々シールド処理や距離減衰、当たり所のDPS変動を追加
                PlayerManager.parameters[i].playerRuntimeDate.date.playerHP = PlayerManager.parameters[i].playerRuntimeDate.date.playerHP - gunParameter.GunPower;
                Debug.Log(PlayerManager.parameters[i].playerRuntimeDate.date.playerHP);
                break;
            }
        }

    }

    /// <summary>
    /// 上の処理のBOT用オーバーロード
    /// </summary>
    /// <param name="playerAiController"></param>
    /// <param name="gunParameter"></param>
    public static void HitToCharacter(AIControl playerAiController, GunParameter gunParameter)
    {
        //例外をあとでいれる
        PlayerParameter thisParameter = playerAiController.SendPlayerParameter;
        for (int i = 0; i < PlayerManager.parameters.Count; i++)
        {
            if (thisParameter.PlayerID == PlayerManager.parameters[i].playerRuntimeDate.date.playerID)
            {
                //テスト用なので後々シールド処理や距離減衰、当たり所のDPS変動を追加
                PlayerManager.parameters[i].playerRuntimeDate.date.playerHP = PlayerManager.parameters[i].playerRuntimeDate.date.playerHP - gunParameter.GunPower;
                Debug.Log(PlayerManager.parameters[i].playerRuntimeDate.date.playerHP);
                break;
            }
        }

    }

    ///// <summary>
    ///// キル、デスを返す
    ///// </summary>
    ///// <param name="killPlayerDate">キルしたプレイヤーのデータ</param>
    ///// <param name="deathPlayerDate">デスしたプレイヤーのデータ</param>
    ///// <returns>名前やらIDが帰ってくる</returns>
    //public static SendKillDeathLog AddKill(PlayerRuntimeDate killPlayerDate, PlayerRuntimeDate deathPlayerDate)
    //{
    //    killPlayerDate.date.killCount++;
    //    deathPlayerDate.date.deathCount++;
    //    return new SendKillDeathLog
    //    (
    //         killPlayerDate.date.playerName,
    //         killPlayerDate.date.playerID,
    //         deathPlayerDate.date.playerName,
    //         deathPlayerDate.date.playerID
    //    );
    //}
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

public class PlayerDate
{
    public PlayerParameter playerParameter = new();
    public PlayerRuntimeDate playerRuntimeDate = new();
    public PlayerManagementDate playerManagementDate = new();
}

/// <summary>
/// 中身が自由にいじれるようになったPlayerParameter.
/// </summary>
public class PlayerRuntimeDate
{
    public PlayerRuntimeDateStruct date;

    public PlayerRuntimeDate()
    {
        date = new PlayerRuntimeDateStruct();
    }

    public PlayerRuntimeDate(PlayerRuntimeDate playerDate)
    {
        this.date = playerDate.date;
    }
}
public struct PlayerRuntimeDateStruct
{
    public int playerID;                    //プレイヤーID
    public string playerName;               //プレイヤー名

    public float playerHP;                  //体力
    public float playerShield;              //シールド耐久力
    public float initialVelocity;           //ジャンプ力

    public float playerSpeed;               //通常速度
    public float sprintSpeed;               //加速
    public float jumpVelocity;              //ジャンプ速度
}

/// <summary>
/// 一時的なゲーム進行をささえるデータ.
/// </summary>
public class PlayerManagementDate
{
    public PlayerManagementDateStruct date;

    public PlayerManagementDate()
    {
        date = new PlayerManagementDateStruct();
    }

    public PlayerManagementDate(PlayerManagementDate playerDate)
    {
        this.date = playerDate.date;
    }
}
public struct PlayerManagementDateStruct
{
    public int killCount;
    public int damageCount;
    public int deathCount;

    public PlayerRuntimeDate lastPlayerToDamage;

    public PlayerInventory playerInventory;
}