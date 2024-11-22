using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/CharBox")]
public class CharBoxList : ScriptableObject
{
    [System.Serializable]
    public class charClass
    {
        public GameObject charPrefab;
        public string charName;
    }
    public List<charClass> charBox = new List<charClass>();       //Player‚ÌPrefabŠÇ—‚·‚éList
    public List<Transform> posBox = new List<Transform>();          //Player‚ÌTransformŠÇ—‚·‚éList
}
