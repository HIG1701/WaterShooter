using UnityEngine;

//コイン関連の処理を書きます。

public class CoinManager : MonoBehaviour
{
    [SerializeField] public int Coin;                          //コインの種類を入力：5 or 20 or 100......

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);                        //プレイヤーと接触すると消す
        }
    }
}