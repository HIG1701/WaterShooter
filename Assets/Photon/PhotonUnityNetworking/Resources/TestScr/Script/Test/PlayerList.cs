using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerList : MonoBehaviourPunCallbacks
{
    [SerializeField] Text text;             //�v���C���[����\�����邽�߂�UI�e�L�X�g
    Player player;                          //���݂̃v���C���[����ێ����邽�߂̕ϐ�
    public void SetUp(Player _player)
    {
        player = _player;
        //�v���C���[�̃j�b�N�l�[����UI�e�L�X�g�ɐݒ� 
        text.text = _player.NickName;
    }
    //���[�����̑��̃v���C���[���ޏo�����Ƃ��̏���
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            //�ޏo�����v���C���[�̃��X�g�A�C�e����j��
            Destroy(gameObject);
        }
    }
    //���g�����[����ޏo�����Ƃ��̏���
    public override void OnLeftRoom()
    {
        //���g�̃��X�g�A�C�e����j��
        Destroy(gameObject);
    }
}
