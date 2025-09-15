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


  public NetworkVariable<FixedString32Bytes> couleurBoutonHost = new NetworkVariable<FixedString32Bytes>(new FixedString32Bytes("blanc"));
  public NetworkVariable<FixedString32Bytes> couleurBoutonClient = new NetworkVariable<FixedString32Bytes>(new FixedString32Bytes("blanc"));


  public override void OnNetworkSpawn()
  {

    // Abonnement aux changements de la variable réseau
    couleurBoutonHost.OnValueChanged += ChangerCouleurBoutonHost;
    couleurBoutonClient.OnValueChanged += ChangerCouleurBoutonClient;
  }

  public override void OnNetworkDespawn()
  {
    // Désabonnement aux changements de la variable réseau
    couleurBoutonHost.OnValueChanged -= ChangerCouleurBoutonHost;
    couleurBoutonClient.OnValueChanged -= ChangerCouleurBoutonClient;
  }


  private void ChangerCouleurBoutonHost(FixedString32Bytes previousValue, FixedString32Bytes newValue)
  {
    if (newValue.ToString() == "red")
    {
      boutonHost.GetComponent<UnityEngine.UI.Image>().color = Color.red;
    }
    else
    {
      boutonHost.GetComponent<UnityEngine.UI.Image>().color = Color.white;
    }
  }
  private void ChangerCouleurBoutonClient(FixedString32Bytes previousValue, FixedString32Bytes newValue)
  {

    if (newValue.ToString() == "red")
    {
      boutonClient.GetComponent<UnityEngine.UI.Image>().color = Color.red;
    }
    else
    {
      boutonClient.GetComponent<UnityEngine.UI.Image>().color = Color.white;
    }

  }

  public void EstHost()
  {
    NetworkManager.Singleton.StartHost();
    // Mettre le bouton rouge
    couleurBoutonHost.Value = new FixedString32Bytes("red");
  }

  public void EstClient()
  {
    NetworkManager.Singleton.StartClient();
    // Mettre le bouton rouge pour tous
    couleurBoutonClient.Value = new FixedString32Bytes("red");
  }


    private void Update()
  {
    if (!IsHost) return; // Si tu n'es pas l'host (car NetworkBehavior se fait partout)

    if (NetworkManager.Singleton.ConnectedClientsList.Count >= 2) // Si 2 joueurs connectés
    {

      // Remettre les couleurs des boutons
      couleurBoutonHost.Value = new FixedString32Bytes("blanc");
      couleurBoutonClient.Value = new FixedString32Bytes("blanc");
      // Lancement du timer de début de partie
      SceneManager.LoadScene("Jeu");
      // 
    }
  }
}