using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class LoadSceneNetwork : MonoBehaviour
{
    [SerializeField] private string nomScene;

    public void ChargerScene()
    {
        if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
        {
            // Host / Serveur : on synchronise la scène avec tous les clients
            NetworkManager.Singleton.SceneManager.LoadScene(nomScene, LoadSceneMode.Single);
        }
        else
        {
            // Si jamais un client clique par erreur, on l'ignore
            Debug.Log("Seul le Host/Serveur peut changer de scène.");
        }
    }
}
