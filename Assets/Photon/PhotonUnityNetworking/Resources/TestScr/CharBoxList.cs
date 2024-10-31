using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/CharBox")]
public class CharBoxList : ScriptableObject
{
    public List<GameObject> charBox = new List<GameObject>();
    public string setCharName;
}
