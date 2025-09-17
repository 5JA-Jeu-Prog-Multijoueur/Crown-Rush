using UnityEngine;
using Unity.Netcode;


public class Musique : MonoBehaviour
{
    public AudioClip musique;
    private AudioSource audioSource;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = musique;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 0 = son 2D, entendable partout

        if (!audioSource.isPlaying && musique != null)
        {
            audioSource.Play();
        }
    }
}
