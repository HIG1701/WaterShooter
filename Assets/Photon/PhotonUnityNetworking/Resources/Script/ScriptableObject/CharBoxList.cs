using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/CharBox")]
public class CharBoxList : ScriptableObject
{
    public List<GameObject> charBox = new List<GameObject>();       //Player‚ÌPrefabŠÇ—‚·‚éList
    public List<Transform> posBox = new List<Transform>();          //Player‚ÌTransformŠÇ—‚·‚éList
}
