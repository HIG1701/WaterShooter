using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectChar : MonoBehaviour
{
    [SerializeField] CharBoxList charBoxList;
    [SerializeField] string charName;

    public void OnClick()
    {
        Debug.Log("�L�����N�^�[" + charName + "��I��") ;
        charBoxList.setCharName = charName;
        SceneManager.LoadScene("SampleScene");
    }
}
