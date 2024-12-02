using UnityEngine;

/// <summary>
/// 弾丸に関するクラス
/// </summary>
[RequireComponent(typeof(Rigidbody))] //リジッドボディ必須
//TODO;リジッドボディの影響か、プレイヤーが前方向に加速する
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

        //弾丸の初速度を設定
        rb.velocity = transform.forward * speed;
    }

    private void FixedUpdate()
    {
        //射程距離に達したら弾丸を破壊する
        if (Vector3.Distance(startPosition, transform.position) >= range) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //障害物に当たったら弾丸を破壊する
        Destroy(gameObject);
    }
}