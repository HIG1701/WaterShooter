using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform Player;                              //�v���C���[��Transform
    [SerializeField] Vector3 Offset;                                //�J�����̃I�t�Z�b�g
    [SerializeField] float Sensitivity = 5f;                        //�}�E�X���x
    [SerializeField] float Distance = 5f;                           //�v���C���[����̋���

    private float CurrentX = 0f;
    private float CurrentY = 0f;

    //���������̉�]�p�x�̐���
    private const float AngleMIN = -20f;
    private const float AngleMAX = 80f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;                   //�J�[�\�����b�N
        Cursor.visible = false;                                     //�J�[�\����\��
    }

    private void Update()
    {
        CurrentX += Input.GetAxis("Mouse X") * Sensitivity;
        CurrentY -= Input.GetAxis("Mouse Y") * Sensitivity;

        //CurrentY�̒l��AngleMIN��AngleMAX�̊Ԃɐ�������
        CurrentY = Mathf.Clamp(CurrentY, AngleMIN, AngleMAX);
    }

    private void LateUpdate()
    {
        Vector3 direction = new Vector3(0, 0, -Distance);
        //CurrentY��CurrentX���g���ĉ�]���v�Z
        Quaternion rotation = Quaternion.Euler(CurrentY, CurrentX, 0);
        //�J�����ʒu���v���C���[�ʒu�Ɖ�]�A�I�t�Z�b�g����ɐݒ�
        transform.position = Player.position + rotation * direction + Offset;
        //�J�������v���C���[�̈ʒu�������悤�ݒ�
        transform.LookAt(Player.position + Offset);
    }
}