using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    //‰¹–Â‚ç‚·‚¾‚¯
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void OnPlayMusic()
    {
        audioSource.Play();
    }
}
