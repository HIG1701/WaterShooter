using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

/// <summary>
/// MFópÇÃÇΩÇﬂìÀä—çHéñ
/// </summary>
public class MainUi : MonoBehaviour
{
    [SerializeField] private float space = 100f;
    [SerializeField] private Transform parentPanel;
    [SerializeField] private GameObject imagePrefab;
    [SerializeField] private TextMeshProUGUI killCountText;
    [SerializeField] private TextMeshProUGUI currentAmmoText;
    [SerializeField] private TextMeshProUGUI deathCountText;
    [SerializeField] private TextMeshProUGUI characterHpText;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI totalFireAmmoText;
    [SerializeField] private GameObject statsObject;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private TextMeshProUGUI recordText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI totalAmmoText;
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject subMenu;
    [SerializeField] public int[] record = new int[5];
    [SerializeField] private List<TextMeshProUGUI> coinRankingTexts = new List<TextMeshProUGUI>();
    [SerializeField] private GameObject coinRankingText;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI dpsText;
    private int maxCoin = 0;
    private AudioSource audioSource;
    private AimTraining aimTraining;
    private int hitCount;

    private void Start()
    {
        aimTraining = GetComponent<AimTraining>();
        audioSource = GetComponent<AudioSource>();
        cameraFollow = cameraFollow.gameObject.GetComponent<CameraFollow>();
        Stats();
        StartCoroutine(StartStatsView());
    }

    private void Update()
    {
        aimTraining = GetComponent<AimTraining>();
        Stats();
        MainMenu();
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

    private void MainMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && buttons.activeSelf)
        {
            mainMenu.SetActive(!mainMenu.activeSelf);
            if (mainMenu.activeSelf) { Cursor.lockState = CursorLockMode.None; Time.timeScale = 0; }
            else if (!mainMenu.activeSelf) { Cursor.lockState = CursorLockMode.Locked; Time.timeScale = 1; }
            Cursor.visible = mainMenu.activeSelf;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !buttons.activeSelf)
        {
            coinRankingText.SetActive(false);
            slider.gameObject.SetActive(false);
            buttons.SetActive(true);
        }
    }

    private void Record()
    {
        if (maxCoin < ParameterDate.SendCharacterCoin(player.gameObject.GetComponentInChildren<PlayerController>().CharacterIndex))
        {
            maxCoin = ParameterDate.SendCharacterCoin(player.gameObject.GetComponentInChildren<PlayerController>().CharacterIndex);
        }
        Debug.Log(maxCoin);
        if (player.activeSelf) { return; }
        if (record[0] < maxCoin)
        {
            Debug.Log(maxCoin);
            record[0] = maxCoin;
            maxCoin = 0;
        }
        int temp = 0;
        for (int i = 0; i < record.Length; i++)
        {
            for (int j = 0; j < i; j++)
            {
                if (record[i] < record[j])
                {
                    temp = record[i];
                    record[i] = record[j];
                    record[j] = temp;
                }
            }
        }
    }

    public void ReturnToDesktop()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();  // ÉQÅ[ÉÄÇèIóπ
        #endif
    }

    public void Ranking()
    {
        if (mainMenu.activeSelf)
        {
            buttons.SetActive(false);
            coinRankingText.SetActive(true);
            coinRankingTexts[0].SetText(record[4].ToString() + "coin   1st");
            coinRankingTexts[1].SetText(record[3].ToString() + "coin   2nd");
            coinRankingTexts[2].SetText(record[2].ToString() + "coin   3rd");
            coinRankingTexts[3].SetText(record[1].ToString() + "coin   4th");
            coinRankingTexts[4].SetText(record[0].ToString() + "coin   5th");
        }
    }

    public void BackToGame()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
        if (mainMenu.activeSelf) { Cursor.lockState = CursorLockMode.None; }
        else if (!mainMenu.activeSelf) { Cursor.lockState = CursorLockMode.Locked; }
        Cursor.visible = mainMenu.activeSelf;
        Time.timeScale = 1;
    }

    public void Setting()
    {
        buttons.SetActive(false);
        slider.gameObject.SetActive(true);
    }

    private void MouseSensitivity()
    {
        cameraFollow.MouseSensitivity = slider.value;
    }

    private void FixedUpdate()
    {
        SendText();
        Record();
        MouseSensitivity();
    }

    private void SendText()
    {
        killCountText.SetText(ParameterDate.SendCharacterKillCount(playerController).ToString());
        deathCountText.SetText(ParameterDate.SendCharacterDeathCount(playerController).ToString());
        currentAmmoText.SetText(ParameterDate.SendCurrentAmmo(playerController.CharacterIndex).ToString());
        characterHpText.SetText(ParameterDate.SendCharacterHP(playerController).ToString());
        totalFireAmmoText.SetText(ParameterDate.SendAccuracy(playerController).ToString() + "%");
        coinText.SetText(ParameterDate.SendCharacterCoin(playerController.CharacterIndex).ToString());
        totalAmmoText.SetText(ParameterDate.SendTotalAmmo(playerController.CharacterIndex).ToString());
        dpsText.SetText(ParameterDate.SendDamageCount(playerController.CharacterIndex).ToString());
    }

    private Sprite TextureToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
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
