using UnityEngine;

//�����̃p�����[�^���Ǘ�����
[CreateAssetMenu(menuName = "ScriptableObject/BeverageParameter")]
public class BeverageParameter : ScriptableObject
{
    [Header("������b���")]
    public string BeverageID;   //�����A�C�e��ID
    public string BeverageName; //�����̖��O
    public string BottleKinds;  //�{�g���̎��
    public float BeverageDmg;   //�U����
    public int Cost;            //�����̒l�i
    //�ǉ��v�f
    //�����A�r���e�Betc...
}