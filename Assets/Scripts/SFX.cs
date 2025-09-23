using UnityEngine;
using Unity.Netcode;

public class NetworkSFXManager : NetworkBehaviour
{
    public static NetworkSFXManager Instance;

    // Composant AudioSource
    public AudioSource audioSource;

    // Structure pour stocker les effets sonores
    [System.Serializable]
    public class SoundEffect
    {
        public string nom;
        public AudioClip clip;
    }
    public SoundEffect[] soundEffects;

    // Detruire les doubles
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ------ Fonctions globale pour jouer les sons ------ //
    // Jouer un son seulement en local
    public void PlaySFXLocal(string soundName)
    {
        SoundEffect sfx = System.Array.Find(soundEffects, s => s.nom == soundName);
        if (sfx != null && sfx.clip != null)
        {
            audioSource.PlayOneShot(sfx.clip);
        }
        else
        {
            Debug.LogWarning("SFX non trouvé : " + soundName);
        }
    }

    // Jouer un son global en réseau
    public void PlaySFXGlobal(string soundName)
    {
        if (IsServer)
        {
            PlaySFXClientRpc(soundName);
        }
        else
        {
            PlaySFXServerRpc(soundName);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void PlaySFXServerRpc(string soundName)
    {
        PlaySFXClientRpc(soundName);
    }

    [ClientRpc]
    private void PlaySFXClientRpc(string soundName)
    {
        PlaySFXLocal(soundName); // réutilise la fonction locale
    }

    // ------ Fonctions spécifique pour jouer les sons ------ //
    public void JoueBouttonHover()
    {
        // Pitch normal
        audioSource.pitch = 1f;
        PlaySFXLocal("Select_Menu");
        audioSource.pitch = 1f; // reset
    }

    public void JoueBouttonClick()
    {
        // Pitch plus bas
        audioSource.pitch = 1.5f;
        PlaySFXLocal("Select_Menu");
    }

    public void JoueCollision()
    {
        // Pitch aléatoire entre 0.8 et 1.2
        audioSource.pitch = Random.Range(0.8f, 1.2f);                           // Fonctionne???
        PlaySFXGlobal("Collision");
        audioSource.pitch = 1f; // reset
    }
}
