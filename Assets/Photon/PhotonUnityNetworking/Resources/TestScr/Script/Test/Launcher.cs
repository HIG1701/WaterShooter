using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] TMP_InputField playerNameInput;        //Player�̖��O������
    [SerializeField] TMP_Text titleWelcomeText;             //
    [SerializeField] TMP_InputField roomNameInput;          //���[���̖��O������
    [SerializeField] Transform roomListContent;
    [SerializeField] Text roomName;                         //���[���̖��O������
    [SerializeField] GameObject roomListPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListPrefab;
    [SerializeField] GameObject startGameBtn;
    

    private void Awake()
    {
        //���݂̃C���X�^���X���ÓI�v���p�e�B�Ɋ��蓖�Ă���
        Instance = this;
    }
    private void Start()
    {
        //�}�X�^�[�T�[�o�[�ɐڑ�
        Debug.Log("�}�X�^�[�T�[�o�[�ɐڑ���");
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        //�}�X�^�[�T�[�o�[�ڑ�������Ƀ��r�[�Q��
        Debug.Log("�}�X�^�[�T�[�o�[�ɐڑ�����");
        PhotonNetwork.JoinLobby();
        //true�ɂ���ƃz�X�g���V�[�����[�h����Ƃق��̃v���C���[�������I�ɃV�[���ړ�
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnJoinedLobby()
    {
        //���r�[�Q����ɌĂяo��
        if (PhotonNetwork.NickName == "")
        {
            //�j�b�N�l�[�����Ȃ��ꍇ�����_���ȃj�b�N�l�[����ݒ�
            //���O���̓��j���[���J��
            PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString();
            MenuMng.Instance.OpenMenu("name");
        }
        else
        {
            MenuMng.Instance.OpenMenu("title");
        }
        Debug.Log("���r�[�ɎQ�����܂���");
    }
    public void SetName()
    {
        //Player�̃j�b�N�l�[���ݒ�
        string name = playerNameInput.text;
        //name�����炩�ǂ���
        if (!string.IsNullOrEmpty(name))
        {
            PhotonNetwork.NickName = name;
            titleWelcomeText.text = $"Welcome, {name}!";
            //�^�C�g����ʂ��J��
            MenuMng.Instance.OpenMenu("title");
            //���̓t�B�[���h���N���A
            playerNameInput.text = "";
        }
        else
        {
            Debug.Log("���O���Ȃ��ł�");
        }
    }
    public void CreatRoom()
    {
        //���[�����j���[�����͂���Ă���ꍇ�A�V�������[���쐬
        if(!string.IsNullOrEmpty(roomNameInput.text))
        {
            PhotonNetwork.CreateRoom(roomNameInput.text);
            MenuMng.Instance.OpenMenu("loading");
            roomNameInput.text = "";
        }
        else
        {
            Debug.Log("���[���������͂���Ă��܂���");
        }
    }
    public override void OnJoinedRoom()
    {
        //���[���ɎQ������ƃ��[��UI��ݒ�
        MenuMng.Instance.OpenMenu("room");
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        //�v���C���[���X�g���X�V
        Player[] players = PhotonNetwork.PlayerList;
        //�����̃v���C���[���X�g���N���A
        foreach (Transform trans in playerListContent)
        {
            Destroy(trans.gameObject);
            //TODO (6)_�v���C���[���X�g�N���A
        }
        //�V�����v���C���[���X�g���쐬
        for (int i = 0; i < players.Count(); i++)
        {
            //TODO (1)_�L�����N�^�̐���
            Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerList>().SetUp(players[i]);
        }
        //�}�X�^�[�N���C�A���g�Ȃ�Q�[���X�^�[�g�{�^���L��
        startGameBtn.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //�}�X�^�[�N���C�A���g���ς��������Ƃ��Ăяo��
        //�X�^�[�g�{�^���̗L����ݒ�
        startGameBtn.SetActive(PhotonNetwork.IsMasterClient);
    }
    public void LeaveRoom()
    {
        //���[���ޏo���A���[�f�B���O��ʂ�\��
        PhotonNetwork.LeaveRoom();
        MenuMng.Instance.OpenMenu("loadeing");
    }
    public void JoinRoom(RoomInfo info)
    {
        //�w�肳�ꂽ���[���ɎQ��
        PhotonNetwork.JoinRoom(info.Name);
        MenuMng.Instance.OpenMenu("title");
    }
    public override void OnLeftRoom()
    {
        //Player��room��ޏo�����Ƃ�
        MenuMng.Instance.OpenMenu("title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //���r�[�̃��[�����X�g���X�V�񂳂ꂽ�Ƃ��ɌĂяo��
        //���X�g�X�V
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count(); i++)
        {
            if (roomList[i].RemovedFromList)
            {
                // Don't instantiate stale rooms
                continue;
            }
            Instantiate(roomListPrefab, roomListContent).GetComponent<RoomList>().SetUp(roomList[i]);
            //TODO (2)_
        }
        //TODO (3)_�L��������
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //TODO (4)_���[���쐬�Ɏ��s�����Ƃ�
        //errorText.text = "Room Creation Failed: " + message;
        //MenuManager.Instance.OpenMenu("error");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //TODO (5)_�V����playr���Q�������Ƃ�
        //playr���X�g���X�V
    }
    public void StartGame()
    {
        //�Q�[���J�n
        //�}�X�^�[�N���C�A���g���Q�[�����J�n
        //�S�����Q�[���V�[���ֈړ�
        PhotonNetwork.LoadLevel("3_GameScene");
    }
    public void QuitGame()
    {
        //�Q�[���I��
        Application.Quit();
    }


}
