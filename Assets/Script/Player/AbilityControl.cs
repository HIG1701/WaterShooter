using UnityEngine;

//���̃X�N���v�g�ł́A�v���C�C���[��Ability�ɂ��āA�F�X�L�ڂ��Ă����B
//�v���C���[�̂ق��ł����ƒ����Ȃ�̂ŁA�������ɂ܂Ƃ߂܂��B

public class AbilityControl : MonoBehaviour
{
    public void Ability()
    {
        // Q�L�[�������ꂽ�Ƃ��Ƀf�o�b�O���O��\��
        if (Input.GetKeyDown(KeyCode.Q)) Debug.Log("Ability�����I");
    }
}