using UnityEngine;

public class TestManeger : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private AIControl controller2;
    void Start()
    {
        //PlayerManager.ParametersReset();
        //PlayerManager.SetPlayerDate(controller);
        //PlayerManager.SetPlayerDate(controller2);
        //PlayerManager.CopyMasterDateToRuntimeDate();
        //Debug.LogWarning(PlayerManager.parameters[0].playerRuntimeDate.date.playerHP);
        //Debug.LogWarning(PlayerManager.parameters[1].playerRuntimeDate.date.playerHP);
    }

    private void FixedUpdate()
    {
        
    }

}
