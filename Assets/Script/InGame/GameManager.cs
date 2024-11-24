using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO:�Q�[���V�X�e���S�̂ɂ��ċL�ڂ���
/*
 * ���̃X�N���v�g�ɕK�v�Ȃ���
 * 
 * �Q�[���J�n���A�U�̃X�|�[���n�_�Ɉړ��B�����������_���Ɍ��߂�B
 * �Q�[���J�n����P�T������A�P�T����Q�[�����I������B
 * �}�b�v�O�ɁA�_���[�W�E�H�[����\��
 * �v���C���[���S���ɑh�����鏈�����s���B
 * �R�C���̗ʂȂǂŏ��s�����߂�̂͂��̃N���X�H
 */

/// <summary>
/// �Q�[���V�X�e���S�̂Ɋւ���N���X
/// </summary>

public class GameManager : MonoBehaviour
{
    [SerializeField] public List<Transform> spawnPoints;
    [SerializeField] public List<GameObject> players;

    private void Start()
    {
        SpawnPlayers();
    }

    private void Update()
    {
        //�X�|�[���n�_���������m�F���邽�߂̃��[�h�V�[��
        if (Input.GetKey(KeyCode.P)) SceneManager.LoadScene("SampleScene");
    }

    //���X�g�̓��e���V���b�t������
    //�t�B�b�V���[�C�F�[�c�̃V���b�t���A���S���Y���ɂ��āA�ȉ������N���Q�l�B
    //�Q�l�����N�Fhttps://qiita.com/nkojima/items/c734f786b61a366de831
    private void ShuffleIndex(List<int> index)
    {
        for (int i = 0; i < index.Count; i++)
        {
            int temp = index[i];
            int randomIndex = Random.Range(i, index.Count);

            //����ւ�����
            index[i] = index[randomIndex];
            index[randomIndex] = temp;
        }
    }

    //�v���C���[���X�|�[�������郁�\�b�h
    private void SpawnPlayers()
    {
        //�X�|�[���n�_�̐������C���f�b�N�X���쐬
        List<int> spawnIndices = new List<int>();
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            spawnIndices.Add(i);
        }

        //�V���b�t�����\�b�h���Ăяo���A�V���b�t���B
        ShuffleIndex(spawnIndices);

        //�v���C���[���X�|�[��
        for (int i = 0; i < players.Count; i++)
        {
            int spawnIndex = spawnIndices[i];

            //players[i]�F���݂̃v���C���[
            //spawnPoints[spawnIndex].position�F�Ή�����X�|�[���ʒu
            //spawnPoints[spawnIndex].rotation�F�Ή�����X�|�[���̌���
            Instantiate(players[i], spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
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
        int randomIndex = Random.Range(0, spawnPoints.Count);                   //�����_���ȃX�|�[���n�_��I��
        Transform respawnPoint = spawnPoints[randomIndex];
        player.transform.position = respawnPoint.position;                      //�v���C���[�̈ʒu�����X�|�[���n�_�ɐݒ�
        player.SetActive(true);                                                 //�v���C���[���A�N�e�B�u�ɂ���

        //�v���C���[��Respawn���\�b�h���Ăяo��
        player.GetComponent<PlayerController>().Respawn();
    }
}