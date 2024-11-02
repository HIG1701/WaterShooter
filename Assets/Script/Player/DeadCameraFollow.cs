using UnityEngine;

public class DeadCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform Player;                  //�v���C���[��Transform��ݒ�
    [SerializeField] private Vector3 offset;                    //�J�����ƃv���C���[�̋�����ݒ�

    private void Start()
    {
        //�����I�t�Z�b�g��ݒ�
        offset = transform.position - Player.position;
    }

    private void LateUpdate()
    {
        //�J�����̈ʒu���v���C���[�ɒǏ]������
        transform.position = Player.position + offset;
    }
}