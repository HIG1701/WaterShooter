using UnityEngine;

public class DeadCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform Player;                  //プレイヤーのTransformを設定
    [SerializeField] private Vector3 offset;                    //カメラとプレイヤーの距離を設定

    private void Start()
    {
        //初期オフセットを設定
        offset = transform.position - Player.position;
    }

    private void LateUpdate()
    {
        //カメラの位置をプレイヤーに追従させる
        transform.position = Player.position + offset;
    }
}