using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;


    private void Update()
    {
        MoveCamera();                       //カメラ
    }
    private void MoveCamera()
    {
        float mx = Input.GetAxis("Mouse X");                            //カーソルの横の移動量を取得
        float my = Input.GetAxis("Mouse Y");                            //カーソルの縦の移動量を取得

        if (Mathf.Abs(mx) > 0.001f)                                     //X方向に一定量移動していれば横回転
        {
            //transform.RotateAround(回転の中心, 回転の軸（Vector3.upは(0,1,0)のことなのでｙ軸を軸としている）, 変化量); 
            transform.RotateAround(transform.position, Vector3.up, mx); //回転軸はplayerオブジェクトのワールド座標Y軸
        }

        if (Mathf.Abs(my) > 0.001f)                                     //Y方向に一定量移動していれば縦回転
        {
            transform.RotateAround(transform.position, Vector3.right, -my);
        }
    }
}