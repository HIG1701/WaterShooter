using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/SelectCharKeep")]
public class SelectCharKeep : ScriptableObject
{
    //�I����ʂɂ�����L�����̑I��ύX���e�A������e��ێ��H
    [SerializeField] public GameObject charactorObj;
    [SerializeField] public string charactorName;
}
