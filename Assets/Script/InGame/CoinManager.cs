using UnityEngine;

//�R�C���֘A�̏����������܂��B

public class CoinManager : MonoBehaviour
{
    [HideInInspector] public int Coin;                  //�R�C���̎�ނ���́F5 or 20 or 100......
    private void Start()
    {
        // �R�C���̎�ނɉ����Ēl��ݒ�
        switch (gameObject.name)
        {
            case "5Coin":
                Coin = 5;
                break;
            case "20Coin":
                Coin = 20;
                break;
            case "100Coin":
                Coin = 100;
                break;
            default:
                Coin = 0;
                break;
        }
    }
}