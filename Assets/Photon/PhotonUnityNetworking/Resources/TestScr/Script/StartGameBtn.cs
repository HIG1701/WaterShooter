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
    /// ゲームを開始するボタンを押したときに発生
    /// </summary>
    public void StartGame()
    {
        if (selectCharKeep.charactorObj == null)
        {
            this.text.text = "キャラクタを選択してください";
            return;
        }
        //選んだキャラクターの名前を格納
        //選んだキャラクターをゲームに出現させるListに追加
        for (int i = 0; charBoxList.charBox.Count > i; i++)
        {
            //NPCと入れ替え
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
