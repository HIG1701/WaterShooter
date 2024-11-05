using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/SelectCharKeep")]
public class SelectCharKeep : ScriptableObject
{
    //選択画面におけるキャラの選択変更内容、決定内容を保持？
    [SerializeField] public GameObject charactorObj;
    [SerializeField] public string charactorName;
}
