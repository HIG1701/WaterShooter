using UnityEngine;

//�ꉞ�c���܂����A�g��Ȃ�����

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;                     //player�̃Q�[���I�u�W�F�N�g������ϐ���ݒ�
    private void Update()
    {
        float my = Input.GetAxis("Mouse Y");                //�}�E�X�̏c�����̈ړ��ʂ��擾

        if (Mathf.Abs(my) > 0.001f)                         //Y�����Ɉ��ʈړ����Ă���Ώc��]
        {
            //transform.RotateAround(player.transform.position, transform.right, -my);
            transform.RotateAround(player.transform.position, Vector3.right, -my);
        }
    }
}