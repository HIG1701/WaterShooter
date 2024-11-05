using UnityEngine;
using UnityEngine.UI;

public class SelectChar : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] SelectCharKeep selectCharKeep;
    [SerializeField] GameObject playerPrefab;   //キャラクターのPrefab
    [SerializeField] string charName;           //キャラクターの名前

    /// <summary>
    /// 任意のキャラクターを選んだ時に発生する
    /// </summary>
    public void OnClick()
    {
        //選ぶキャラクタを格納
        selectCharKeep.charactorObj = playerPrefab;
        selectCharKeep.charactorName = charName;
        this.text.text = charName + " を 選択中"; 
    }
}
