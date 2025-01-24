using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// InGameのプレイヤ、キャラデータベースの更新など.
/// </summary>
public static class PlayerManager
{
    public static List<PlayerDate> parameters = new List<PlayerDate>();

    /// <summary>
    /// キャラデータベースリセット.
    /// </summary>
    public static void ParametersReset()
    {
        parameters.Clear();
    }

    public static void CopyMasterDateToRuntimeDate()
    {
        for (int i = 0; i < parameters.Count; i++)
        {
            parameters[i].playerRuntimeDate.date.playerName = parameters[i].playerParameter.PlayerName;
            parameters[i].playerRuntimeDate.date.playerHP = parameters[i].playerParameter.PlayerHP;
            parameters[i].playerRuntimeDate.date.playerShield = parameters[i].playerParameter.PlayerShield;
            parameters[i].playerRuntimeDate.date.initialVelocity = parameters[i].playerParameter.InitialVelocity;
            parameters[i].playerRuntimeDate.date.playerSpeed = parameters[i].playerParameter.PlayerSpeed;
            parameters[i].playerRuntimeDate.date.sprintSpeed = parameters[i].playerParameter.SprintSpeed;
            parameters[i].playerRuntimeDate.date.jumpVelocity = parameters[i].playerParameter.JumpVelocity;
        }
    }

    /// <summary>
    /// プレイヤーのデータをリストに追加.
    /// </summary>
    /// <param name="playerController">追加するプレイヤー</param>
    public static void SetPlayerDate(PlayerController playerController)
    {
        PlayerParameter player = playerController.gameObject.GetComponent<PlayerController>().SendPlayerParameter;
        PlayerDate playerDate = new PlayerDate();
        playerDate.playerParameter = player;
        parameters.Add(playerDate);
    }

    /// <summary>
    /// BOTのデータをリストに追加.
    /// </summary>
    /// <param name="aiControl">追加するBOT</param>
    public static void SetPlayerDate(AIControl aiControl)
    {
        PlayerParameter player = aiControl.gameObject.GetComponent<AIControl>().SendPlayerParameter;
        PlayerDate playerDate = new PlayerDate();
        playerDate.playerParameter = player;
        parameters.Add(playerDate);
    }

}

/// <summary>
/// データベース内の各種データを渡す.
/// </summary>
public static class ParameterDate
{
    /// <summary>
    /// 残弾を渡す(Player).
    /// </summary>
    /// <param name="playerController"></param>
    /// <returns></returns>
    public static int SendCurrentAmmo(PlayerController playerController)
    {
        return PlayerManager.parameters[playerController.CharacterIndex].playerRuntimeDate.date.currentAmmo;
    }

    /// <summary>
    /// 残弾を渡す(Bot).
    /// </summary>
    /// <param name="playerController"></param>
    /// <returns></returns>
    public static int SendCurrentAmmo(AIControl aIControl)
    {
        return PlayerManager.parameters[aIControl.CharacterIndex].playerRuntimeDate.date.currentAmmo;
    }

    /// <summary>
    /// 武器命中率を渡す(Player).
    /// </summary>
    /// <returns>命中率</returns>
    public static int SendAccuracy(PlayerController playerController)
    {
        float thisTotalFireAmmo = PlayerManager.parameters[playerController.CharacterIndex].playerRuntimeDate.date.totalFireAmmo;
        float thisHItAmmo = PlayerManager.parameters[playerController.CharacterIndex].playerRuntimeDate.date.hitAmmo;
        if (thisTotalFireAmmo == 0) return 0;
        float thisAccuracy = thisHItAmmo / thisTotalFireAmmo * 100;
        int accuracy = (int)thisAccuracy;
        return accuracy;
    }

    /// <summary>
    /// 武器命中率を渡す(Bot).
    /// </summary>
    /// <returns>命中率</returns>
    public static int SendAccuracy(AIControl aIControl)
    {
        int thisTotalFireAmmo = PlayerManager.parameters[aIControl.CharacterIndex].playerRuntimeDate.date.totalFireAmmo;
        int thisHItAmmo = PlayerManager.parameters[aIControl.CharacterIndex].playerRuntimeDate.date.hitAmmo;
        int accuracy = (thisHItAmmo / thisTotalFireAmmo) * 100;
        return accuracy;
    }

