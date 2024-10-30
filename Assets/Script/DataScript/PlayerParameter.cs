using UnityEngine;

//このスクリプタブルオブジェクトに、プレイヤーのParameterを書きます
[CreateAssetMenu(menuName = "ScriptableObject/PlayerParameter")]

public class PlayerParameter : ScriptableObject
{
    [Header("プレイヤーの設定に関するParameter")]
    public int PlayerID;                    //プレイヤーID
    public string PlayerName;               //プレイヤー名
    public float PlayerHeight;              //プレイヤー身長

    [Header("プレイヤーのInGameに影響するParameter")]
    public float PlayerHP;                  //体力
    public float PlayerShield;              //シールド耐久力
    public int Coin;                        //コイン枚数

    [Header("速度に関するParameter")]
    public float PlayerSpeed;               //通常速度
    public float SprintSpeed;               //加速
    public float DownSpeed;                 //壁に当たっている時
    public float JumpVelocity;              //ジャンプ速度

    [Header("重力に関するParameter")]
    public float GravityMultiplier;         //このオブジェクトのみ、重力の影響を強める
}