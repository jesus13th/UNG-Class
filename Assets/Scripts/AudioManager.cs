using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance => instance ?? (instance = FindObjectOfType<AudioManager>());

    [SerializeField] private AudioSource audioSource;

    private void Awake() => instance = this;
    public void PlaySound(AudioClip clip) => audioSource.PlayOneShot(clip);
}
