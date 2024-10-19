using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float PlayerSpeed = 10f;

    private void Update()
    {
        PlayerShift();
        PlayerDash();
        PlayerMove();
        PlayerJump();
        Playerfire();
        //リロードRキー
        //マウスScrollで飲料選択。数字でも可
        //壁に向かってWSキー
        //アビリティQキー
    }

    private void PlayerMove()
    {
        //WASDキーで移動
    }

    private void PlayerDash()
    {
        //Controlキー押したときに加速
    }

    private void PlayerJump()
    {
        //Space入力時jump
    }

    private void PlayerShift()
    {
        //シフトでしゃがむ
    }

    private void Playerfire()
    {
        //左マウスで発射
        //右マウスでエイム
    }
}