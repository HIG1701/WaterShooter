using UnityEngine;

//コイン関連の処理を書きます。

public class CoinManager : MonoBehaviour
{
    [HideInInspector] public int Coin;                  //コインの種類を入力：5 or 20 or 100......
    private void Start()
    {
        // コインの種類に応じて値を設定
        switch (gameObject.name)
        {
            case "5Coin":
                Coin = 5;
                break;
            case "20Coin":
                Coin = 20;
                break;
            case "100Coin":
                Coin = 100;
                break;
            default:
                Coin = 0;
                break;
        }
    }
}