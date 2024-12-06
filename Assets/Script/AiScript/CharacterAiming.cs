using UnityEngine;

public class HeadAimController : MonoBehaviour
{
    public Transform head;           // �����ihead�j�{�[��
    public float sensitivityX = 2.0f; // ���̊��x
    public float sensitivityY = 2.0f; // �c�̊��x
    public float minPitch = -45f;     // �㉺�̊p�x����
    public float maxPitch = 45f;      // �㉺�̊p�x����
    public float smoothSpeed = 10f;   // ��ԃX�s�[�h

    private float currentPitch = 0f;  // ���݂̏㉺�p�x
    private float currentYaw = 0f;    // ���݂̍��E�p�x
    private Quaternion initialRotation; // �����̓�����]
    private Vector3 initialEulerAngles; // �����̓����̃I�C���[�p
    private Vector3 currentEulerAngles; // ���݂̃I�C���[�p

    void Start()
    {
        // head�{�[�����w�肳��Ă��Ȃ��ꍇ�̓G���[��\��
        if (head == null)
        {
            Debug.LogError("Head Transform is not assigned!");
            return;
        }

        // �����̉�]��ۑ�
        initialRotation = head.localRotation;
        initialEulerAngles = head.localEulerAngles;
    }

    void Update()
    {
        // �}�E�X���͂̎擾
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // ��]�p�x���v�Z
        currentPitch = Mathf.Clamp(currentPitch - mouseY * sensitivityY, minPitch, maxPitch);
        currentYaw = currentYaw + mouseX * sensitivityX;

        // ���݂̃I�C���[�p���v�Z
        currentEulerAngles = initialEulerAngles;
        currentEulerAngles.x = currentPitch; // �㉺
        currentEulerAngles.y = currentYaw;   // ���E

        // �X���[�Y�ɕ��
        Vector3 smoothedEulerAngles = Vector3.Lerp(head.localEulerAngles, currentEulerAngles, Time.deltaTime * smoothSpeed);

        // �V������]��ݒ�
        head.localRotation = Quaternion.Euler(smoothedEulerAngles);
    }
}
