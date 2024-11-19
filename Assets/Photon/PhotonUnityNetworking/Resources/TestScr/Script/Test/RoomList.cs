using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomList : MonoBehaviour
{
    [SerializeField] Text label;        //���[������\�����邽�߂�UI�e�L�X�g 
    RoomInfo info;                      //���݂̃��[���̏���ێ����邽�߂̕ϐ� 

    //���[������ݒ肷�郁�\�b�h 
    public void SetUp(RoomInfo _info)
    {
        //���[������UI�e�L�X�g�ɐݒ�
        info = _info; label.text = _info.Name;
    }
    //���[�����X�g�A�C�e�����N���b�N���ꂽ�Ƃ��̏���
    public void OnClick()
    {
        //Launcher�̃C���X�^���X���g���ă��[���ɎQ��
        Launcher.Instance.JoinRoom(info);
    }
}
