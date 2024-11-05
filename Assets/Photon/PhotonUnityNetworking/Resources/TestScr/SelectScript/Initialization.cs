using UnityEngine;

public class Initialization : MonoBehaviour
{
    [SerializeField] SelectCharKeep selectCharKeep;

    private void Start()
    {
        selectCharKeep.charactorObj = null;
        selectCharKeep.charactorName = null;
    }
}
