using UnityEngine;

/// <summary>
/// �J�������[�N�Ɋւ���N���X
/// </summary>
//���̃X�N���v�g�ł́A�}�E�X���v���C���[�𒆐S�ɒǏ]����悤�ȏ�������������
//�Q�l�����N�Fhttps://chamucode.com/unity-camera-follow/

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;
    [SerializeField] float sensitivity = 5f;
    [SerializeField] float distance = 5f;
    [SerializeField] float sphereCastRadius = 0.5f;                 //SphereCast�̔��a
    [SerializeField] LayerMask sollisionLayers;                     //�Փ˂����o���郌�C���[�i�C���X�y�N�^�[��Wall���w��j

    private float currentX = 0f;
    private float currentY = 0f;

    //���������̉�]�p�x�̐���
    private const float angleMIN = -5f;
    private const float angleMAX = 80f;

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
        currentX += Input.GetAxis("Mouse X") * sensitivity;
        currentY -= Input.GetAxis("Mouse Y") * sensitivity;

        //CurrentY�̒l��AngleMIN��AngleMAX�̊Ԃɐ�������
        //Mathf.Clamp�F�l���w�肳�ꂽ�͈͂̒��ɂƂǂ߂�
        //���Ȃ킿�AY�����ɒ[�Ȋp�x�ɉ�]�����邱�Ƃ�h��
        currentY = Mathf.Clamp(currentY, angleMIN, angleMAX);
    }

    private void RotatePlayer()
    {
        //�v���C���[�̉�]���J�����̉�]�ɍ��킹��
        player.rotation = Quaternion.Euler(0, currentX, 0);
    }

    private void CameraMove()
    {
        //-distance�F�v���C���[�̌��ɔz�u
        Vector3 direction = new Vector3(0, 0, -distance);

        //CurrentY��CurrentX���g���ĉ�]���v�Z
        //Quaternion.Euler�F���A���A���̏��ŉ�]����
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        //�J�����ʒu���v���C���[�ʒu�Ɖ�]�A�I�t�Z�b�g����ɐݒ�i�ʏ�̈ʒu�j
        Vector3 desiredPosition = player.position + rotation * direction + offset;

        //Physics.SphereCast�F�v���C���[���痝�z�I�Ȉʒu�idesiredPosition�j�Ɍ������ċ����΂��A��Q�������m
        //player.position�FSphereCast�J�n�ʒu
        //sphereCastRadius�FSphereCast�̔��a
        //desiredPosition - player.position�FSphereCast�̕���
        RaycastHit hit;
        if (Physics.SphereCast(player.position, sphereCastRadius, desiredPosition - player.position, out hit, distance, sollisionLayers))
        {
            //��Q�������o���ꂽ�ꍇ�A�J��������Q���̎�O�ɔz�u
            //hit.point�F�Փ˒n�_
            //hit.normal�F�Փ˂̃x�N�g��
            transform.position = hit.point + hit.normal * sphereCastRadius;
        }
        else
        {
            //��Q�����Ȃ��ꍇ�A�ʏ�̈ʒu�ɃJ������z�u
            transform.position = desiredPosition;
        }

        //transform.LookAt�F�J�����ɓ���̈ʒu��������������
        //�J�������v���C���[�̈ʒu�������悤�ݒ�
        transform.LookAt(player.position + offset);
    }
}