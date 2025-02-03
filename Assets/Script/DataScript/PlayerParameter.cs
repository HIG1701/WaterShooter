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
    [SerializeField] private float playerHP = 100f;           //体力
    [SerializeField] private float playerShield = 50f;        //シールド耐久力
    [SerializeField] private float initialVelocity = 5f;      //ジャンプ力
    [SerializeField] private int totalAmmo = 120;
    [SerializeField] private int coin = 0;

    [Header("速度に関するParameter")]
    [SerializeField] private float playerSpeed = 5f;          //通常速度
    [SerializeField] private float sprintSpeed = 10f;         //加速
    [SerializeField] private float climbSpeed = 3f;           //壁を上る速度
    [SerializeField] private float crouchSpeed = 2f;          //しゃがみ速度  
    [SerializeField] private float jumpVelocity = 7f;         //ジャンプ速度

    //プロパティのバリデーション
    private void OnValidate()
    {
        playerHP = Mathf.Max(0, playerHP);
        playerShield = Mathf.Max(0, playerShield);
        initialVelocity = Mathf.Max(0, initialVelocity);
        playerSpeed = Mathf.Max(0, playerSpeed);
        sprintSpeed = Mathf.Max(0, sprintSpeed);
        climbSpeed = Mathf.Max(0, climbSpeed);
        crouchSpeed = Mathf.Max(0, crouchSpeed);
        jumpVelocity = Mathf.Max(0, jumpVelocity);
    }

    //プロパティの公開
    public int PlayerID => playerID;
    public string PlayerName => playerName;
    public float PlayerHeight => playerHeight;
    public float PlayerHP => playerHP;
    public float PlayerShield => playerShield;
    public float InitialVelocity => initialVelocity;
    public float PlayerSpeed => playerSpeed;
    public float SprintSpeed => sprintSpeed;
    public float ClimbSpeed => climbSpeed;
    public float CrouchSpeed => crouchSpeed;
    public float JumpVelocity => jumpVelocity;
    public int TotalAmmo => totalAmmo;
    public int Coin => coin;
}