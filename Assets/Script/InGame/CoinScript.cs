using UnityEngine;

//コイン関連の処理を書きます。

public class CoinScript : MonoBehaviour
{
    //パブリックだけど変更してほしくないので隠してます
    [SerializeField] private int coin;

    public int Coin => coin;
}