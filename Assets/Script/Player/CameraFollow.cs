using UnityEngine;

//���̃X�N���v�g�ł́A�}�E�X���v���C���[�𒆐S�ɒǏ]����悤�ȏ�������������
//�Q�l�����N�Fhttps://chamucode.com/unity-camera-follow/

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform Player;                              //�v���C���[��Transform
    [SerializeField] private Vector3 Offset;                                //�J�����̃I�t�Z�b�g
    [SerializeField] private float Sensitivity;                             //�}�E�X���x
    [SerializeField] private float Distance;                                //�v���C���[����̋���
    [SerializeField] private const float SphereCastRadius = 0.5f;           //SphereCast�̔��a
    [SerializeField] private LayerMask CollisionLayers;                     //�Փ˂����o���郌�C���[�i�C���X�y�N�^�[��Wall���w��j
    //���݂̂w�C�x��������
    private float CurrentX;
    private float CurrentY;
    //���������̉�]�p�x�̐���
    //const�ŕύX�ł��Ȃ��悤�ɂ��Ă���
    private const float AngleMIN = -5f;
    private const float AngleMAX = 80f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;                   //�J�[�\�����b�N
        Cursor.visible = false;                                     //�J�[�\����\��
        Sensitivity = 5f;                                           //���x�ݒ�
        Distance = 5f;                                              //�����ݒ�
        CurrentX = 0f;
        CurrentY = 0f;

        //�J�����̃t�B�[���h�I�u�r���[�iFOV�j��ݒ�
        //Camera.main.fieldOfView�F�ȒP�Ɍ����΃J�����̎���p�������鎖���ł��邼�I�I
        //Camera.main.fieldOfView = 90;
    }

    private void Update()
    {
        GetMousePos();
        RotatePlayerWithCamera();
    }

    private void LateUpdate()
    {
        CameraMove();
    }

    private void GetMousePos()
    {
        CurrentX += Input.GetAxis("Mouse X") * Sensitivity;         //�}�E�XX���擾
        CurrentY -= Input.GetAxis("Mouse Y") * Sensitivity;         //�}�E�XY���擾

        //CurrentY�̒l��AngleMIN��AngleMAX�̊Ԃɐ�������
        //Mathf.Clamp�F�l���w�肳�ꂽ�͈͂̒��ɂƂǂ߂�
        //���Ȃ킿�AY�����ɒ[�Ȋp�x�ɉ�]�����邱�Ƃ�h��
        CurrentY = Mathf.Clamp(CurrentY, AngleMIN, AngleMAX);
    }

    private void CameraMove()
    {
        //-Distance�F�v���C���[�̌��ɔz�u
        Vector3 Direction = new Vector3(0, 0, -Distance);

        //CurrentY��CurrentX���g���ĉ�]���v�Z
        //Quaternion.Euler�F���A���A���̏��ŉ�]����
        Quaternion Rotation = Quaternion.Euler(CurrentY, CurrentX, 0);

        //�J�����ʒu���v���C���[�ʒu�Ɖ�]�A�I�t�Z�b�g����ɐݒ�i�ʏ�̈ʒu�j
        Vector3 DesiredPosition = Player.position + Rotation * Direction + Offset;

        //Physics.SphereCast�F�v���C���[���痝�z�I�Ȉʒu�iDesiredPosition�j�Ɍ������ċ����΂��A��Q�������m
        //Player.position�FSphereCast�J�n�ʒu
        //SphereCastRadius�FSphereCast�̔��a
        //DesiredPosition - Player.position�FSphereCast�̕���
        RaycastHit hit;
        if (Physics.SphereCast(Player.position, SphereCastRadius, DesiredPosition - Player.position, out hit, Distance, CollisionLayers))
        {
            //��Q�������o���ꂽ�ꍇ�A�J��������Q���̎�O�ɔz�u
            //hit.point�F�Փ˒n�_
            //hit.normal�F�Փ˂̃x�N�g��
            transform.position = hit.point + hit.normal * SphereCastRadius;
        }
        else
        {
            //��Q�����Ȃ��ꍇ�A�ʏ�̈ʒu�ɃJ������z�u
            transform.position = DesiredPosition;
        }

        //transform.LookAt�F�J�����ɓ���̈ʒu��������������
        //�J�������v���C���[�̈ʒu�������悤�ݒ�
        transform.LookAt(Player.position + Offset);
    }

    //������]�p�x�擾���\�b�h
    public float GetCurrentX()
    {
        return CurrentX;
    }

    //Player�̉�]�ɉ����ăJ��������]������
    private void RotatePlayerWithCamera()
    {
        Player.rotation = Quaternion.Euler(0, CurrentX, 0);
    }
}