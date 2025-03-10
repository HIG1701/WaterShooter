using UnityEngine;

/// <summary>
/// 弾丸に関するクラス
/// </summary>
[RequireComponent(typeof(Rigidbody))] //リジッドボディ必須
//TODO;リジッドボディの影響か、プレイヤーが前方向に加速する
public class Bullet : MonoBehaviour
{
    public int CharaIndex = 0;
    [SerializeField] private GunParameter gunParameter;
    [SerializeField] private GameObject hitParticle;
    [SerializeField] private GameObject muzzleFlash;
    private float speed;                                           //弾丸の速度
    private float range;                                           //射程距離
    private Vector3 startPosition;
    private Rigidbody rb;
    private GameObject particleFlash;

    private void Start()
    {
        speed = gunParameter.BulletSpeed;
        range = gunParameter.AttackRange;
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        particleFlash = Instantiate(muzzleFlash, gameObject.transform.position, transform.rotation);
        particleFlash.GetComponent<ParticleSystem>().Play();

        //弾丸の初速度を設定
        rb.velocity = transform.forward * speed;
    }



    private void FixedUpdate()
    {
        //射程距離に達したら弾丸を破壊する
        if (Vector3.Distance(startPosition, transform.position) >= range)
        {
            particleFlash.GetComponent<ParticleSystem>().Stop();
            Destroy(particleFlash);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.TryGetComponent<HeadShoot>(out HeadShoot headShoot))
        //{
        //    headShoot.HeadHit(gunParameter, CharaIndex, hitParticle);
        //    return;
        //}

        //障害物に当たったら弾丸を破壊する
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            //プレイヤーに対してダメ加算.
            SetDateManager.HitToCharacter(playerController, gunParameter, CharaIndex);
        }
        else if (collision.gameObject.TryGetComponent<AIControl>(out AIControl playerAiController))
        {
            //BOTに対してダメ加算.
            SetDateManager.HitToCharacter(playerAiController, gunParameter, CharaIndex);
            GameObject particleInstance = Instantiate(hitParticle, gameObject.transform.position, Quaternion.identity);
            particleInstance.GetComponentInChildren<ParticleSystem>().Play();
        }
        Destroy(particleFlash);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collision)
    {
        //障害物に当たったら弾丸を破壊する
        Debug.Log(collision.gameObject);
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            //プレイヤーに対してダメ加算.
            SetDateManager.HitToCharacter(playerController, gunParameter, CharaIndex);
        }
        else if (collision.gameObject.TryGetComponent<AIControl>(out AIControl playerAiController))
        {
            //BOTに対してダメ加算.
            SetDateManager.HitToCharacter(playerAiController, gunParameter, CharaIndex);
            GameObject particleInstance = Instantiate(hitParticle, gameObject.transform.position, Quaternion.identity);
            particleInstance.GetComponentInChildren<ParticleSystem>().Play();
        }
        particleFlash.GetComponent<ParticleSystem>().Stop();
        Destroy(particleFlash);
        Destroy(gameObject);
    }
}