using UnityEngine;

//このスクリプタブルオブジェクトに、プレイヤーのParameterを書きます
[CreateAssetMenu(menuName = "ScriptableObject/PlayerParameter")]
public class PlayerParameter : ScriptableObject
{
    [Header("プレイヤーの設定に関するParameter")]
    [SerializeField] private int playerID;                    //プレイヤーID
    [SerializeField] private string playerName;               //プレイヤー名
    [SerializeField] private float playerHeight;              //プレイヤー身長

    [Header("プレイヤーのInGameに影響するParameter")]
    [SerializeField] private float playerHP;                  //体力
    [SerializeField] private float playerShield;              //シールド耐久力
    [SerializeField] private float initialVelocity;           //ジャンプ力

    [Header("速度に関するParameter")]
    [SerializeField] private float playerSpeed;               //通常速度
    [SerializeField] private float sprintSpeed;               //加速
    [SerializeField] private float climbSpeed;                //壁を上る速度
    [SerializeField] private float jumpVelocity;              //ジャンプ速度

    public int PlayerID => playerID;
    public string PlayerName => playerName;
    public float PlayerHeight => playerHeight;
    public float PlayerHP => playerHP;
    public float PlayerShield => playerShield;
    public float InitialVelocity => initialVelocity;
    public float PlayerSpeed => playerSpeed;
    public float SprintSpeed => sprintSpeed;
    public float ClimbSpeed => climbSpeed;
    public float JumpVelocity => jumpVelocity;
}