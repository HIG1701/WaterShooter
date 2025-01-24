using UnityEngine;
using UnityEngine.UI;

public class HPBarAnim : MonoBehaviour
{
    private RawImage rawImage;

    private const float maxHPBar = 0.51f;
    private float maxHP;
    [SerializeField] private float currentDamageBar = 0f;
    [SerializeField] private float nextDamageBar;
    [SerializeField] private float speed = 2f;
    [SerializeField] private PlayerController playerController;

    private void Start()
    {
        rawImage = GetComponent<RawImage>();
        rawImage.uvRect = new Rect(0.005f, 0f, 0.49f, 1f);

        maxHP = playerController.SendPlayerParameter.PlayerHP;

        currentDamageBar = CalculateBarPosition(PlayerManager.parameters[playerController.CharacterIndex].playerRuntimeDate.date.playerHP);
        nextDamageBar = currentDamageBar;
    }

    private void Update()
    {
        HPBarDateRef();
    }

    private float CalculateBarPosition(float hp)
    {
        return Mathf.Clamp(maxHPBar - (hp / maxHP) * maxHPBar, 0.005f, maxHPBar);
    }

    private void HPBarDateRef()
    {
        nextDamageBar = CalculateBarPosition(PlayerManager.parameters[playerController.CharacterIndex].playerRuntimeDate.date.playerHP);
        currentDamageBar = Mathf.Lerp(currentDamageBar, nextDamageBar, speed * Time.deltaTime);
        rawImage.uvRect = new Rect(currentDamageBar, 0f, 0.49f, 1f);
    }
}