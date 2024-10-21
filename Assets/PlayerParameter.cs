using UnityEngine;

//このスクリプタブルオブジェクトに、プレイヤーのParameterを書きます
[CreateAssetMenu(menuName = "ScriptableObject/PlayerParameter")]
public class PlayerParameter : ScriptableObject
{
    //速度に関するParameter
    public float PlayerSpeed;               //通常速度
    public float SprintSpeed;               //加速
    public float DownSpeed;                 //壁に当たっている時
    public float JumpVelocity;              //ジャンプ速度

    //重力に関するParameter
    public float GravityMultiplier;         //このオブジェクトのみ、重力の影響を強める

}
