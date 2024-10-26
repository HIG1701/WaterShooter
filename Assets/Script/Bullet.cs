using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float IgnoreCollisionTime = 1f;       //”­ËŒãÕ“Ë–³‹ŠÔi•bj
    private float SpawnTime;

    private void Start()
    {
        SpawnTime = Time.time;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //”­Ë’¼Œã‚ÌÕ“Ë‚ğ–³‹‚·‚é
        if (Time.time - SpawnTime < IgnoreCollisionTime) return;

        //Õ“Ë‚É’e‚ğÁ‚·
        Destroy(gameObject);
    }
}