    /// <summary>
    /// HPを渡す(Player).
    /// </summary>
    /// <param name="playerController">HPを参照したいキャラ</param>
    /// <returns>キャラHP(整数型)</returns>
    public static int SendCharacterHP(PlayerController playerController)
    {
        int characterHP = 0;
        for (int i = 0; 1 < PlayerManager.parameters.Count; i++)
        {
            if (PlayerManager.parameters[i].playerRuntimeDate.date.playerID == playerController.CharacterIndex)
            {
                characterHP = (int)PlayerManager.parameters[i].playerRuntimeDate.date.playerHP;
                return characterHP;
            }
        }
        return characterHP;
    }

    /// <summary>
    /// HPを渡す(BOT).
    /// </summary>
    /// <param name="playerController">HPを参照したいキャラ</param>
    /// <returns>キャラHP(整数型)</returns>
    public static int SendCharacterHP(AIControl aIControl)
    {
        int characterHP = 0;
        for (int i = 0; 1 < PlayerManager.parameters.Count; i++)
        {
            if (PlayerManager.parameters[i].playerRuntimeDate.date.playerID == aIControl.CharacterIndex)
            {
                characterHP = (int)PlayerManager.parameters[i].playerRuntimeDate.date.playerHP;
                return characterHP;
            }
        }
        return characterHP;
    }

    /// <summary>
    /// キル数を渡す(Player).
    /// </summary>
    /// <param name="playerController">キル数を参照したいキャラ</param>
    /// <returns>キル数</returns>
    public static int SendCharacterKillCount(PlayerController playerController)
    {
        int killCount = 0;
        for (int i = 0; 1 < PlayerManager.parameters.Count; i++)
        {
            if (PlayerManager.parameters[i].playerRuntimeDate.date.playerID == playerController.CharacterIndex)
            {
                killCount = PlayerManager.parameters[i].playerManagementDate.date.killCount;
                return killCount;
            }
        }
        return killCount;
    }

    /// <summary>
    /// キル数を渡す(Bot).
    /// </summary>
    /// <param name="playerController">キル数を参照したいキャラ</param>
    /// <returns>キル数</returns>
    public static int SendCharacterKillCount(AIControl aIControl)
    {
        int killCount = 0;
        for (int i = 0; 1 < PlayerManager.parameters.Count; i++)
        {
            if (PlayerManager.parameters[i].playerRuntimeDate.date.playerID == aIControl.CharacterIndex)
            {
                killCount = PlayerManager.parameters[i].playerManagementDate.date.killCount;
                return killCount;
            }
        }
        return killCount;
    }

    /// <summary>
    /// デス数を渡す(Player).
    /// </summary>
    /// <param name="playerController">デス数を参照したいキャラ</param>
    /// <returns>デス数</returns>
    public static int SendCharacterDeathCount(PlayerController playerController)
    {
        int deathCount = 0;
        for (int i = 0; 1 < PlayerManager.parameters.Count; i++)
        {
            if (PlayerManager.parameters[i].playerRuntimeDate.date.playerID == playerController.CharacterIndex)
            {
                deathCount = PlayerManager.parameters[i].playerManagementDate.date.deathCount;
                return deathCount;
            }
        }
        return deathCount;
    }

    /// <summary>
    /// デス数を渡す(Bot).
    /// </summary>
    /// <param name="playerController">デス数を参照したいキャラ</param>
    /// <returns></returns>
    public static int SendCharacterDeathCount(AIControl aiControl)
    {
        int deathCount = 0;
        for (int i = 0; 1 < PlayerManager.parameters.Count; i++)
        {
            if (PlayerManager.parameters[i].playerRuntimeDate.date.playerID == aiControl.CharacterIndex)
            {
                deathCount = PlayerManager.parameters[i].playerManagementDate.date.deathCount;
                return deathCount;
            }
        }
        return deathCount;
    }

    /// <summary>
    /// キル、デスを返す
    /// </summary>
    /// <param name="killPlayerDate">キルしたプレイヤーのデータ</param>
    /// <param name="deathPlayerDate">デスしたプレイヤーのデータ</param>
    /// <returns>名前やらIDが帰ってくる</returns>
    public static SendKillDeathLog AddKill(PlayerDate killPlayerDate, PlayerDate deathPlayerDate)
    {
        killPlayerDate.playerManagementDate.date.killCount++;
        deathPlayerDate.playerManagementDate.date.deathCount++;

        return new SendKillDeathLog
        (
            killPlayerDate.playerRuntimeDate.date.playerName,
            killPlayerDate.playerRuntimeDate.date.playerID,
            deathPlayerDate.playerRuntimeDate.date.playerName,
            deathPlayerDate.playerRuntimeDate.date.playerID
        );
    }
}

