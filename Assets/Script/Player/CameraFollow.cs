using UnityEngine;

//このスクリプトでは、マウスがプレイヤーを中心に追従するような処理を実装する
//参考リンク：https://chamucode.com/unity-camera-follow/
/// <summary>
/// カメラに関するクラス
/// </summary>

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] Vector3 Offset;
    [SerializeField] float Sensitivity = 5f;
    [SerializeField] float Distance = 5;
    [SerializeField] float SphereCastRadius = 0.5f;                 //SphereCastの半径
    [SerializeField] LayerMask CollisionLayers;                     //衝突を検出するレイヤー（インスペクターでWallを指定）

    private float CurrentX = 0f;
    private float CurrentY = 0f;

    private const float AngleMIN = -5f; //Y軸最小値
    private const float AngleMAX = 80f; //Y軸最大値

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
        CurrentX += Input.GetAxis("Mouse X") * Sensitivity;
        CurrentY -= Input.GetAxis("Mouse Y") * Sensitivity;

        //CurrentYの値をAngleMINとAngleMAXの間に制限する
        //Mathf.Clamp：値を指定された範囲の中にとどめる
        CurrentY = Mathf.Clamp(CurrentY, AngleMIN, AngleMAX);
    }

    private void RotatePlayer()
    {
        Player.rotation = Quaternion.Euler(0, CurrentX, 0);
    }

    private void CameraMove()
    {
        //プレイヤーの後ろに配置
        Vector3 Direction = new Vector3(0, 0, -Distance);

        //Quaternion.Euler：ｚ、ｘ、ｙの順で回転する
        Quaternion Rotation = Quaternion.Euler(CurrentY, CurrentX, 0);

        //カメラ位置をプレイヤー位置と回転、オフセットを基に設定（通常の位置）
        Vector3 DesiredPosition = Player.position + Rotation * Direction + Offset;

        //Physics.SphereCast：障害物を検知
        //SphereCastRadius：SphereCastの半径
        //DesiredPosition - Player.position：SphereCastの方向
        RaycastHit hit;
        if (Physics.SphereCast(Player.position, SphereCastRadius, DesiredPosition - Player.position, out hit, Distance, CollisionLayers))
        {
            //hit.point：衝突地点
            //hit.normal：衝突のベクトル
            transform.position = hit.point + hit.normal * SphereCastRadius;
        }
        else transform.position = DesiredPosition;

        //transform.LookAt：カメラに特定の位置を向かせ続ける
        transform.LookAt(Player.position + Offset);
    }
}