using UnityEngine;

//�R�C���֘A�̏����������܂��B

public class CoinManager : MonoBehaviour
{
    [SerializeField] public int Coin;                          //�R�C���̎�ނ���́F5 or 20 or 100......

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);                        //�v���C���[�ƐڐG����Ə���
        }
    }
}