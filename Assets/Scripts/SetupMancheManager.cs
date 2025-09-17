using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class SetupMancheManager : NetworkBehaviour
{

    // Blocs // Array des types de blocs à instancier (prefabs)
    public GameObject[] typesDeBlocs;


    // Balle 
    public GameObject balle;
    public Vector2 positionBalleHOSTDepart;
    public Vector2 positionBalleCLIENTDepart;

    // Variables de progression du setup
    private bool setupBlocsFini = false;

    private void Awake()
    {
        GameManager.onMancheSetup += SetupManche;
        GameManager.onMancheStart += StartManche;
    }

    private void OnNetworkDisable()
    {
        GameManager.onMancheSetup -= SetupManche;
        GameManager.onMancheStart -= StartManche;
    }

    private void SetupManche()
    {
        Debug.Log("Setup de la manche");
        // Apparition des blocs
        if (IsServer) // Que le serveur doit faire apparaitre les choses
        {

            Debug.Log("Setup de la manche lance coroutines");
            StartCoroutine(ApparitionBlocs("host"));
            StartCoroutine(ApparitionBlocs("client"));

        }

    }

    private IEnumerator ApparitionBlocs(string joueur)
    {
        Debug.Log("Apparition des blocs commencée pour " + joueur);
        setupBlocsFini = false;
        // Variables générales
        bool apparitionEnCours = true;

        // Qte max de blocs et de lignes
        int qteMaxBlocsParLigne = 13;
        int qteMaxLignes = 8;

        // Qte actuelle de blocs et de lignes
        int qteBlocsParLigne = 1;
        int qteLignes = 1;

        // Variable des blocs
        Vector2 coordBloc = new Vector2(0, 0);
        int directionX = 0;
        int directionY = 0;
        float deplacementEnX = 1.02f; // Largeur des blocs
        float deplacementEnY = 0.42f; // Hauteur des blocs

        // "Host" est tjrs en haut, Client en bas

        if (joueur == "host")
        {
            // Variables en haut. Donc déplacement de gauche à droite et haut en bas
            coordBloc = new Vector2(0, 0);
            directionX = 1;
            directionY = -1;
        }
        else
        {
            // Variables en bas. Donc déplacement de droite à gauche et bas en haut
            coordBloc = new Vector2(8f, -4f);
            directionX = -1;
            directionY = 1;
        }

        // Apparition des blocs
        while (apparitionEnCours)
        {
            // 1: Détermine le type de bloc 
            // Random.Range(0, longueur de l'array typesDeBlocs)
            int typeDeBloc = Random.Range(0, typesDeBlocs.Length);


            // 2: Apparaitre un bloc (comme ça le 1er est aussi randomized)

            // Instancie le bloc aux coordonnées actuelles
            GameObject nouveauBloc = Instantiate(typesDeBlocs[typeDeBloc], new Vector2(coordBloc.x, coordBloc.y),Quaternion.identity); // Quaternion.identity pour aucune rotation (2D)
            nouveauBloc.GetComponent<NetworkObject>().Spawn(); // Spawn le bloc en réseau

            Debug.Log(
              "Bloc apparu! Coords: " + coordBloc + " Type: " + typeDeBloc +
              " | Qte blocs actuelle: " + qteBlocsParLigne +
              " Qte lignes actuelle: " + qteLignes);

            qteBlocsParLigne++;

            // 3: Check si on dépasse la fin de la ligne (13 blocs placés, rendu au 14) / Fin des lignes (8 lignes)
            if (qteBlocsParLigne > qteMaxBlocsParLigne)
            {
                // 3.1.B | Fin de ligne attente: Change de ligne, change la direction, et incrémente de compteur de lignes
                qteLignes++; // Change la qte de ligne à laquelle on est
                qteBlocsParLigne = 1; // Est le 1er bloc de la next ligne

                coordBloc.y += directionY * deplacementEnY; // Change la hauteur des blocs
                directionX *= -1; // Change la direction du placement
                                  // 3.1.B.2 | Fin des lignes total: Arrêter de placer des blocs
                if (qteLignes > qteMaxLignes)
                {
                    apparitionEnCours = false;
                    break;
                }
            }
            else
            {
                // 3.1 Si on attend ici, ça veut dire des blocs dans la même ligne donc on change le X

                // 4: Change les coordonnées du prochain bloc
                coordBloc.x += directionX * deplacementEnX;
            }


            // Attend 0.1 seconde
            yield return new WaitForSeconds(0.1f); // 1 seconde / bloc pour testing
        } // Le while se refait

        Debug.Log("Apparition des blocs terminée pour " + joueur);
        setupBlocsFini = true;
        yield return new WaitForSeconds(0.5f);
    }



    void Update()
    {
        // Check si le setup est fini
        if (setupBlocsFini)
        {
            Debug.Log("Setup de la manche terminé pour " + (IsHost ? "host" : "client"));

            // Lancer la manche
            GameManager.onMancheStart?.Invoke();

            // Reset les variables pour la prochaine manche
            setupBlocsFini = false;
        }
    }


    private void StartManche()
    {
        Debug.Log("Manche commencée pour " + (IsHost ? "host" : "client"));

        // Apparition des 2 balles (1 par joueur)

        if (IsServer) // Que le serveur doit faire apparaitre les choses
        {
            // Instancie la balle en haut
            GameObject balleHost = Instantiate(balle, positionBalleHOSTDepart, Quaternion.identity);
            balleHost.GetComponent<NetworkObject>().Spawn();

            GameObject balleClient = Instantiate(balle, positionBalleCLIENTDepart, Quaternion.identity);
            balleClient.GetComponent<NetworkObject>().Spawn();
        }
    }
}
