using UnityEngine;

/// <summary>
/// �e�ۂɊւ���N���X
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] private GunParameter gunParameter;
    private float speed;                                           //�e�ۂ̑��x
    private float range;                                           //�˒�����
    private Vector3 startPosition;

    private void Start()
    {
        speed = gunParameter.BulletSpeed;
        range = gunParameter.AttackRange;
        startPosition = transform.position;
    }
    private void FixedUpdate()
    {
        AdvanceBullet();
    }

    /// <summary>
    /// �e�ۂ�O���ɐi�߂�
    /// </summary>
    private void AdvanceBullet()
    {
        //�e�ۂ�O���Ɉړ�������
        //TODO:���W�b�h�{�f�B��Trigger
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        //�˒������ɒB������e�ۂ�j�󂷂�
        if (Vector3.Distance(startPosition, transform.position) >= range) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //��Q���ɓ���������e�ۂ�j�󂷂�
        Destroy(gameObject);
    }
}