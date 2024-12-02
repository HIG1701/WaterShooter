using UnityEngine;

/// <summary>
/// 弾丸に関するクラス
/// </summary>
[RequireComponent(typeof(Rigidbody))] //リジッドボディ必須
public class Bullet : MonoBehaviour
{
    [SerializeField] private GunParameter gunParameter;
    private float speed;                                           //弾丸の速度
    private float range;                                           //射程距離
    private Vector3 startPosition;
    private Rigidbody rb;

    private void Start()
    {
        speed = gunParameter.BulletSpeed;
        range = gunParameter.AttackRange;
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();

        //弾丸に瞬間的な力を加えて発射する
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        //射程距離に達したら弾丸を破壊する
        if (Vector3.Distance(startPosition, transform.position) >= range) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}