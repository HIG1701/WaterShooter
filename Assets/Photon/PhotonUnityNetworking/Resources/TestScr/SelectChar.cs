using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectChar : MonoBehaviour
{
    [SerializeField] CharBoxList charBoxList;
    [SerializeField] string charName;

    public void OnClick()
    {
        Debug.Log("キャラクター" + charName + "を選択") ;
        charBoxList.setCharName = charName;
        SceneManager.LoadScene("SampleScene");
    }
}
