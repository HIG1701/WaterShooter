using UnityEngine;

/// <summary>
/// コインに関するクラス
/// </summary>
public class CoinScript : MonoBehaviour
{
    [SerializeField] private readonly int coin;

    public int Coin => coin;
}