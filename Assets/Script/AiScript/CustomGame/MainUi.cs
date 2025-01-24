using TMPro;
using UnityEngine;

public class MainUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI killCountText;
    [SerializeField] private TextMeshProUGUI currentAmmoText;
    [SerializeField] private TextMeshProUGUI deathCountText;
    [SerializeField] private TextMeshProUGUI characterHpText;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private TextMeshProUGUI totalFireAmmoText;
    [SerializeField] private GameObject statsObject;
    [SerializeField] private TextMeshProUGUI recordText;
    private AimTraining aimTraining;

    private void Update()
    {
        aimTraining = GetComponent<AimTraining>();
        Stats();
    }

    private void Stats()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            statsObject.SetActive(true);
            recordText.SetText
                (
                aimTraining.record[4].ToString() + "\n" + 
                aimTraining.record[3].ToString() + "\n" +
                aimTraining.record[2].ToString() + "\n" +
                aimTraining.record[1].ToString() + "\n" +
                aimTraining.record[0].ToString() + "\n"
                );
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            statsObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        SendText();
    }

    private void SendText()
    {
        killCountText.SetText(ParameterDate.SendCharacterKillCount(playerController).ToString());
        deathCountText.SetText(ParameterDate.SendCharacterDeathCount(playerController).ToString());
        currentAmmoText.SetText(ParameterDate.SendCurrentAmmo(playerController).ToString());
        characterHpText.SetText(ParameterDate.SendCharacterHP(playerController).ToString());
        totalFireAmmoText.SetText(ParameterDate.SendAccuracy(playerController).ToString() + "%");
    }
}
