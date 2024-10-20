using UnityEngine;

//���̃X�N���v�g�ł́A�}�E�X���v���C���[�𒆐S�ɒǏ]����悤�ȏ�������������
//�Q�l�����N�Fhttps://chamucode.com/unity-camera-follow/

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform Player;                              //�v���C���[��Transform
    [SerializeField] Vector3 Offset;                                //�J�����̃I�t�Z�b�g
    [SerializeField] float Sensitivity = 5f;                        //�}�E�X���x
    [SerializeField] float Distance = 5f;                           //�v���C���[����̋���

    private float CurrentX = 0f;
    private float CurrentY = 0f;

    //���������̉�]�p�x�̐���
    private const float AngleMIN = -5f;
    private const float AngleMAX = 80f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;                   //�J�[�\�����b�N
        Cursor.visible = false;                                     //�J�[�\����\��
    }

    private void Update()
    {
        CurrentX += Input.GetAxis("Mouse X") * Sensitivity;         //�}�E�XX���擾
        CurrentY -= Input.GetAxis("Mouse Y") * Sensitivity;         //�}�E�XY���擾

        //CurrentY�̒l��AngleMIN��AngleMAX�̊Ԃɐ�������
        //Mathf.Clamp�F�l���w�肳�ꂽ�͈͂̒��ɂƂǂ߂�
        //���Ȃ킿�AY�����ɒ[�Ȋp�x�ɉ�]�����邱�Ƃ�h��
        CurrentY = Mathf.Clamp(CurrentY, AngleMIN, AngleMAX);
    }

    private void LateUpdate()
    {
        Vector3 Direction = new Vector3(0, 0, -Distance);

        //CurrentY��CurrentX���g���ĉ�]���v�Z
        Quaternion Rotation = Quaternion.Euler(CurrentY, CurrentX, 0);

        //�J�����ʒu���v���C���[�ʒu�Ɖ�]�A�I�t�Z�b�g����ɐݒ�
        transform.position = Player.position + Rotation * Direction + Offset;

        //transform.LookAt�F�J�����ɓ���̈ʒu��������������
        //�J�������v���C���[�̈ʒu�������悤�ݒ�
        transform.LookAt(Player.position + Offset);
    }
}