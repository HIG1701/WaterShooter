using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour
{
    [SerializeField] CharBoxList charBoxList;
    [SerializeField] private Camera respawnCamera;                  //���X�|�[�����Ɏg�p����J����

    private void Start()
    {
        SpawnPlayers();
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
        for (int i = 0; i < charBoxList.posBox.Count; i++)
        {
            SpawnIndices.Add(i);
        }

        //�V���b�t�����\�b�h���Ăяo���A�V���b�t���B
        ShuffleIndex(SpawnIndices);

        //�v���C���[���X�|�[��
        for (int i = 0; i < charBoxList.charBox.Count; i++)
        {
            int SpawnIndex = SpawnIndices[i];
            //charBoxList.charBox[i]�F���݂̃v���C���[
            //charBoxList.posBox[spawnIndex].position�F�Ή�����X�|�[���ʒu
            //charBoxList.posBox[spawnIndex].rotation�F�Ή�����X�|�[���̌���
            //Instantiate(charBoxList.charBox[i], charBoxList.posBox[SpawnIndex].position, charBoxList.posBox[SpawnIndex].rotation);
            if (charBoxList.charBox[i].charPrefab.tag == "Player")
            {
                GameObject myChar = PhotonNetwork.Instantiate(charBoxList.charBox[i].charName,
                    charBoxList.posBox[SpawnIndex].position, charBoxList.posBox[SpawnIndex].rotation);
                //�����̂ݑ���\�ɂ���
                PlayerController playerController = myChar.GetComponent<PlayerController>();
                playerController.enabled = true;
                Transform childTransform = myChar.transform.Find("PlayerCamera");
                GameObject myCharChil = childTransform.gameObject;
                CameraFollow cameraFollow = myCharChil.GetComponent<CameraFollow>();
                cameraFollow.enabled = true;
                //GameObject myCharChil = gameObject.transform.Find("PlayerCamera")?.gameObject;
                if (myCharChil == null)
                {
                    Debug.LogError("PlayerCamera not found or is inactive.");
                }

                //CameraFollow cameraFollow = myCharChil.GetComponent<CameraFollow>();
                //cameraFollow.enabled = true;
            }
            else if (charBoxList.charBox[i].charPrefab.tag == "NPC")
            {
                Instantiate(charBoxList.charBox[i].charPrefab, charBoxList.posBox[SpawnIndex].position, charBoxList.posBox[SpawnIndex].rotation);
            }
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
        //���X�|�[���J�������A�N�e�B�u�ɂ���
        respawnCamera.gameObject.SetActive(true);

        yield return new WaitForSeconds(delay);
        int randomIndex = Random.Range(0, charBoxList.posBox.Count);                   //�����_���ȃX�|�[���n�_��I��
        Transform respawnPoint = charBoxList.posBox[randomIndex];
        player.transform.position = respawnPoint.position;                      //�v���C���[�̈ʒu�����X�|�[���n�_�ɐݒ�
        player.SetActive(true);                                                 //�v���C���[���A�N�e�B�u�ɂ���
        respawnCamera.gameObject.SetActive(false);                              //���X�|�[���J�������A�N�e�B�u�ɂ���
    }
}
