using UnityEngine;

/// <summary>
/// カメラワークに関するクラス
/// </summary>
//このスクリプトでは、マウスがプレイヤーを中心に追従するような処理を実装する
//参考リンク：https://chamucode.com/unity-camera-follow/

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;
    [SerializeField] float sensitivity = 5f;
    [SerializeField] float distance = 5f;
    [SerializeField] float sphereCastRadius = 0.5f;                 //SphereCastの半径
    [SerializeField] LayerMask sollisionLayers;                     //衝突を検出するレイヤー（インスペクターでWallを指定）

    private float currentX = 0f;
    private float currentY = 0f;

    //垂直方向の回転角度の制限
    private const float angleMIN = -5f;
    private const float angleMAX = 80f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;                   //カーソルロック
        Cursor.visible = false;                                     //カーソル非表示
    }

    private void Update()
    {
        GetMouthPos();
        RotatePlayer();
    }

    private void LateUpdate()
    {
        CameraMove();
    }

    private void GetMouthPos()
    {
        currentX += Input.GetAxis("Mouse X") * sensitivity;
        currentY -= Input.GetAxis("Mouse Y") * sensitivity;

        //CurrentYの値をAngleMINとAngleMAXの間に制限する
        //Mathf.Clamp：値を指定された範囲の中にとどめる
        //すなわち、Y軸を極端な角度に回転させることを防ぐ
        currentY = Mathf.Clamp(currentY, angleMIN, angleMAX);
    }

    private void RotatePlayer()
    {
        //プレイヤーの回転をカメラの回転に合わせる
        player.rotation = Quaternion.Euler(0, currentX, 0);
    }

    private void CameraMove()
    {
        //-distance：プレイヤーの後ろに配置
        Vector3 direction = new Vector3(0, 0, -distance);

        //CurrentYとCurrentXを使って回転を計算
        //Quaternion.Euler：ｚ、ｘ、ｙの順で回転する
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        //カメラ位置をプレイヤー位置と回転、オフセットを基に設定（通常の位置）
        Vector3 desiredPosition = player.position + rotation * direction + offset;

        //Physics.SphereCast：プレイヤーから理想的な位置（desiredPosition）に向かって球を飛ばし、障害物を検知
        //player.position：SphereCast開始位置
        //sphereCastRadius：SphereCastの半径
        //desiredPosition - player.position：SphereCastの方向
        RaycastHit hit;
        if (Physics.SphereCast(player.position, sphereCastRadius, desiredPosition - player.position, out hit, distance, sollisionLayers))
        {
            //障害物が検出された場合、カメラを障害物の手前に配置
            //hit.point：衝突地点
            //hit.normal：衝突のベクトル
            transform.position = hit.point + hit.normal * sphereCastRadius;
        }
        else
        {
            //障害物がない場合、通常の位置にカメラを配置
            transform.position = desiredPosition;
        }

        //transform.LookAt：カメラに特定の位置を向かせ続ける
        //カメラがプレイヤーの位置を向くよう設定
        transform.LookAt(player.position + offset);
    }
}