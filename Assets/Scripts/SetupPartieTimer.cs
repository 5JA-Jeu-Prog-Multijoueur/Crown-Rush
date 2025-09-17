using System.Collections;
using TMPro;
using UnityEngine;

public class SetupPartieTimer : MonoBehaviour
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
        yield return new WaitForSeconds(1);
        timerTexte.text = "2";
        yield return new WaitForSeconds(1);
        timerTexte.text = "1";
        yield return new WaitForSeconds(1);
        timerTexte.text = "GO!";
        yield return new WaitForSeconds(0.5f);
        timerTexte.text = "";

        // Lancer le setup de la manche (apparition des blocs, palettes etc.)
        Debug.Log("GameManager onMancheSetup invoke");
        GameManager.onMancheSetup?.Invoke();
    }
}
