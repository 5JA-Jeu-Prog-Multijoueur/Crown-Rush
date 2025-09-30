using UnityEngine;
using Unity.Netcode;
using TMPro;

public class AnimationFinScript : NetworkBehaviour
{
    public GameObject panelHaut;
    public GameObject panelBas;
    public TextMeshProUGUI texte;
    private string hostWin = "Le joueur Hôte a gagné la partie !";
    private string clientWin = "Le joueur Client a gagné la partie !";
    private string gagnantPartie;

    // S'abonner à etoileTouchee
    private void Awake()
    {
        GameManager.etoileTouchee += animationFinLancement;
    }
    private void OnNetworkDisable()
    {
        GameManager.etoileTouchee -= animationFinLancement;
    }

    public void animationFinLancement()
    {
        if (!IsHost) return;
        Debug.Log("Animation de fin lancée");

        Debug.Log("Gagnant de la partie: " + GameManager.gagnantPartie);

        gagnantPartie = GameManager.gagnantPartie;
        // Lancer RPC 
        ChangeTexteClientRpc(gagnantPartie);
        // Lancer l'animation
        this.GetComponent<Animator>().SetTrigger("lancement");
        // Animation pour retirer les panels 
        Invoke("animationFinRangement", 2f);

    }

    private void animationFinRangement()
    {
        Debug.Log("Animation de fin terminée");

        this.GetComponent<Animator>().SetTrigger("fin");
        Invoke("finDeLaManche", 2f);

    }

    private void finDeLaManche()
    {
        Debug.Log("Fin de partie!");
        ResetAnimationTexteClientRpc(); // Reset le texte de l'animation

        GameManager.onMancheEnd?.Invoke(); // Active la sequence de fin de la manche
    }

    [ClientRpc]
    private void ChangeTexteClientRpc(string gagnant)
    {
        if (gagnant == "host")
        {
            texte.text = hostWin;
        }
        else
        {
            texte.text = clientWin;
        }
    }

    [ClientRpc]
    public void ResetAnimationTexteClientRpc()
    {
        texte.text = "";
    }

}
