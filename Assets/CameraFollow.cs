using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform Player;                              //プレイヤーのTransform
    [SerializeField] Vector3 Offset;                                //カメラのオフセット
    [SerializeField] float Sensitivity = 5f;                        //マウス感度
    [SerializeField] float Distance = 5f;                           //プレイヤーからの距離

    private float CurrentX = 0f;
    private float CurrentY = 0f;

    //垂直方向の回転角度の制限
    private const float AngleMIN = -20f;
    private const float AngleMAX = 80f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;                   //カーソルロック
        Cursor.visible = false;                                     //カーソル非表示
    }

    private void Update()
    {
        CurrentX += Input.GetAxis("Mouse X") * Sensitivity;
        CurrentY -= Input.GetAxis("Mouse Y") * Sensitivity;

        //CurrentYの値をAngleMINとAngleMAXの間に制限する
        CurrentY = Mathf.Clamp(CurrentY, AngleMIN, AngleMAX);
    }

    private void LateUpdate()
    {
        Vector3 direction = new Vector3(0, 0, -Distance);
        //CurrentYとCurrentXを使って回転を計算
        Quaternion rotation = Quaternion.Euler(CurrentY, CurrentX, 0);
        //カメラ位置をプレイヤー位置と回転、オフセットを基に設定
        transform.position = Player.position + rotation * direction + Offset;
        //カメラがプレイヤーの位置を向くよう設定
        transform.LookAt(Player.position + Offset);
    }
}