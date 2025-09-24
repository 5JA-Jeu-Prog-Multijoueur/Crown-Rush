using UnityEngine;
using Unity.Netcode;
using TMPro;

public class NewMonoBehaviourScript : NetworkBehaviour
{
    public GameObject panelHaut;
    public GameObject panelBas;
    public TextMeshProUGUI texte;

    private string hostWin = "Le joueur Hôte a gagné la partie !";
    private string clientWin = "Le joueur Client a gagné la partie !";

    public void animationFinLancement(string gagnant)
    {
        Debug.Log("Animation de fin de partie pour " + gagnant);

        texte.text = gagnant == "host" ? hostWin : clientWin;
        MettreAJourTexteClientRpc(gagnant);

        if (IsServer)
        {
            deplacementPanelClientRpc();
        }

    }

    // RPC pour que le texte s'affiche chez tout le monde
    [ClientRpc]
    private void MettreAJourTexteClientRpc(string gagnant)
    {
        texte.text = gagnant == "host" ? hostWin : clientWin;
    }

    [ClientRpc]
    private void deplacementPanelClientRpc()
    {
        // Haut se deplace vers la gauche, bas vers la droite
        panelHaut.transform.Translate(Vector3.left * Time.deltaTime * 200);
        panelBas.transform.Translate(Vector3.right * Time.deltaTime * 200);
        Invoke("arretAnimationClientRpc", 2f);

    }

    [ClientRpc]
    private void arretAnimationClientRpc()
    {
        CancelInvoke("deplacementPanelClientRpc");

        Invoke("inverseDeplacementPanelClientRpc", 5f);
    }

    [ClientRpc]
    private void inverseDeplacementPanelClientRpc()
    {
        panelHaut.transform.Translate(Vector3.right * Time.deltaTime * 200);
        panelBas.transform.Translate(Vector3.left * Time.deltaTime * 200);
        Invoke("arretInverseAnimationRpc", 2f);
    }


    [ClientRpc]
    private void arretInverseAnimationClientRpc()
    {
        if (IsServer) // Pour que seulement le serveur fasse la fin de manche
        {
            GameManager.onMancheEnd?.Invoke(); // Fin de manche. On reset le jeu
        }
        CancelInvoke("inverseDeplacementPanelClientRpc");
        gameObject.SetActive(false); // Desactive l'animation de fin
    }

}
