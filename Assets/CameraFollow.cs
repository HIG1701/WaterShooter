using UnityEngine;

//このスクリプトでは、マウスがプレイヤーを中心に追従するような処理を実装する
//参考リンク：https://chamucode.com/unity-camera-follow/

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform Player;                              //プレイヤーのTransform
    [SerializeField] Vector3 Offset;                                //カメラのオフセット
    [SerializeField] float Sensitivity = 5f;                        //マウス感度
    [SerializeField] float Distance = 5f;                           //プレイヤーからの距離
    [SerializeField] float SphereCastRadius = 0.5f;                 //SphereCastの半径
    [SerializeField] LayerMask CollisionLayers;                     //衝突を検出するレイヤー（インスペクターでWallを指定）


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
        GetMouthPos();
    }

    private void LateUpdate()
    {
        CameraMove();
    }

    private void GetMouthPos()
    {
        CurrentX += Input.GetAxis("Mouse X") * Sensitivity;         //マウスX軸取得
        CurrentY -= Input.GetAxis("Mouse Y") * Sensitivity;         //マウスY軸取得

        //CurrentYの値をAngleMINとAngleMAXの間に制限する
        //Mathf.Clamp：値を指定された範囲の中にとどめる
        //すなわち、Y軸を極端な角度に回転させることを防ぐ
        CurrentY = Mathf.Clamp(CurrentY, AngleMIN, AngleMAX);
    }

    private void CameraMove()
    {
        //-Distance：プレイヤーの後ろに配置
        Vector3 Direction = new Vector3(0, 0, -Distance);

        //CurrentYとCurrentXを使って回転を計算
        //Quaternion.Euler：ｚ、ｘ、ｙの順で回転する
        Quaternion Rotation = Quaternion.Euler(CurrentY, CurrentX, 0);

        //カメラ位置をプレイヤー位置と回転、オフセットを基に設定（通常の位置）
        Vector3 DesiredPosition = Player.position + Rotation * Direction + Offset;

        //Physics.SphereCast：プレイヤーから理想的な位置（DesiredPosition）に向かって球を飛ばし、障害物を検知
        //Player.position：SphereCast開始位置
        //SphereCastRadius：SphereCastの半径
        //DesiredPosition - Player.position：SphereCastの方向
        RaycastHit hit;
        if (Physics.SphereCast(Player.position, SphereCastRadius, DesiredPosition - Player.position, out hit, Distance, CollisionLayers))
        {
            //障害物が検出された場合、カメラを障害物の手前に配置
            //hit.point：衝突地点
            //hit.normal：衝突のベクトル
            transform.position = hit.point + hit.normal * SphereCastRadius;
        }
        else
        {
            //障害物がない場合、通常の位置にカメラを配置
            transform.position = DesiredPosition;
        }

        //transform.LookAt：カメラに特定の位置を向かせ続ける
        //カメラがプレイヤーの位置を向くよう設定
        transform.LookAt(Player.position + Offset);
    }
}