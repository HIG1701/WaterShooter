using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public int coinCount = 10;
    public float area = 10f;
    public float maxRenge = 5.0f;
    public int maxEntry = 10;
    public float heightOffset = 0.1f;
    public List<GameObject> coinList = new List<GameObject>();
    [SerializeField] private float resetTime = 60f;
    private bool isCoolTime;

    void Start()
    {
        for (int i = 0; i < coinCount; i++)
        {
            SpawnCoin();
        }
        //StartCoroutine(CoinReset());
    }

    private void FixedUpdate()
    {
        StartCoroutine(CoinReset());
    }

    private IEnumerator CoinReset()
    {
        if (isCoolTime) { yield break; }
        isCoolTime = true;
        yield return new WaitForSeconds(resetTime);
        CoinListReset();
        for (int i = 0; i < coinCount; i++)
        {
            SpawnCoin();
        }
        isCoolTime = false;
    }

    private void CoinListReset()
    {
        coinList.Clear();
    }

    void SpawnCoin()
    {
        for (int entry = 0; entry < maxEntry; entry++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(-area, area),
                10f, //‰¼’u‚«.
                Random.Range(-area, area)
            );

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPosition, out hit, maxRenge, NavMesh.AllAreas))
            {
                Vector3 spawnPosition = hit.position;

                if (Physics.Raycast(spawnPosition + Vector3.up * 5f, Vector3.down, out RaycastHit rayHit, 10f))
                {
                    spawnPosition.y = rayHit.point.y + heightOffset;
                }

                coinList.Add(Instantiate(coinPrefab, spawnPosition, coinPrefab.transform.rotation));
                return;
            }
        }
    }
}