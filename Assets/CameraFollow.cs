using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // プレイヤーのTransform
    public Vector3 offset; // カメラのオフセット
    public float sensitivity = 5f; // マウス感度
    public float distance = 5f; // プレイヤーからの距離

    private float currentX = 0f;
    private float currentY = 0f;
    private const float Y_ANGLE_MIN = -20f;
    private const float Y_ANGLE_MAX = 80f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // カーソルをロック
        Cursor.visible = false; // カーソルを非表示
    }

    void Update()
    {
        currentX += Input.GetAxis("Mouse X") * sensitivity;
        currentY -= Input.GetAxis("Mouse Y") * sensitivity;

        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }

    void LateUpdate()
    {
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = player.position + rotation * direction + offset;
        transform.LookAt(player.position + offset);
    }
}