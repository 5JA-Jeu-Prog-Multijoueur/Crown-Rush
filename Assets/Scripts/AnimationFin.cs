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

    public void animationFinLancement(string gagnant)
    {
        if (gagnant == "host")
        {
            texte.text = hostWin;
        }
        else
        {
            texte.text = clientWin;
        }

        this.GetComponent<Animator>().SetTrigger("lancement");
        Invoke("animationFinRangement", 2f);

    }

    private void animationFinRangement()
    {
        Debug.Log("Animation de fin terminée");

        this.GetComponent<Animator>().SetTrigger("fin");
        // Dire au GameManager que la partie est terminée apres un délais de 2 secondes
        Invoke("finDeLaPartie", 2f);

    }

    private void finDeLaPartie()
    {
                GameManager.onPartieEnd?.Invoke();
    }

}
