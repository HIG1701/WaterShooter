using UnityEngine;

//飲料のパラメータを管理する
[CreateAssetMenu(menuName = "ScriptableObject/BeverageParameter")]
public class BeverageParameter : ScriptableObject
{
    [Header("飲料基礎情報")]
    public string BeverageID;   //飲料アイテムID
    public string BeverageName; //飲料の名前
    public string BottleKinds;  //ボトルの種類
    public float BeverageDmg;   //攻撃力
    public int Cost;            //飲料の値段
    //追加要素
    //飲料アビリティetc...
}