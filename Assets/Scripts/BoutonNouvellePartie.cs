using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class LoadSceneNetwork : NetworkBehaviour
{
    [SerializeField] private string nomScene;

    public void ChargerScene()
    {
        if (!IsHost) return; // Si tu n'es pas l'host (car NetworkBehavior se fait partout)

        if (NetworkManager.Singleton.ConnectedClientsList.Count >= 2) // Si 2 joueurs connectés
        {
        Debug.Log("2 joueurs connectés, lancement de la partie");
        NetworkManager.Singleton.SceneManager.LoadScene("Jeu", LoadSceneMode.Single);
        // 
        }
    }
}
