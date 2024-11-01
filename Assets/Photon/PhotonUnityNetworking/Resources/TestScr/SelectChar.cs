using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectChar : MonoBehaviour
{
    [SerializeField] CharBoxList charBoxList;
    [SerializeField] GameObject playerPrefab;   //�L�����N�^�[��Prefab
    [SerializeField] string charName;           //�L�����N�^�[�̖��O

    /// <summary>
    /// �C�ӂ̃L�����N�^�[��I�񂾎��ɔ�������
    /// </summary>
    public void OnClick()
    {
        Debug.Log("�L�����N�^�[" + charName + "��I��") ;
        //�I�񂾃L�����N�^�[�̖��O���i�[
        //�I�񂾃L�����N�^�[���Q�[���ɏo��������List�ɒǉ�
        for (int i = 0; charBoxList.charBox.Count > i; i++)
        {
            if (charBoxList.charBox[i].charPrefab.tag == "NPC")
            {
                charBoxList.charBox[i].charPrefab = playerPrefab;
                charBoxList.charBox[i].charName = charName;
                break;
            }
        }
        SceneManager.LoadScene("K_GameScene");
    }
}
