using UnityEngine;

public class SelectChar : MonoBehaviour
{
    [SerializeField] CreateRoom room;
    [SerializeField] string charName;
    [SerializeField] GameObject selectObj;

    public void OnClick()
    {
        Debug.Log("�L�����N�^�[��I��");
        selectObj.SetActive(false);
        room.charKind = charName;
    }
}
