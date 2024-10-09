using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject Player;
    private Vector3 CurrentPos;             //現在のカメラ位置
    private Vector3 PastPos;                //過去のカメラ位置
    private Vector3 Distance;                   //移動距離

    private void Start()
    {
        //最初のプレイヤーの位置の取得
        PastPos = Player.transform.position;
    }
    private void Update()
    {
        MoveCamera();
        RotateCamera();
    }

    private void MoveCamera()
    {
        //プレイヤーの現在地の取得
        CurrentPos = Player.transform.position;
        Distance = CurrentPos - PastPos;
        transform.position = Vector3.Lerp(transform.position, transform.position + Distance, 1.0f);
        PastPos = CurrentPos;
    }

    private void RotateCamera()
    {
        //マウス移動
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(mx) > 0.01f)
            //Y座標
            transform.RotateAround(Player.transform.position, Vector3.up, mx);
        if (Mathf.Abs(my) > 0.01f)
            //X座標
            transform.RotateAround(Player.transform.position, transform.right, -my);
    }
}