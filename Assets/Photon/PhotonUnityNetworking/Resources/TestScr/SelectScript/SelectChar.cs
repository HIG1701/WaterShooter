using UnityEngine;
using UnityEngine.UI;

public class SelectChar : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] SelectCharKeep selectCharKeep;
    [SerializeField] GameObject playerPrefab;   //�L�����N�^�[��Prefab
    [SerializeField] string charName;           //�L�����N�^�[�̖��O

    /// <summary>
    /// �C�ӂ̃L�����N�^�[��I�񂾎��ɔ�������
    /// </summary>
    public void OnClick()
    {
        //�I�ԃL�����N�^���i�[
        selectCharKeep.charactorObj = playerPrefab;
        selectCharKeep.charactorName = charName;
        this.text.text = charName + " �� �I��"; 
    }
}
