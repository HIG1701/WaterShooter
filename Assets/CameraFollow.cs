using UnityEngine;

//このスクリプトでは、マウスがプレイヤーを中心に追従するような処理を実装する
//参考リンク：https://chamucode.com/unity-camera-follow/

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform Player;                              //プレイヤーのTransform
    [SerializeField] Vector3 Offset;                                //カメラのオフセット
    [SerializeField] float Sensitivity = 5f;                        //マウス感度
    [SerializeField] float Distance = 5f;                           //プレイヤーからの距離

    private float CurrentX = 0f;
    private float CurrentY = 0f;

    //垂直方向の回転角度の制限
    private const float AngleMIN = -5f;
    private const float AngleMAX = 80f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;                   //カーソルロック
        Cursor.visible = false;                                     //カーソル非表示
    }

    private void Update()
    {
        CurrentX += Input.GetAxis("Mouse X") * Sensitivity;         //マウスX軸取得
        CurrentY -= Input.GetAxis("Mouse Y") * Sensitivity;         //マウスY軸取得

        //CurrentYの値をAngleMINとAngleMAXの間に制限する
        //Mathf.Clamp：値を指定された範囲の中にとどめる
        //すなわち、Y軸を極端な角度に回転させることを防ぐ
        CurrentY = Mathf.Clamp(CurrentY, AngleMIN, AngleMAX);
    }

    private void LateUpdate()
    {
        Vector3 Direction = new Vector3(0, 0, -Distance);

        //CurrentYとCurrentXを使って回転を計算
        Quaternion Rotation = Quaternion.Euler(CurrentY, CurrentX, 0);

        //カメラ位置をプレイヤー位置と回転、オフセットを基に設定
        transform.position = Player.position + Rotation * Direction + Offset;

        //transform.LookAt：カメラに特定の位置を向かせ続ける
        //カメラがプレイヤーの位置を向くよう設定
        transform.LookAt(Player.position + Offset);
    }
}