public static class SetDateManager
{
    /// <summary>
    /// 銃くらったときの処理
    /// </summary>
    /// <param name="playerController">この中に入ってるパラメータがほしい</param>
    /// <param name="gunParameter">銃のパラメータももらいます</param>
    public static void HitToCharacter(PlayerController playerController, GunParameter gunParameter, int charaIndex)
    {
        //例外をあとでいれる
        for (int i = 0; i < PlayerManager.parameters.Count; i++)
        {
            if (playerController.CharacterIndex == PlayerManager.parameters[i].playerRuntimeDate.date.playerID)
            {
                //テスト用なので後々シールド処理や距離減衰、当たり所のDPS変動を追加
                PlayerManager.parameters[i].playerRuntimeDate.date.playerHP = PlayerManager.parameters[i].playerRuntimeDate.date.playerHP - gunParameter.GunPower;
                PlayerManager.parameters[i].playerManagementDate.date.lastPlayerToDamage = PlayerManager.parameters[charaIndex].playerRuntimeDate;
                PlayerManager.parameters[charaIndex].playerRuntimeDate.date.hitAmmo++;
                Debug.LogWarning(PlayerManager.parameters[i].playerRuntimeDate.date.playerHP);
                break;
            }
        }

    }

    /// <summary>
    /// 上の処理のBOT用オーバーロード
    /// </summary>
    /// <param name="playerAiController">ダメージを受けたキャラ</param>
    /// <param name="gunParameter">ダメージを与えたキャラの武器</param>
    public static void HitToCharacter(AIControl playerAiController, GunParameter gunParameter, int charaIndex)
    {
        //例外をあとでいれる
        for (int i = 0; i < PlayerManager.parameters.Count; i++)
        {
            if (playerAiController.CharacterIndex == PlayerManager.parameters[i].playerRuntimeDate.date.playerID)
            {
                //テスト用なので後々シールド処理や距離減衰、当たり所のDPS変動を追加
                PlayerManager.parameters[i].playerRuntimeDate.date.playerHP = PlayerManager.parameters[i].playerRuntimeDate.date.playerHP - gunParameter.GunPower;
                PlayerManager.parameters[i].playerManagementDate.date.lastPlayerToDamage = PlayerManager.parameters[charaIndex].playerRuntimeDate;
                PlayerManager.parameters[charaIndex].playerRuntimeDate.date.hitAmmo++;
                Debug.LogWarning(PlayerManager.parameters[i].playerRuntimeDate.date.playerHP);
                break;
            }
        }

    }

    /// <summary>
    /// 撃った弾の数.
    /// </summary>
    /// <param name="deathPlayerDate">発砲したキャラ</param>
    public static void TotalFireCount(int charaIndex)
    {
        var fireCharacter = PlayerManager.parameters[charaIndex];
        fireCharacter.playerRuntimeDate.date.totalFireAmmo++;
    }

    /// <summary>
    /// デス数を加算する.
    /// </summary>
    /// <param name="deathPlayerDate">デスしたキャラのデータ</param>
    public static void DeathCount(int charaIndex)
    {
        var deathPlayerDate = PlayerManager.parameters[charaIndex];
        deathPlayerDate.playerManagementDate.date.deathCount++;
        deathPlayerDate.playerRuntimeDate.date.playerHP = deathPlayerDate.playerParameter.PlayerHP;
    }

    /// <summary>
    /// キル数を加算する.
    /// </summary>
    /// <param name="charaIndex">キルしたキャラのインデックス</param>
    public static void KillCount(int charaIndex)
    {
        var killPlayerDate = PlayerManager.parameters[charaIndex];
        killPlayerDate.playerManagementDate.date.killCount++;
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
/// インゲーム実行時にキャラクタの各種状態を保存・更新を行えるデータ.
/// </summary>
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

    public int currentAmmo;                 //残弾
    public int totalFireAmmo;               //撃った弾.
    public int hitAmmo;                     //当たった弾の数.
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