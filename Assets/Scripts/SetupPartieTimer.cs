using System.Collections;
using TMPro;
using UnityEngine;
using Unity.Netcode;

public class SetupPartieTimer : NetworkBehaviour
{
    public TextMeshProUGUI timerTexte;

    public void Awake()
    {
        GameManager.onPartieStart += TimerLancement;
    }

    public void OnDisable()
    {
        GameManager.onPartieStart -= TimerLancement;
    }


    private void TimerLancement()
    {
        Debug.Log("Lancement du timer de départ");
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        Debug.Log("Lancement du timer de départ");
        timerTexte.text = "3";
        MettreAJourTimerClientRpc("3");
        yield return new WaitForSeconds(1);
        timerTexte.text = "2";
        MettreAJourTimerClientRpc("2");
        yield return new WaitForSeconds(1);
        timerTexte.text = "1";
        MettreAJourTimerClientRpc("1");
        yield return new WaitForSeconds(1);
        timerTexte.text = "GO!";
        MettreAJourTimerClientRpc("GO!");
        yield return new WaitForSeconds(0.5f);
        timerTexte.text = "";
        MettreAJourTimerClientRpc("");

        // Lancer le setup de la manche (apparition des blocs, palettes etc.)
        Debug.Log("SetupPartieTimer onMancheSetup invoke");
        GameManager.onMancheSetup?.Invoke();
    }

    // RPC pour mettre à jour le texte du timer sur tous les joueurs incluant l'host
    [ClientRpc]
    private void MettreAJourTimerClientRpc(string texte)
    {
        timerTexte.text = texte;
        
    }



}
