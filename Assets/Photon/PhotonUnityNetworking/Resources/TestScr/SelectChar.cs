using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectChar : MonoBehaviour
{
    [SerializeField] CharBoxList charBoxList;
    [SerializeField] GameObject playerPrefab;   //キャラクターのPrefab
    [SerializeField] string charName;           //キャラクターの名前

    /// <summary>
    /// 任意のキャラクターを選んだ時に発生する
    /// </summary>
    public void OnClick()
    {
        Debug.Log("キャラクター" + charName + "を選択") ;
        //選んだキャラクターの名前を格納
        //選んだキャラクターをゲームに出現させるListに追加
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
