using UnityEngine;
using Unity.Netcode;
using System.Collections;
using TMPro;
using System;

public class GameManager : NetworkBehaviour
{

  // VARIABLES 
  // Singleton
  public static GameManager instance;

  // Partie
  public bool partieEnCours { private set; get; }
  public bool partieTerminee { private set; get; }

  // Manches
  public bool mancheEnCours { private set; get; }
  public bool mancheTerminee { private set; get; }
  private int qteManches = 0;
  public int qteMaxManches;

  // Texte Timer
  public TextMeshProUGUI timerTexte;

  // Panel d'attente de joueurs
  public GameObject panelAttente;


  // Actions
  public static Action onPartieStart;
  public static Action onPartieEnd;
  public static Action onMancheSetup;
  public static Action onMancheStart;
  public static Action onMancheEnd;


  // Palettes des joueurs
  public GameObject paletteJoueur1; // Prefab de la palette du joueur 1
  public GameObject paletteJoueur2; // Prefab de la palette du joueur 2
  // 


  // Création du singleton
  private void Awake()
  {

    if (instance == null)
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  // Fonctions pour attribuer les joueurs a host / client
  public void lancerHost()
  {
    NetworkManager.Singleton.StartHost();
  }
  
  public void lancerClient()
  {
    NetworkManager.Singleton.StartClient();
  }




  public override void OnNetworkSpawn()
  {
    base.OnNetworkSpawn();

    NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
  }

  public override void OnNetworkDespawn()
  {
    base.OnNetworkDespawn();

    if (NetworkManager.Singleton != null)
    {
      NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }
  }


  private void OnClientConnected(ulong clientId)
  {
    Debug.Log("Qte clients: "+ NetworkManager.Singleton.ConnectedClientsList.Count);

    if (!IsServer) return;


    for (int i = 0; i < NetworkManager.Singleton.ConnectedClientsList.Count; i++)
    {
      Debug.Log("Joueur " + i + " ClientID: " + NetworkManager.Singleton.ConnectedClientsList[i].ClientId);
      // Faire apparaitre les palettes des joueurs (host = joueur 1, client = joueur 2, chacun à sa palette)
      GameObject nouveauJoueur = null;
      // En gros, variable vide en haut pour la remplir de la palette qui correspond au joueur
      if (i == 0)
      {
        nouveauJoueur = Instantiate(paletteJoueur1);
      }
      else if (i == 1)
      {
        nouveauJoueur = Instantiate(paletteJoueur2);
      }

      nouveauJoueur.GetComponent<NetworkObject>().SpawnWithOwnership(NetworkManager.Singleton.ConnectedClientsList[i].ClientId);
      Debug.Log("Palette du joueur " + i + " instanciée.");

    }

    // Ici, le for est fait donc les palettes sont instanciées
        Debug.Log("Partie lancée");
    panelAttente.SetActive(false); // Cacher le panel d'attente
    partieEnCours = true;
    onPartieStart?.Invoke(); // Appel de l'event partout


  }



  // 

  private void Start()
  {
    Debug.Log("GameManager started.");
  }




  // Fonctions de fin de manche / partie
  private void lancePartie()
  {
    Debug.Log("Lancement de la partie");

    lanceManche();
  }
  public void finManche()
  {
    Debug.Log("Fin de la manche actuelle");

    qteManches++;
    mancheEnCours = false;
    mancheTerminee = true;

    if (qteManches >= qteMaxManches)
    {
      qteManches = 0;
      finPartie();
    }
    else
    {
      // Relance une manche
      lanceManche();
      mancheEnCours = true;
      mancheTerminee = false;
    }
  }

  public void lanceManche()
  {
    Debug.Log("Lancement d'une manche");

    // Script pour apparition des blocs
    }

    // Une fois le script des blocs fini, donne aux joueurs le contrôle de leur palette + lance la balle 
  
  public void finPartie()
  {
    Debug.Log("Fin de la partie");
  }


}


