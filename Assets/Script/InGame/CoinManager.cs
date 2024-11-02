using UnityEngine;

//コイン関連の処理を書きます。

public class CoinManager : MonoBehaviour
{
    //パブリックだけど変更してほしくないので隠してます
    [HideInInspector] public int Coin;

    private void Start()
    {
        //コインの種類に応じて値を設定
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