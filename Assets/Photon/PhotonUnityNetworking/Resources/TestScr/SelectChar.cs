using UnityEngine;

public class SelectChar : MonoBehaviour
{
    [SerializeField] CreateRoom room;
    [SerializeField] string charName;
    [SerializeField] GameObject selectObj;

    public void OnClick()
    {
        Debug.Log("キャラクターを選択");
        selectObj.SetActive(false);
        room.charKind = charName;
    }
}
