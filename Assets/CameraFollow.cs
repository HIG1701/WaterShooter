using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // �v���C���[��Transform
    public Vector3 offset; // �J�����̃I�t�Z�b�g
    public float sensitivity = 5f; // �}�E�X���x
    public float distance = 5f; // �v���C���[����̋���

    private float currentX = 0f;
    private float currentY = 0f;
    private const float Y_ANGLE_MIN = -20f;
    private const float Y_ANGLE_MAX = 80f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // �J�[�\�������b�N
        Cursor.visible = false; // �J�[�\�����\��
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