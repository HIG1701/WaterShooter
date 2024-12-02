using UnityEngine;

/// <summary>
/// アビリティに関するクラス
/// </summary>
public class AbilityControl
{
    public void Ability()
    {
        // Qキーが押されたときにデバッグログを表示
        if (Input.GetKeyDown(KeyCode.Q)) Debug.Log("Ability発動！");
    }
}