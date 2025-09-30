using UnityEngine;
using Unity.Netcode;
using System.Collections;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{

    // VARIABLES 
    // Singleton
    public static GameManager instance { private set; get; }

    // Partie
    public bool partieEnCours { private set; get; }
    public bool partieTerminee { private set; get; }
    private bool joueurGagnant = false;

    // Manches
    public bool mancheEnCours { private set; get; }
    public bool mancheTerminee { private set; get; }
    private int qteManches = 0;
    public int qteMaxManches;
    // Tableau des scores
    public GameObject[] scoreHost = new GameObject[3];
    public GameObject[] scoreClient = new GameObject[3];
    public TextMeshProUGUI gagnantTexte;
    private int scoreHostInt = 0;
    private int scoreClientInt = 0;

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
    public static Action etoileTouchee;

    public static string gagnantPartie; // "host" ou "client"


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
            //  DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public override void OnNetworkSpawn()
    {
        Debug.Log("GameManager OnNetworkSpawn");
        base.OnNetworkSpawn();

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        onMancheEnd += finManche;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            onMancheEnd -= finManche;
        }
    }


    private void OnClientConnected(ulong clientId)
    {
        Debug.Log("Qte clients: " + NetworkManager.Singleton.ConnectedClientsList.Count);

        if (!IsHost) return;

        for (int i = 0; i < NetworkManager.Singleton.ConnectedClientsList.Count; i++)
        {
            Debug.Log("Joueur " + i + " ClientID: " + NetworkManager.Singleton.ConnectedClientsList[i].ClientId);
            // Faire apparaitre les palettes des joueurs (host = joueur 1, client = joueur 2, chacun à sa palette)
            GameObject nouveauJoueur = null;
            // En gros, variable vide en haut pour la remplir de la palette qui correspond au joueur
            if (i == 0)
            {
                nouveauJoueur = Instantiate(paletteJoueur1, new Vector2(0, 8.5f), Quaternion.identity);
            }
            else if (i == 1)
            {
                nouveauJoueur = Instantiate(paletteJoueur2, new Vector2(0, -8.5f), Quaternion.identity);
            }

            nouveauJoueur.GetComponent<NetworkObject>().SpawnWithOwnership(NetworkManager.Singleton.ConnectedClientsList[i].ClientId);
            Debug.Log("Palette du joueur " + i + " instanciée.");

        }

        // Ici, le for est fait donc les palettes sont instanciées
        Debug.Log("Partie lancée");
        panelAttente.SetActive(false); // Cacher le panel d'attente
        CacherPanelAttenteClientRpc(); // RPC pour cacher le panel d'attente sur tous les clients
        partieEnCours = true;
        onPartieStart?.Invoke(); // Appel de l'event partout


    }

    // RPC pour cacher le panelAttente sur tous les clients
    [ClientRpc]
    private void CacherPanelAttenteClientRpc()
    {
        panelAttente.SetActive(false);
    }

    // 

    private void Start()
    {
        Debug.Log("GameManager started.");
        gagnantPartie = ""; // Reset le gagnant de la partie
        scoreClientInt = 0;
        scoreHostInt = 0;
        joueurGagnant = false;
    }




    // Fonctions de fin de manche / partie
    private void lancePartie()
    {
        Debug.Log("Lancement de la partie");

        lanceManche();
    }

    public void finManche()
    {

        Debug.Log("Fin de la manche actuelle. Gagnant = "+gagnantPartie);

        // Enlever tout ce qui doit etre enlevé (blocs, balles etc.)
        int layer = LayerMask.NameToLayer("mancheObjets");
        GameObject[] tousObjets = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject objet in tousObjets)
        {
            if (objet.layer == layer)
            {
                Destroy(objet);
            }
        }

        // Si on a un gagnant de manche et il a deja 2 points, on a un gagnant de partie
        if (gagnantPartie == "host")
        {
            if (scoreHostInt == 2) 
            {
                joueurGagnant = true;
            } else
            {
                scoreHost[scoreHostInt].SetActive(true);
                scoreHostInt++;
            }
        }
        else if (gagnantPartie == "client")
        {
            if (scoreClientInt == 2)
            {
                joueurGagnant = true;
            }
            else
            {
                scoreClient[scoreClientInt].SetActive(true);
                scoreClientInt++;
            }
        }

        // Incrémenter le nombre de manches jouées
        qteManches++;
        mancheEnCours = false;
        mancheTerminee = true;

        // If pour fin de partie ( au lieu de fin de manche ) si on a un gagnant 
        if (joueurGagnant)
        {
            qteManches = 0;
            // Set le texte du gagnant sur tous les clients
            setTexteGagnantClientRpc();
            gagnantTexte.GetComponent<Animator>().SetTrigger("apparait");

            // Cacher les scores
            for (int i = 0; i < scoreHost.Length; i++)
            {
                scoreHost[i].SetActive(false);
                scoreClient[i].SetActive(false);
            }
            // Retour au hub
            Invoke("nouvellePartie", 2f);
        }
        else
        {
            // Relance une manche
            Debug.Log("On relance une autre manche, voici la qteManche: " + qteManches);
            lanceManche();
            mancheEnCours = true;
            mancheTerminee = false;
        }
    }

    public void lanceManche()
    {
        Debug.Log("Lancement d'une manche");
        onPartieStart?.Invoke(); // A la place de manche pour re-avoir le timer

        // Script pour apparition des blocs
    }

    // Une fois le script des blocs fini, donne aux joueurs le contrôle de leur palette + lance la balle 

    public void nouvellePartie()
    {
        // Deconnecte tous les joueurs et retourne au hub
        for (int i = 0; i < Network.connections.length; i++)
        {
            Network.CloseConnection(Network.connections[i], true);
        }

        NetworkManager.Singleton.Shutdown();
        NetworkManager.Singleton.SceneManager.LoadScene("Hub", LoadSceneMode.Single);
        SceneManager.LoadScene("Hub"); // Au cas ou le shutdown empeche le Singleton.SceneManager de marcher




    }

    [ClientRpc]
    private void setTexteGagnantClientRpc()
    {
        gagnantTexte.text = "Bravo au " + gagnantPartie + ", tu as gagné la partie !";
    }

}


