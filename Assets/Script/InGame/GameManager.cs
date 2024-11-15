using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO:�v���C���[�̃��\�b�h��ǂ񂾂肵�Ă�̂ŁA�݌v����������
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
        StartSpawnPlayers();
    }

    private void Update()
    {
        //�X�|�[���n�_���������m�F���邽�߂̃��[�h�V�[��
        if (Input.GetKey(KeyCode.P)) SceneManager.LoadScene("SampleScene");
    }

    /// <summary>
    /// ���X�g�̃V���b�t�����s�����\�b�h
    /// </summary>
    /// <param name="index"></param>
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

    private void StartSpawnPlayers()
    {
        //�X�|�[���n�_�̐������C���f�b�N�X���쐬
        List<int> spawnIndices = new List<int>();
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            spawnIndices.Add(i);
        }

        ShuffleIndex(spawnIndices);

        for (int i = 0; i < players.Count; i++)
        {
            int spawnIndex = spawnIndices[i];

            //spawnPoints[spawnIndex].position�F�Ή�����X�|�[���ʒu
            //spawnPoints[spawnIndex].rotation�F�Ή�����X�|�[���̌���
            Instantiate(players[i], spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
        }
    }

    public void StartRespawnTimer(GameObject player)
    {
        //5�b��Ƀ��X�|�[��
        StartCoroutine(RespawnPlayer(player, 5f));
    }

    /// <summary>
    /// �v���C���[���X�|�[�������郁�\�b�h
    /// �i�R���[�`���ŋN�������܂��B�j
    /// </summary>
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