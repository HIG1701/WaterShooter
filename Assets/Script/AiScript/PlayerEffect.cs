using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayerEffect : MonoBehaviour
{
    [Header("エフェクト透明度"), SerializeField] private float transparency = 0.15f;
    [Header("透明化速度"), SerializeField] private float transparencySpeed = 0.01f;
    private VideoPlayer damageHit;
    private PlayerController playerController;
    private int currentHP = 0;
    private bool isHit;

    private void Start()
    {
        damageHit = GetComponent<VideoPlayer>();
        playerController = transform.parent.GetComponentInChildren<PlayerController>();
        currentHP = ParameterDate.SendCharacterHP(playerController);
    }

    private IEnumerator DamageHitEffect()
    {
        if (isHit) { yield break; }
        isHit = true;
        while (damageHit.targetCameraAlpha < transparency)
        {
            damageHit.targetCameraAlpha += transparencySpeed;
            yield return null;
        }
        while (damageHit.targetCameraAlpha >= 0)
        {
            damageHit.targetCameraAlpha -= transparencySpeed;
            yield return null;
        }
        damageHit.targetCameraAlpha = 0;
        isHit = false;
    }

    private void Update()
    {
        float hp = ParameterDate.SendCharacterHP(playerController);
        if (currentHP > hp)
        {
            StartCoroutine(DamageHitEffect());
        }
        currentHP = ParameterDate.SendCharacterHP(playerController);
    }
}
