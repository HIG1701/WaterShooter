using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//�Q�[���V�X�e���S�̂ɂ��ċL�ڂ���
/*
 * ���̃X�N���v�g�ɕK�v�Ȃ���
 * 
 * �Q�[���J�n���A�U�̃X�|�[���n�_�Ɉړ��B�����������_���Ɍ��߂�B
 * �Q�[���J�n����P�T������A�P�T����Q�[�����I������B
 * �}�b�v�O�ɁA�_���[�W�E�H�[����\��
 * �v���C���[���S���ɑh�����鏈�����s���B
 */

public class GameManager : MonoBehaviour
{
    [SerializeField] public List<Transform> SpawnPoints;
    [SerializeField] public List<GameObject> Players;

    private void Start()
    {
        SpawnPlayers();
    }

    private void Update()
    {
        //�X�|�[���n�_���������m�F���邽�߂̃��[�h�V�[��
        if (Input.GetKey(KeyCode.P))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    //���X�g�̓��e���V���b�t������
    //�t�B�b�V���[�C�F�[�c�̃V���b�t���A���S���Y���ɂ��āA�ȉ������N���Q�l�B
    //�Q�l�����N�Fhttps://qiita.com/nkojima/items/c734f786b61a366de831
    private void ShuffleIndex(List<int> Index)
    {
        for (int i = 0; i < Index.Count; i++)
        {
            int Temp = Index[i];
            int RandomIndex = Random.Range(i, Index.Count);

            //����ւ�����
            Index[i] = Index[RandomIndex];
            Index[RandomIndex] = Temp;
        }
    }

    //�v���C���[���X�|�[�������郁�\�b�h
    private void SpawnPlayers()
    {
        //�X�|�[���n�_�̐������C���f�b�N�X���쐬
        List<int> SpawnIndices = new List<int>();
        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            SpawnIndices.Add(i);
        }

        //�V���b�t�����\�b�h���Ăяo���A�V���b�t���B
        ShuffleIndex(SpawnIndices);

        //�v���C���[���X�|�[��
        for (int i = 0; i < Players.Count; i++)
        {
            int SpawnIndex = SpawnIndices[i];

            //players[i]�F���݂̃v���C���[
            //spawnPoints[spawnIndex].position�F�Ή�����X�|�[���ʒu
            //spawnPoints[spawnIndex].rotation�F�Ή�����X�|�[���̌���
            Instantiate(Players[i], SpawnPoints[SpawnIndex].position, SpawnPoints[SpawnIndex].rotation);
        }
    }

    //���S��̃��X�|�[���^�C�}�[���J�n���郁�\�b�h
    public void StartRespawnTimer(GameObject player)
    {
        //5�b��Ƀ��X�|�[��
        StartCoroutine(RespawnPlayer(player, 5f));
    }

    //�v���C���[�����X�|�[��������R���[�`��
    private IEnumerator RespawnPlayer(GameObject player, float delay)
    {
        yield return new WaitForSeconds(delay);
        int randomIndex = Random.Range(0, SpawnPoints.Count);                   //�����_���ȃX�|�[���n�_��I��
        Transform respawnPoint = SpawnPoints[randomIndex];
        player.transform.position = respawnPoint.position;                      //�v���C���[�̈ʒu�����X�|�[���n�_�ɐݒ�
        player.SetActive(true);                                                 //�v���C���[���A�N�e�B�u�ɂ���
    }
}