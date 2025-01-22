using System.Collections.Generic;
using UnityEngine;

public class ShuffleSpawn : MonoBehaviour
{
    [SerializeField] int element;
    private List<int> shuffle = new List<int> ();

    private void Awake()
    {
        for (int i = 0; i < element; i++)
        {
            shuffle.Add(i);
        }
        shuffle = Shuffle(shuffle);
    }
    private List<int> Shuffle(List<int> Index)
    {
        for (int i = 0; i < Index.Count; i++)
        {
            int Temp = Index[i];
            int RandomIndex = Random.Range(i, Index.Count);

            //“ü‚ê‘Ö‚¦ˆ—
            Index[i] = Index[RandomIndex];
            Index[RandomIndex] = Temp;
        }
        return Index;
    }
    public List<int> ShuffleSpawnIdx()
    {
        return shuffle;
    }
}
