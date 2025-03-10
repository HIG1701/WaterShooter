using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadShoot : MonoBehaviour
{
    public int characterIndex = 0;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeTime = 0.2f;

    public void HeadHit(GunParameter gunParameter, int CharaIndex, GameObject hitParticle)
    {
        var parentObject = transform.parent;
        if (parentObject.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            //プレイヤーに対してダメ加算.
            SetDateManager.HitToCharacter(playerController, gunParameter, CharaIndex);
            SetDateManager.SetHP(CharaIndex, 5);
            StartCoroutine(FadeInOut());
        }
        else if (parentObject.gameObject.TryGetComponent<AIControl>(out AIControl playerAiController))
        {
            //BOTに対してダメ加算.

            GameObject particleInstance = Instantiate(hitParticle, gameObject.transform.position, Quaternion.identity);
            SetDateManager.SetHP(CharaIndex, 5);
            particleInstance.GetComponentInChildren<ParticleSystem>().Play();
            StartCoroutine(FadeInOut());
        }
    }

    private IEnumerator FadeInOut()
    {
        yield return StartCoroutine(Fade(1f, 0f, fadeTime));

        yield return StartCoroutine(Fade(0f, 1f, fadeTime));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
    }
}
