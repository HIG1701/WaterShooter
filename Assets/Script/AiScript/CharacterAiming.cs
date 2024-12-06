using UnityEngine;

public class HeadAimController : MonoBehaviour
{
    public Transform head;           // 頭部（head）ボーン
    public float sensitivityX = 2.0f; // 横の感度
    public float sensitivityY = 2.0f; // 縦の感度
    public float minPitch = -45f;     // 上下の角度制限
    public float maxPitch = 45f;      // 上下の角度制限
    public float smoothSpeed = 10f;   // 補間スピード

    private float currentPitch = 0f;  // 現在の上下角度
    private float currentYaw = 0f;    // 現在の左右角度
    private Quaternion initialRotation; // 初期の頭部回転
    private Vector3 initialEulerAngles; // 初期の頭部のオイラー角
    private Vector3 currentEulerAngles; // 現在のオイラー角

    void Start()
    {
        // headボーンが指定されていない場合はエラーを表示
        if (head == null)
        {
            Debug.LogError("Head Transform is not assigned!");
            return;
        }

        // 初期の回転を保存
        initialRotation = head.localRotation;
        initialEulerAngles = head.localEulerAngles;
    }

    void Update()
    {
        // マウス入力の取得
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 回転角度を計算
        currentPitch = Mathf.Clamp(currentPitch - mouseY * sensitivityY, minPitch, maxPitch);
        currentYaw = currentYaw + mouseX * sensitivityX;

        // 現在のオイラー角を計算
        currentEulerAngles = initialEulerAngles;
        currentEulerAngles.x = currentPitch; // 上下
        currentEulerAngles.y = currentYaw;   // 左右

        // スムーズに補間
        Vector3 smoothedEulerAngles = Vector3.Lerp(head.localEulerAngles, currentEulerAngles, Time.deltaTime * smoothSpeed);

        // 新しい回転を設定
        head.localRotation = Quaternion.Euler(smoothedEulerAngles);
    }
}
