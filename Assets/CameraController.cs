using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject Player;
    private Vector3 CurrentPos;             //���݂̃J�����ʒu
    private Vector3 PastPos;                //�ߋ��̃J�����ʒu
    private Vector3 Distance;                   //�ړ�����

    private void Start()
    {
        //�ŏ��̃v���C���[�̈ʒu�̎擾
        PastPos = Player.transform.position;
    }
    private void Update()
    {
        MoveCamera();
        RotateCamera();
    }

    private void MoveCamera()
    {
        //�v���C���[�̌��ݒn�̎擾
        CurrentPos = Player.transform.position;
        Distance = CurrentPos - PastPos;
        transform.position = Vector3.Lerp(transform.position, transform.position + Distance, 1.0f);
        PastPos = CurrentPos;
    }

    private void RotateCamera()
    {
        //�}�E�X�ړ�
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(mx) > 0.01f)
            //Y���W
            transform.RotateAround(Player.transform.position, Vector3.up, mx);
        if (Mathf.Abs(my) > 0.01f)
            //X���W
            transform.RotateAround(Player.transform.position, transform.right, -my);
    }
}