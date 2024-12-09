using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class SelectChar : MonoBehaviourPunCallbacks
{
    [SerializeField] Text msgText;

    [SerializeField] Button confirmButton;
    [SerializeField] Button backButton;
    [SerializeField] Button startGameButton;
    [SerializeField] Button exitBtn;

    [SerializeField] Button[] charBtn;        //�L�����I���{�^��
    private bool[] isCharSelect;      //�L�������I������Ă��邩

    private void Start()
    {
        Debug.Log("����������");
        msgText.text = "�L������I�����Ă�����";

        //�z��̗v�f������������
        //for���𗘗p���āA�e�{�^���ɃN���b�N���X�i�[��ǉ�
        isCharSelect = new bool[charBtn.Length];
        for (int i = 0; i < isCharSelect.Length; i++)
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
            startGameButton.interactable = false;
            //charBtn[i].onClick.AddListener(() => OnCharSelect(idx));
            //�����̃L�����N�^�[�I����Ԃ𓯊�
            //UpdateCharSelectFromProperties();
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
        if (isCharSelect == null || charBtn == null) 
        {
            Debug.LogError("charSelect �܂��� charBtn ������������ĂȂ�");
            return;
        }
        if (idx < 0 || idx >= isCharSelect.Length) 
        {
            Debug.LogError("Index out of bounds: " + idx); 
            return;
        }
        if (!isCharSelect[idx])
        {
            Debug.Log("�L�����N�^�i���o�[" + idx + "��I��");
            msgText.text = "�p�ӂ��ł����珀�������{�^�����N���b�N";

            isCharSelect[idx] = true;
            //�{�^���𖳌�
            //�I�������{�^����L��
            //�߂�{�^����\��
            charBtn[idx].interactable = false;    �@
            confirmButton.interactable = true;
            exitBtn.interactable = false;
            backButton.gameObject.SetActive(true);
            //�L�����N�^�I����Ԃłق��̃L�����N�^�͑I�ׂȂ�
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
            //isCharSelect[idx] = true;
            ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
            playerProperties["CharSelect"] = idx;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
            photonView.RPC("CharSelect", RpcTarget.AllBuffered, idx, PhotonNetwork.LocalPlayer.ActorNumber);
            photonView.RPC("UpdateCharSelectState", RpcTarget.AllBuffered, idx);
        }
    }
    //RPC���\�b�h�F�L�����N�^���I�����ꂽ���Ƃ�S�v���C���[�ɒʒm
    //Photon�l�b�g���[�N��ʂ��ă����[�g�ŌĂяo��
    // idx      : �I�����ꂽ�L�����N�^�[�̃C���f�b�N�X
    // playerID : �v���C���[��ID
    [PunRPC]
    public void CharSelect(int idx, int playerID)
    {
        if (isCharSelect == null || charBtn == null)
        {
            return;
        }
        if (idx < 0 || idx >= isCharSelect.Length)
        {
            return;
        }
        if (!isCharSelect[idx])
        {
            //�I�����ꂽ�L�����N�^�[�̃{�^���𖳌���
            isCharSelect[idx] = true;
            charBtn[idx].interactable = false;
            //�z�X�g���S�v���C���[���L������I�����Ă��邩�ǂ����m�F
            //�����I�����Ă�����Q�[���J�n�{�^����L����
            if (PhotonNetwork.IsMasterClient && AllPlayerSelect())
            {
                EnableStartGameButton();
            }
        }
    }
    //�ق��̃v���C���[����̑I�����������󂯎U��RPC���\�b�h
    [PunRPC]
    public void ResetCharSelect(int playerID)
    {
        //�w�肳�ꂽ�v���C���[�̑I�������̂ݏ���
        if(PhotonNetwork.LocalPlayer.ActorNumber == playerID)
        {
            int selectedIdx = GetSelectedCharIndex(PhotonNetwork.LocalPlayer);
            if (selectedIdx >= 0 && selectedIdx < isCharSelect.Length)
            {
                // �L�����N�^�[�̑I����Ԃ�����
                isCharSelect[selectedIdx] = false;
                // �{�^����L����
                charBtn[selectedIdx].interactable = true; 
            }
        }
    }

    //�I�������̏�Ԃ�S�v���C���[�ɒʒm
    [PunRPC]
    public void UpdateCharSelectState(int idx, PhotonMessageInfo info)
    {
        Debug.Log($"Player {info.Sender.NickName} has updated their selection.");
        if (idx >= 0 && idx < isCharSelect.Length)
        {
            isCharSelect[idx] = true;
            charBtn[idx].interactable = false;
        }
        else if (idx == -1)
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties.TryGetValue("CharSelect", out object charIdxObj))
                {
                    int charIdx = (int)charIdxObj;
                    if (charIdx >= 0 && charIdx < isCharSelect.Length)
                    {
                        isCharSelect[charIdx] = false;
                        charBtn[charIdx].interactable = true;
                    }
                }
            }
        }
    }
    //�I�������{�^����������ɌĂяo��
    public void OnConfirmSelection()
    {
        msgText.text = "���������I";

        //�I�������{�^������
        //�߂�{�^���\��
        confirmButton.interactable = false;
        backButton.gameObject.SetActive(true);
        // �Q�[���J�n�{�^�����L�����ǂ����̃`�F�b�N
        if (PhotonNetwork.IsMasterClient && AllPlayerSelect())
        {
            EnableStartGameButton();
        }
    }
    //�߂�{�^��������ɌĂяo��
    public void OnBack()
    {
        //�J�X�^���v���p�e�B����I�����ꂽ�L�����N�^�[�̃C���f�b�N�X���擾
        //�������I�����Ă����L�����N�^�[�̃C���f�b�N�X���擾
        int selectedIdx = GetSelectedCharIndex(PhotonNetwork.LocalPlayer);
        if (selectedIdx >= 0 && selectedIdx < isCharSelect.Length)
        {
            //�I������
            //���������L�����N�^�[���đI���\��
            isCharSelect[selectedIdx] = false;
            charBtn[selectedIdx].interactable = true; 

            //�����̃J�X�^���v���p�e�B�����Z�b�g
            ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable { ["CharSelect"] = -1 };
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

            //UI�����Z�b�g
            confirmButton.interactable = false;
            backButton.gameObject.SetActive(false);
            exitBtn.interactable = true;
            foreach (Button btn in charBtn)
            {
                int idx = System.Array.IndexOf(charBtn, btn);
                //���I���L������L����
                btn.interactable = !isCharSelect[idx];
            }

            //���b�Z�[�W�X�V
            msgText.text = "�L������I�����Ă�������";
        }
        //�}�X�^�[�N���C�A���g�̏ꍇ�A�S�v���C���[���I�����Ă��邩�m�F
        if (PhotonNetwork.IsMasterClient && !AllPlayerSelect())
        {
            startGameButton.interactable = false;
        }
    }

    //�J�X�^���v���p�e�B����v���C���[���I�񂾃L�����N�^�[�̃C���f�b�N�X���擾����
    public int GetSelectedCharIndex(Player player)
    {
        if (player.CustomProperties.TryGetValue("CharSelect", out object charIndex)) 
        {
            return (int)charIndex;
        }
        else
        { 
            // �L�����N�^�[���I������Ă��Ȃ��ꍇ�̃f�t�H���g�l
            return -1; 
        }
    }
    //�S�v���[���[���L�����N�^��I���������ǂ���
    private bool AllPlayerSelect()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            msgText.text = "�Q���l�����P�̏ꍇ�͊J�n�ł��܂���";
            //�Q���l������l�̏ꍇ�Q�[���J�n�͂ł��Ȃ�
            return false;
        }
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("CharSelect"))
            {
                //�����ꂩ��Player�����I��
                return false;
            }
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
    public void StartGame() 
    {
        Debug.Log("�Q�[����ʂֈڍs");
    }
    //�J�X�^���v���p�e�B����L�����N�^�[�I����Ԃ��X�V���郁�\�b�h
    private void UpdateCharSelectFromProperties()
    {
        if (charBtn == null || isCharSelect == null)
        {
            return;
        }
        if (PhotonNetwork.PlayerList == null || PhotonNetwork.PlayerList.Length == 0)
        {
            return;
        }
        //������
        for (int i = 0; i < isCharSelect.Length; i++)
        {
            isCharSelect[i] = false;
            //��x���ׂẴ{�^����L����
            charBtn[i].interactable = true;
        }
        //�e�v���C���[�̑I����Ԃ��m�F���A�I���ς݂̃L�����N�^�[�̃{�^���𖳌���
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue("CharSelect", out object charIdxObj))
            {
                int charIdx = (int)charIdxObj;

                if (charIdx >= 0 && charIdx < isCharSelect.Length)
                {
                    isCharSelect[charIdx] = true;
                    //�I���ς݂̃L�����𖳌���
                    charBtn[charIdx].interactable = false;
                }
            }
            
        }
        //�������I�����Ă����L�����N�^�[���ĂёI���\�ɐݒ�
        int myCharIdx = GetSelectedCharIndex(PhotonNetwork.LocalPlayer);
        if (myCharIdx >= 0 && myCharIdx < isCharSelect.Length)
        {
            //�đI��������
            charBtn[myCharIdx].interactable = true; //�đI��������
        }
    }
    //�v���C���[�������ɓ������^�C�~���O�ŁA����������Ă��邩�m�F
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        if (!newPlayer.CustomProperties.ContainsKey("CharSelect"))
        {
            //���I����Ԃ�����
            properties["CharSelect"] = -1; 
            newPlayer.SetCustomProperties(properties);
        }
    }
    //�v���C���[���Q�����Ă������ɌĂяo��
    public override void OnJoinedRoom()
    {
        UpdateCharSelectFromProperties();
    }
}