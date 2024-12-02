using UnityEngine;

/// <summary>
/// �e�ۂɊւ���N���X
/// </summary>
[RequireComponent(typeof(Rigidbody))] //���W�b�h�{�f�B�K�{
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

        //�e�ۂɏu�ԓI�ȗ͂������Ĕ��˂���
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        //�˒������ɒB������e�ۂ�j�󂷂�
        if (Vector3.Distance(startPosition, transform.position) >= range) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}