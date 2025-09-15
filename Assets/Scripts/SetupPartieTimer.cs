using System.Collections;
using TMPro;
using UnityEngine;

public class SetupPartieTimer : MonoBehaviour
{
      public TextMeshProUGUI timerTexte;

    private void OnEnable()
    {
        GameManager.onPartieStart += TimerLancement;
    }

    private void OnDisable()
    {
        GameManager.onPartieStart -= TimerLancement;
    }


    private void TimerLancement()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
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

        // Lancer le setup de la manche (apparition des blocs, palettes etc.)
        GameManager.onMancheSetup?.Invoke();
    }
}
