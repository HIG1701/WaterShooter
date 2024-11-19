using UnityEngine;

/// <summary>
/// ’eŠÛ‚ÉŠÖ‚·‚éƒNƒ‰ƒX
/// </summary>
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

    /// <summary>
    /// ’eŠÛ‚ğ‘O•û‚Éi‚ß‚é
    /// </summary>
    private void AdvanceBullet()
    {
        //’eŠÛ‚ğ‘O•û‚ÉˆÚ“®‚³‚¹‚é
        //TODO:ƒŠƒWƒbƒhƒ{ƒfƒB‚ÆTrigger
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        //Ë’ö‹——£‚É’B‚µ‚½‚ç’eŠÛ‚ğ”j‰ó‚·‚é
        if (Vector3.Distance(startPosition, transform.position) >= range) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //áŠQ•¨‚É“–‚½‚Á‚½‚ç’eŠÛ‚ğ”j‰ó‚·‚é
        Destroy(gameObject);
    }
}