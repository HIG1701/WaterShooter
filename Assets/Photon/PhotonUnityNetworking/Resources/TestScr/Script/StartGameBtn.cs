using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameBtn : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] SelectCharKeep selectCharKeep;
    [SerializeField] CharBoxList charBoxList;
    bool isSelectChar;

    private void Start()
    {
        isSelectChar = false;
    }
    /// <summary>
    /// �Q�[�����J�n����{�^�����������Ƃ��ɔ���
    /// </summary>
    public void StartGame()
    {
        if (selectCharKeep.charactorObj == null)
        {
            this.text.text = "�L�����N�^��I�����Ă�������";
            return;
        }
        //�I�񂾃L�����N�^�[�̖��O���i�[
        //�I�񂾃L�����N�^�[���Q�[���ɏo��������List�ɒǉ�
        for (int i = 0; charBoxList.charBox.Count > i; i++)
        {
            //NPC�Ɠ���ւ�
            if (charBoxList.charBox[i].charPrefab.tag == "NPC")
            {
                charBoxList.charBox[i].charPrefab = selectCharKeep.charactorObj;
                charBoxList.charBox[i].charName = selectCharKeep.charactorName;
                break;
            }
        }
        isSelectChar = true;
    }
    public bool IsSelectChar()
    {
        return isSelectChar;
    }
}
