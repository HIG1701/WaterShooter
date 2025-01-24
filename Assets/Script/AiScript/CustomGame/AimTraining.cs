using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AimTraining : MonoBehaviour
{
    [SerializeField] private int levelUpPoint = 2;
    [SerializeField] private int killCount = 0;
    [SerializeField] private int killStrengthCount = 0;
    [SerializeField] private int setTime = 30;
    private int gameTimer = 0;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI leveltext;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private int level;
    [SerializeField] private bool isGameStart;
    [SerializeField] private int maxlevel = 0;
    private int endLevel;
    [SerializeField] public int[] record = new int[5];

    private void Start()
    {
        gameTimer = setTime;
        Ranking();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        StartCoroutine(CustomGame());
        CustomGameManager();

    }

    private void Ranking()
    {
        int temp = 0;
        for (int i = 0; i < record.Length; i++)
        {
            for (int j = 0; j < i; j++)
            {
                Debug.Log("sas");
                if (record[i] < record[j])
                {
                    temp = record[i];
                    record[i] = record[j];
                    record[j] = temp;
                }
            }
        }
    }

    private void CustomGameManager()
    {
        leveltext.SetText("Level " + level);
        if (!isGameStart) { return; }
        killStrengthCount = PlayerManager.parameters[playerController.CharacterIndex].playerManagementDate.date.killCount;
        var thisKillillCount = killStrengthCount - killCount;
        if (thisKillillCount >= levelUpPoint)
        {
            level++;
            gameTimer = gameTimer + setTime;
            killCount = PlayerManager.parameters[playerController.CharacterIndex].playerManagementDate.date.killCount;

            if (record[0] < level)
            {
                record[0] = level;
                Ranking();
            }
        }
    }

    private IEnumerator CustomGame()
    {
        if (isGameStart) { yield break; }
        killCount = PlayerManager.parameters[playerController.CharacterIndex].playerManagementDate.date.killCount;
        isGameStart = true;
        while (gameTimer >= 0 && isGameStart)
        {
            timer.SetText("Time " + gameTimer.ToString());
            yield return new WaitForSeconds(1);
            gameTimer--;
        }

        level = 0;
        killCount = 0;
        isGameStart = false;
        gameTimer = setTime;
        endLevel = level;
        yield break;
    }
}
