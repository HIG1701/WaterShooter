using UnityEngine;

/// <summary>
/// �A�r���e�B�Ɋւ���N���X
/// </summary>
public class AbilityControl
{
    public void Ability()
    {
        // Q�L�[�������ꂽ�Ƃ��Ƀf�o�b�O���O��\��
        if (Input.GetKeyDown(KeyCode.Q)) Debug.Log("Ability�����I");
    }
}