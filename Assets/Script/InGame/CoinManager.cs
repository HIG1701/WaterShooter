using UnityEngine;

//�R�C���֘A�̏����������܂��B

public class CoinManager : MonoBehaviour
{
    //�p�u���b�N�����ǕύX���Ăق����Ȃ��̂ŉB���Ă܂�
    [HideInInspector] public int Coin;

    private void Start()
    {
        //�R�C���̎�ނɉ����Ēl��ݒ�
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