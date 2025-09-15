using Unity.Collections;
using Unity.Netcode;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;



public class HubManager : NetworkBehaviour
{

  // Les boutons de connection
  public GameObject boutonHost;
  public GameObject boutonClient;


  public void EstHost()
  {
    NetworkManager.Singleton.StartHost();
  }

  public void EstClient()
  {
    NetworkManager.Singleton.StartClient();
  }


    private void Update()
  {
    if (!IsHost) return; // Si tu n'es pas l'host (car NetworkBehavior se fait partout)

    if (NetworkManager.Singleton.ConnectedClientsList.Count >= 2) // Si 2 joueurs connectés
    {
      NetworkManager.Singleton.SceneManager.LoadScene("Jeu", LoadSceneMode.Single);
      // 
    }
  }
}