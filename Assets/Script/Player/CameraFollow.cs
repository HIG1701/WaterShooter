using UnityEngine;

//このスクリプトでは、マウスがプレイヤーを中心に追従するような処理を実装する
//参考リンク：https://chamucode.com/unity-camera-follow/
/// <summary>
/// カメラに関するクラス
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [SerializeField] Rigidbody Player;
    [SerializeField] Vector3 Offset;
    
    [Header("感度")] public float MouseSensitivity = 5f; // マウス感度1~10.
    private float Sensitivity;

    [SerializeField] float Distance = 5;
    [SerializeField] float SphereCastRadius = 0.5f;                 //SphereCastの半径
    [SerializeField] LayerMask CollisionLayers;                     //衝突を検出するレイヤー（インスペクターでWallを指定）

    private float CurrentX = 0f;
    private float CurrentY = 0f;

    private const float AngleMIN = -80f; //Y軸最小値
    private const float AngleMAX = 80f; //Y軸最大値

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;                   //カーソルロック
        Cursor.visible = false;                                     //カーソル非表示
    }

    private void Update()
    {
        Sensitivity = Mathf.Lerp(0.1f, 5f, MouseSensitivity / 10f);
        GetMousePos();
        RotatePlayer();
    }

    private void LateUpdate()
    {
        CameraMove();
    }

    private void GetMousePos()
    {
        CurrentX += Input.GetAxis("Mouse X") * Sensitivity;
        CurrentY -= Input.GetAxis("Mouse Y") * Sensitivity;

        //CurrentYの値をAngleMINとAngleMAXの間に制限する
        //Mathf.Clamp：値を指定された範囲の中にとどめる
        CurrentY = Mathf.Clamp(CurrentY, AngleMIN, AngleMAX);
    }

    private void RotatePlayer()
    {
        Player.MoveRotation(Quaternion.Euler(0, CurrentX, 0));
    }

    private void CameraMove()
    {
        //プレイヤーの後ろに配置
        Vector3 Direction = new Vector3(0, 0, -Distance);

        //Quaternion.Euler：ｚ、ｘ、ｙの順で回転する
        Quaternion Rotation = Quaternion.Euler(CurrentY, CurrentX, 0);

        float cameraDistance = Distance;

        //カメラ位置をプレイヤー位置と回転、オフセットを基に設定（通常の位置）
        Vector3 DesiredPosition = Player.transform.position + Rotation * Direction + Offset;

        //Physics.SphereCast：障害物を検知
        //SphereCastRadius：SphereCastの半径
        //DesiredPosition - Player.position：SphereCastの方向
        if (Physics.SphereCast(Player.transform.position + Offset, SphereCastRadius, Rotation * -Vector3.forward, out RaycastHit hit, Distance, CollisionLayers))
        {
            cameraDistance = Mathf.Min(Distance, hit.distance); // 壁が近ければ距離を短縮
        }

        // カメラ位置を調整
        DesiredPosition = Player.transform.position + Rotation * new Vector3(0, 0, -cameraDistance) + Offset;

        transform.position = DesiredPosition;
        transform.LookAt(Player.position + Rotation * Offset);
    }
}
