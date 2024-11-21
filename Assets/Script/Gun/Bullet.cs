using UnityEngine;

/// <summary>
/// �e�ۂɊւ���N���X
/// </summary>
[RequireComponent(typeof(Rigidbody))] //���W�b�h�{�f�B�K�{
//TODO;���W�b�h�{�f�B�̉e�����A�v���C���[���O�����ɉ�������
public class Bullet : MonoBehaviour
{
    [SerializeField] private GunParameter gunParameter;
    private float speed;                                           //�e�ۂ̑��x
    private float range;                                           //�˒�����
    private Vector3 startPosition;
    private Rigidbody rb;

    private void Start()
    {
        speed = gunParameter.BulletSpeed;
        range = gunParameter.AttackRange;
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();

        //�e�ۂ̏����x��ݒ�
        rb.velocity = transform.forward * speed;
    }

    private void FixedUpdate()
    {
        //�˒������ɒB������e�ۂ�j�󂷂�
        if (Vector3.Distance(startPosition, transform.position) >= range) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //��Q���ɓ���������e�ۂ�j�󂷂�
        Destroy(gameObject);
    }
}