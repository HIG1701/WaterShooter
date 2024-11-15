using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] TMP_InputField playerNameInput;
    [SerializeField] TMP_Text titleWelcomeText;
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

    }
}
