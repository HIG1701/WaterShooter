using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float PlayerSpeed = 10f;

    private void Update()
    {
        PlayerShift();
        PlayerDash();
        PlayerMove();
        PlayerJump();
        Playerfire();
        //�����[�hR�L�[
        //�}�E�XScroll�ň����I���B�����ł���
        //�ǂɌ�������WS�L�[
        //�A�r���e�BQ�L�[
    }

    private void PlayerMove()
    {
        //WASD�L�[�ňړ�
    }

    private void PlayerDash()
    {
        //Control�L�[�������Ƃ��ɉ���
    }

    private void PlayerJump()
    {
        //Space���͎�jump
    }

    private void PlayerShift()
    {
        //�V�t�g�ł��Ⴊ��
    }

    private void Playerfire()
    {
        //���}�E�X�Ŕ���
        //�E�}�E�X�ŃG�C��
    }
}