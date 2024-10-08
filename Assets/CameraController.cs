using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;


    private void Update()
    {
        MoveCamera();                       //�J����
    }
    private void MoveCamera()
    {
        float mx = Input.GetAxis("Mouse X");                            //�J�[�\���̉��̈ړ��ʂ��擾
        float my = Input.GetAxis("Mouse Y");                            //�J�[�\���̏c�̈ړ��ʂ��擾

        if (Mathf.Abs(mx) > 0.001f)                                     //X�����Ɉ��ʈړ����Ă���Ή���]
        {
            //transform.RotateAround(��]�̒��S, ��]�̎��iVector3.up��(0,1,0)�̂��ƂȂ̂ł��������Ƃ��Ă���j, �ω���); 
            transform.RotateAround(transform.position, Vector3.up, mx); //��]����player�I�u�W�F�N�g�̃��[���h���WY��
        }

        if (Mathf.Abs(my) > 0.001f)                                     //Y�����Ɉ��ʈړ����Ă���Ώc��]
        {
            transform.RotateAround(transform.position, Vector3.right, -my);
        }
    }
}