using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float IgnoreCollisionTime = 1f;       //���ˌ�Փ˖������ԁi�b�j
    private float SpawnTime;

    private void Start()
    {
        SpawnTime = Time.time;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //���˒���̏Փ˂𖳎�����
        if (Time.time - SpawnTime < IgnoreCollisionTime) return;

        //�Փˎ��ɒe������
        Destroy(gameObject);
    }
}