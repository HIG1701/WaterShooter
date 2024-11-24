using UnityEngine;

/// <summary>
/// コインに関するクラス
/// </summary>

public class CoinScript : MonoBehaviour
{
    [SerializeField] private int coin;

    public int Coin => coin;
}