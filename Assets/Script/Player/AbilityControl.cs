using UnityEngine;

//このスクリプトでは、プレイイヤーのAbilityについて、色々記載していく。
//プレイヤーのほうでかくと長くなるので、こっちにまとめます。

public class AbilityControl : MonoBehaviour
{
    public void Ability()
    {
        // Qキーが押されたときにデバッグログを表示
        if (Input.GetKeyDown(KeyCode.Q)) Debug.Log("Ability発動！");
    }
}