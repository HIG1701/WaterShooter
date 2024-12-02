using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class SelectChar : MonoBehaviourPunCallbacks
{
    public Button confirmButton;
    public Button backButton;
    public Button startGameButton;
    public Button[] charBtn;        //�L�����I���{�^��
    private bool[] charSelect;      //�L�������I������Ă��邩

    private void Start()
    {
        //�z��̗v�f������������
        //for���𗘗p���āA�e�{�^���ɃN���b�N���X�i�[��ǉ�
        charSelect = new bool[charBtn.Length];
        for (int i = 0; i < charSelect.Length; i++)
        {
            int idx = i;
            if (charBtn[i] != null)
            {
                charBtn[i].onClick.AddListener(() => OnCharSelect(idx));
            }
            else
            {
                Debug.LogError("charBtn[" + i + "] is not set.");
            }
            //�I�������{�^���Ɩ߂�{�^���Ƀ��X�i�[��ǉ�
            confirmButton.onClick.AddListener(OnConfirmSelection);
            backButton.onClick.AddListener(OnBack);
            confirmButton.interactable = false;
            backButton.gameObject.SetActive(false);
            //charBtn[i].onClick.AddListener(() => OnCharSelect(idx));
        }
    }

    public void SelectCharInitialize()
    {
        //������
        confirmButton.interactable = false;
        backButton.gameObject.SetActive(false);
        foreach (Button btn in charBtn)
        {
            btn.interactable = true;
        }
    }

    //�L�����N�^�I�����ɌĂяo��
    public void OnCharSelect(int idx)
    {
        if (charSelect == null || charBtn == null) 
        {
            Debug.LogError("charSelect or charBtn is not initialized.");
            return;
        }
        if (idx < 0 || idx >= charSelect.Length) 
        {
            Debug.LogError("Index out of bounds: " + idx); 
            return;
        }
        if (!charSelect[idx])
        {
            charSelect[idx] = true;
            //�{�^���𖳌�
            //�I�������{�^����L��
            //�߂�{�^����\��
            charBtn[idx].interactable = false;    �@
            confirmButton.interactable = true;    �@
            backButton.gameObject.SetActive(true);
            //���̃L�����N�^�[�{�^���𖳌���
            foreach (Button btn in charBtn) 
            {
                if (btn != charBtn[idx]) 
                {
                    btn.interactable = false; 
                }
            }
            //�L�����N�^�I����Ԃ��X�V�A�L�^
            //�I�����ꂽ�L�����N�^�[�̏����J�X�^���v���p�e�B���i�[���邽�߂̃n�b�V���e�[�u���쐬���ۑ�
            //�I���L�����N�^�[��idx���J�X�^���v���p�e�B�ɐݒ�
            //�v���C���[�̃J�X�^���v���p�e�B�Ƃ��Đݒ肵������Photon�l�b�g���[�N��ɕۑ�
            //RPC���\�b�h���g���đS�v���C���[�ɑI������ʒm�A�V���ȃv���C���[�ɂ����f�����悤�Ƀo�b�t�@�����O
            charSelect[idx] = true;
            ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
            playerProperties["CharSelect"] = idx;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
            photonView.RPC("CharSelect", RpcTarget.AllBuffered, idx, PhotonNetwork.LocalPlayer.ActorNumber);
            //Debug.Log(RpcTarget.AllBuffered);
            //Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }
    //RPC���\�b�h�F�L�����N�^���I�����ꂽ���Ƃ�S�v���C���[�ɒʒm
    //Photon�l�b�g���[�N��ʂ��ă����[�g�ŌĂяo��
    // idx      : �I�����ꂽ�L�����N�^�[�̃C���f�b�N�X
    // playerID : �v���C���[��ID
    [PunRPC]
    public void CharSelect(int idx, int playerID)
    {
        if (charSelect == null || charBtn == null)
        {
            Debug.LogError("charSelect or charBtn is not initialized.");
            return;
        }
        if (idx < 0 || idx >= charSelect.Length)
        {
            Debug.LogError("Index out of bounds: " + idx);
            return;
        }
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
    //�I�������{�^����������ɌĂяo��
    public void OnConfirmSelection()
    {
        //�I�������{�^������
        //�߂�{�^����\��
        confirmButton.interactable = false;
        backButton.gameObject.SetActive(true);
        // �Q�[���J�n�{�^�����L�����ǂ����̃`�F�b�N
        if (PhotonNetwork.IsMasterClient && AllPlayerSelect())
        {
            EnableStartGameButton();
        }
    }
    //�߂�{�^��������ɌĂё�s
    public void OnBack()
    {
        //�J�X�^���v���p�e�B����I�����ꂽ�L�����N�^�[�̃C���f�b�N�X���擾
        int selectedIdx = (int)PhotonNetwork.LocalPlayer.CustomProperties["CharSelect"];
        if (selectedIdx >= 0 && selectedIdx < charSelect.Length)
        {
            //�L�����I������
            //�L�����N�^�{�^���L��
            //�I�������{�^���L��
            //�߂�{�^����\��
            charSelect[selectedIdx] = false;
            charBtn[selectedIdx].interactable = true;
            confirmButton.interactable = false;
            backButton.gameObject.SetActive(false);
            //���̃L�����N�^�[�{�^�����ēx�L����
            foreach (Button btn in charBtn)
            {
                btn.interactable = true;
            }
            //�I���������������Ƃ��J�X�^���v���p�e�B����폜
            ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
            playerProperties["CharSelect"] = -1;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        }
    }
    //�S�v���[���[���L�����N�^��I���������ǂ���
    private bool AllPlayerSelect()
    {
        foreach (var selected in charSelect) 
        { 
            if (!selected) return false; 
        }
        return true;
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