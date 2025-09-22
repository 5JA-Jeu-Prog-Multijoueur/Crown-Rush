using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System;

public class Etoile : NetworkBehaviour
{

    

    public float limiteGauche;
    public float limiteDroite;
    public float vitesseEtoile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Debug.Log("Etoile est en ligne");
        StartCoroutine(bougerEtoile());
    }

    // Deplacement gauche-droite de l'etoile (en restant dans les limites)
    IEnumerator bougerEtoile()
    {

        bool direction = UnityEngine.Random.Range(0, 2) == 0 ? true : false; // Direction de depart au hasard
        float lerpProgress = 0.0f; // Variable de progression du lerp


        while (true) // Toujours en mouvement (avec lerp) - Va vers la gauche/droite pour debuter et ensuite fait des allers-retours entre limiteGauche et limiteDroite
        {

            // Deplace dans la direction random choisie jusqu'a l'une des deux limites
            if (direction) // Va a droite
            {
                lerpProgress += Time.deltaTime * vitesseEtoile;
                if (lerpProgress >= 1.0f)
                {
                    lerpProgress = 1.0f; // Set a 1 pour s'assurer qu'il atteint la fin
                    direction = false; // Change direction
                }
            }
            else // Va a gauche
            {
                lerpProgress -= Time.deltaTime * vitesseEtoile;
                if (lerpProgress <= 0.0f)
                {
                    lerpProgress = 0.0f; // Set a 0 pour s'assurer qu'il atteint la fin
                    direction = true; // Change direction
                }
            }

            // Calculer la position cible en utilisant le lerp  
            float targetX = Mathf.Lerp(limiteGauche, limiteDroite, lerpProgress);

            // Mouvement
            transform.position = new Vector3(targetX, transform.position.y, transform.position.z);


        }
        yield return new WaitForSeconds(0.5f); // Petite pause avant de repartir

    }


 
}
