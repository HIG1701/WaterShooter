using UnityEngine;

/// <summary>
/// 弾丸に関するクラス
/// </summary>
//TODO:リジッドボディとTrigger

public class Bullet : MonoBehaviour
{
    [SerializeField] private GunParameter gunParameter;
    private float speed;                                           //弾丸の速度
    private float range;                                           //射程距離
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

    private void AdvanceBullet()
    {
        //弾丸を前方に移動させる
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        //射程距離に達したら弾丸を破壊する
        if (Vector3.Distance(startPosition, transform.position) >= range) Destroy(gameObject);

    }

    private void OnCollisionEnter(Collision collision)
    {
        // 障害物に当たったら弾丸を破壊する
        Destroy(gameObject);
    }
}