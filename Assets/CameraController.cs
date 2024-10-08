using UnityEngine;

//一応残しますが、使わないかも

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;                     //playerのゲームオブジェクトを入れる変数を設定
    private void Update()
    {
        float my = Input.GetAxis("Mouse Y");                //マウスの縦方向の移動量を取得

        if (Mathf.Abs(my) > 0.001f)                         //Y方向に一定量移動していれば縦回転
        {
            //transform.RotateAround(player.transform.position, transform.right, -my);
            transform.RotateAround(player.transform.position, Vector3.right, -my);
        }
    }
}