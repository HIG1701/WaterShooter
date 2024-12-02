using Photon.Pun;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SelectChar : MonoBehaviourPunCallbacks
{
    public Button startGameButton;
    public Button[] charBtn;        //�L�����I���{�^��
    private bool[] charSelect;      //�L�������I������Ă��邩
    private int maxplyer = 6;       //�ő�Player��Player��

    private void Start()
    {
        //�z��̗v�f������������
        //for���𗘗p���āA�e�{�^���ɃN���b�N���X�i�[��ǉ�
        charSelect = new bool[charBtn.Length];
        for (int i = 0; i < charSelect.Length; i++)
        {
            int idx = i;
            charBtn[i].onClick.AddListener(() => OnCharSelect(idx));
        }
    }
    //�L�����N�^�I�����ɌĂяo��
    public void OnCharSelect(int idx)
    {
        if (!charSelect[idx])
        {
            //�L�����N�^�I����Ԃ��X�V�A�L�^
            //�I�����ꂽ�L�����N�^�[�̏����J�X�^���v���p�e�B���i�[���邽�߂̃n�b�V���e�[�u���쐬���ۑ�
            //�I���L�����N�^�[��idx���J�X�^���v���p�e�B�ɐݒ�
            //�v���C���[�̃J�X�^���v���p�e�B�Ƃ��Đݒ肵������Photon�l�b�g���[�N��ɕۑ�
            //RPC���\�b�h���g���đS�v���C���[�ɑI������ʒm�A�V���ȃv���C���[�ɂ����f�����悤�Ƀo�b�t�@�����O
            charSelect[idx] = true;
            ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
            playerProperties["CharSelect"] = idx;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
            //photonView.RPC("CharSelect", RpcTarget.AllBuffered, idx, PhotonNetwork.LocalPlayer.ActorNumber);
            Debug.Log(RpcTarget.AllBuffered);
            Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }
    //RPC���\�b�h�F�L�����N�^���I�����ꂽ���Ƃ�S�v���C���[�ɒʒm
    //Photon�l�b�g���[�N��ʂ��ă����[�g�ŌĂяo��
    // idx      : �I�����ꂽ�L�����N�^�[�̃C���f�b�N�X
    // playerID : �v���C���[��ID
    [PunRPC]
    public void CharSelect(int idx, int playerID)
    {
        if (!charSelect[idx])
        {
            //�I�����ꂽ�L�����N�^�[�̃{�^���𖳌���
            charSelect[idx] = true;
            charBtn[idx].interactable = false;
            //�z�X�g���S�v���C���[���L������I�����Ă��邩�ǂ����m�F
            //�����I�����Ă�����Q�[���J�n�{�^����L����
            if (PhotonNetwork.IsMasterClient && AllPlayerSelect())
            {
                EnableStartGameButton();
            }
        }
    }

    //�S�v���[���[���L�����N�^��I���������ǂ���
    private bool AllPlayerSelect()
    {
        return PhotonNetwork.PlayerList.Length == maxplyer && charSelect.All(selected => selected);
    }
    // �Q�[���J�n�{�^����L����
    private void EnableStartGameButton()
    {
        //�{�^���N���b�N�\�ɂ���
        //onClick.AddListener���\�b�h���g�����Ƃœ���̏��������s
        startGameButton.interactable = true;
        startGameButton.onClick.AddListener(StartGame);
    }
    // �Q�[���V�[���ֈڍs
    private void StartGame() 
    {
        Debug.Log("�Q�[����ʂֈڍs");
    }
}
