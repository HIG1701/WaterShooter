using UnityEngine;

//���̃X�N���v�g�ł́A�}�E�X���v���C���[�𒆐S�ɒǏ]����悤�ȏ�������������
//�Q�l�����N�Fhttps://chamucode.com/unity-camera-follow/
/// <summary>
/// �J�����Ɋւ���N���X
/// </summary>

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] Vector3 Offset;
    [SerializeField] float Sensitivity = 5f;
    [SerializeField] float Distance = 5;
    [SerializeField] float SphereCastRadius = 0.5f;                 //SphereCast�̔��a
    [SerializeField] LayerMask CollisionLayers;                     //�Փ˂����o���郌�C���[�i�C���X�y�N�^�[��Wall���w��j

    private float CurrentX = 0f;
    private float CurrentY = 0f;

    private const float AngleMIN = -5f; //Y���ŏ��l
    private const float AngleMAX = 80f; //Y���ő�l

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;                   //�J�[�\�����b�N
        Cursor.visible = false;                                     //�J�[�\����\��
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

        //CurrentY�̒l��AngleMIN��AngleMAX�̊Ԃɐ�������
        //Mathf.Clamp�F�l���w�肳�ꂽ�͈͂̒��ɂƂǂ߂�
        CurrentY = Mathf.Clamp(CurrentY, AngleMIN, AngleMAX);
    }

    private void RotatePlayer()
    {
        Player.rotation = Quaternion.Euler(0, CurrentX, 0);
    }

    private void CameraMove()
    {
        //�v���C���[�̌��ɔz�u
        Vector3 Direction = new Vector3(0, 0, -Distance);

        //Quaternion.Euler�F���A���A���̏��ŉ�]����
        Quaternion Rotation = Quaternion.Euler(CurrentY, CurrentX, 0);

        //�J�����ʒu���v���C���[�ʒu�Ɖ�]�A�I�t�Z�b�g����ɐݒ�i�ʏ�̈ʒu�j
        Vector3 DesiredPosition = Player.position + Rotation * Direction + Offset;

        //Physics.SphereCast�F��Q�������m
        //SphereCastRadius�FSphereCast�̔��a
        //DesiredPosition - Player.position�FSphereCast�̕���
        RaycastHit hit;
        if (Physics.SphereCast(Player.position, SphereCastRadius, DesiredPosition - Player.position, out hit, Distance, CollisionLayers))
        {
            //hit.point�F�Փ˒n�_
            //hit.normal�F�Փ˂̃x�N�g��
            transform.position = hit.point + hit.normal * SphereCastRadius;
        }
        else transform.position = DesiredPosition;

        //transform.LookAt�F�J�����ɓ���̈ʒu��������������
        transform.LookAt(Player.position + Offset);
    }
}