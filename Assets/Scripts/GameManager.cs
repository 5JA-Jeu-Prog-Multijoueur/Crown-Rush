using UnityEngine;
using Unity.Netcode;
using System.Collections;
using TMPro;

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

  // Blocs // Array des types de blocs à instancier (prefabs)
  public GameObject[] typesDeBlocs;

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


  // 

  private void Start()
  {

    Debug.Log("GameManager started.");
  }

  private void Update()
  {
    if (IsHost) // // // // // // REMETTRE LE ! AVANT IsHost APRES TESTS
    {
      return;
    } 

    if (partieEnCours) // Si une partie est en cours, return
    {
      return;
    } 


    // if (NetworkManager.Singleton.ConnectedClientsList.Count >= 2) // Donc si tu es l'host, et qu'il n'y a pas de partie active, et que les 2 joueurs sont connectés
    if (true)
    {
      Debug.Log("Démarrage de la partie");
      partieEnCours = true;
      // Lancement du timer de début de partie
      StartCoroutine(TimerLancement());
      // 
    }
  }


  // Coroutine pour un timer de 3 secondes
  private IEnumerator TimerLancement()
  {
    timerTexte.text = "3";
    yield return new WaitForSeconds(1);
    timerTexte.text = "2";
    yield return new WaitForSeconds(1);
    timerTexte.text = "1";
    yield return new WaitForSeconds(1);
    timerTexte.text = "GO!";
    yield return new WaitForSeconds(0.5f);
    timerTexte.text = "";
    lancePartie();
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
    StartCoroutine(ApparitionBlocs());

    // Une fois le script des blocs fini, donne aux joueurs le contrôle de leur palette + lance la balle 
  }
  public void finPartie()
  {
    Debug.Log("Fin de la partie");
  }

  private IEnumerator ApparitionBlocs()
  {
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

    if (IsHost)
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
      Instantiate(typesDeBlocs[typeDeBloc], new Vector2(coordBloc.x, coordBloc.y), Quaternion.identity); // Quaternion.identity pour aucune rotation (2D)

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

    yield return new WaitForSeconds(0.5f);
  }
    
}


