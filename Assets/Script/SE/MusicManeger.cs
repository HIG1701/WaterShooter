using UnityEngine;

public class MusicManeger : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSE;
    [SerializeField] private AudioClip dmgSE;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void OnPlayShootSE()
    {
        audioSource.PlayOneShot(shootSE,0.2f);
    }
    public void OnPlayDmgSE()
    {
        audioSource?.PlayOneShot(dmgSE,1f);
    }
}
