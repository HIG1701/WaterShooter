using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float MoveSpeedIn = 30.0f;                             //プレイヤー移動速度（入力）
    [SerializeField] private float jumpForce = 20f;                                 //プレイヤージャンプ力（入力）
    private Rigidbody PlayerRb;                                                     //プレイヤーRigidbody
    private Vector3 MoveSpeed;                                                      //プレイヤー移動速度
    private Vector3 CurrentPos;                                                     //プレイヤー現在位置
    private Vector3 PastPos;                                                        //プレイヤー過去位置
    private Vector3 Delta;                                                          //プレイヤー移動量
    private Quaternion PlayerRot;                                                   //プレイヤー進行方向
    private float CurrentAngularVel;                                                //現在回転角速度
    [SerializeField] private float MaxAngularVel = Mathf.Infinity;                  //最大回転角速度[deg/s]
    [SerializeField] private float SmoothTime = 0.1f;                               //進行にかかる時間[s]
    private float DistanceAngle;                                                    //現在の向きと進行方向角度
    private float RotAngle;                                                         //現在の回転角度
    private Quaternion NextRot;                                                     //どの程度回転するか
    private bool isGrounded;                                                        //地面との設置判定

    private void Awake()
    {
        PlayerRb = GetComponent<Rigidbody>();
        PastPos = transform.position;
    }

    private void Update()
    {
        Dash();
        Jump();
        MovePlayer();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        //前方取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        //右方取得
        Vector3 cameraRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;

        //初期化
        MoveSpeed = Vector3.zero;

        //移動入力
        if (Input.GetKey(KeyCode.W)) MoveSpeed = MoveSpeedIn * cameraForward;
        if (Input.GetKey(KeyCode.A)) MoveSpeed = -MoveSpeedIn * cameraRight;
        if (Input.GetKey(KeyCode.S)) MoveSpeed = -MoveSpeedIn * cameraForward;
        if (Input.GetKey(KeyCode.D)) MoveSpeed = MoveSpeedIn * cameraRight;
        PlayerRb.velocity = MoveSpeed;
    }

    private void RotatePlayer()
    {
        //現在の位置
        CurrentPos = transform.position;
        //移動量計算
        Delta = CurrentPos - PastPos;
        Delta.y = 0;
        //過去の位置の更新
        PastPos = CurrentPos;

        if (Delta == Vector3.zero) return;
        PlayerRot = Quaternion.LookRotation(Delta, Vector3.up);
        DistanceAngle = Vector3.Angle(transform.forward, Delta);

        //Vector3.SmoothDamp (現在地, 目的地, ref 現在の速度, 遷移時間, 最高速度);
        RotAngle = Mathf.SmoothDampAngle(0, DistanceAngle, ref CurrentAngularVel, SmoothTime, MaxAngularVel);
        NextRot = Quaternion.RotateTowards(transform.rotation, PlayerRot, RotAngle);
        transform.rotation = NextRot;
    }

    private void Dash()
    {
        if (Input.GetKey(KeyCode.LeftShift)) MoveSpeedIn = 60.0f;
        else MoveSpeedIn = 30.0f;
    }

    private void Jump()
    {
        //�n�ʂƐG��Ă���
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            PlayerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
