using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GunParameter gunParameter;
    private float speed;                                           //’eŠÛ‚Ì‘¬“x
    private float range;                                           //Ë’ö‹——£
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
        //’eŠÛ‚ğ‘O•û‚ÉˆÚ“®‚³‚¹‚é
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        //Ë’ö‹——£‚É’B‚µ‚½‚ç’eŠÛ‚ğ”j‰ó‚·‚é
        if (Vector3.Distance(startPosition, transform.position) >= range) Destroy(gameObject);

    }

    void OnCollisionEnter(Collision collision)
    {
        // áŠQ•¨‚É“–‚½‚Á‚½‚ç’eŠÛ‚ğ”j‰ó‚·‚é
        Destroy(gameObject);
    }
}