using System.Collections;
using TMPro;
using UnityEngine;

public class MainUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI killCountText;
    [SerializeField] private TextMeshProUGUI currentAmmoText;
    [SerializeField] private TextMeshProUGUI deathCountText;
    [SerializeField] private TextMeshProUGUI characterHpText;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI totalFireAmmoText;
    [SerializeField] private GameObject statsObject;
    [SerializeField] private TextMeshProUGUI recordText;
    [SerializeField] private TextMeshProUGUI coinText;
    private AudioSource audioSource;
    private AimTraining aimTraining;
    private int hitCount;

    private void Start()
    {
        aimTraining = GetComponent<AimTraining>();
        audioSource = GetComponent<AudioSource>();
        Stats();
        StartCoroutine(StartStatsView());
    }

    private void Update()
    {
        aimTraining = GetComponent<AimTraining>();
        Stats();
        HitAudio();
    }

    private IEnumerator StartStatsView()
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
        yield return new WaitForSeconds(1f);
        statsObject.SetActive(false);
    }

    private void Stats()
    {
        Debug.Log(playerController.gameObject.activeSelf);
        if (Input.GetKeyDown(KeyCode.Tab) && player.activeSelf)
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
        else if (Input.GetKeyUp(KeyCode.Tab) && player.activeSelf)
        {
            statsObject.SetActive(false);
        }

        if (!player.activeSelf)
        {
            Debug.LogWarning("non");
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

        if (!Input.GetKey(KeyCode.Tab) && player.activeSelf)
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
        coinText.SetText(ParameterDate.SendCharacterCoin(playerController.CharacterIndex).ToString());
    }

    private void HitAudio()
    {
        if (PlayerManager.parameters[playerController.CharacterIndex].playerRuntimeDate.date.hitAmmo > hitCount)
        {
            audioSource.Play();
        }
        hitCount = PlayerManager.parameters[playerController.CharacterIndex].playerRuntimeDate.date.hitAmmo;
    }
}
