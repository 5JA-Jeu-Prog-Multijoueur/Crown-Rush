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
        float direction = UnityEngine.Random.Range(0, 2) == 0 ? -1f : 1f;

        while (true) // Toujours en mouvement (avec lerp) - Va vers la gauche/droite pour debuter et ensuite fait des allers-retours entre limiteGauche et limiteDroite
        {
            // Deplace dans la direction random choisie jusqu'a l'une des deux limites

            if (direction == -1) // Vers la gauche
            {
                while (transform.position.x >= limiteGauche)
                {
                    transform.position = Vector2.Lerp(transform.position, new Vector2(limiteGauche, transform.position.y), Time.deltaTime * vitesseEtoile); // Mouvement
                    yield return null;
                }
                direction = 1; // Change de direction
            }
            else // Vers la droite
            {
                while (transform.position.x <= limiteDroite)
                {
                    transform.position = Vector2.Lerp(transform.position, new Vector2(limiteDroite, transform.position.y), Time.deltaTime * vitesseEtoile); // Mouvement
                    yield return null;
                }
                direction = -1; // Change de direction
            }



        }

        Debug.Log("Etoile a fini de bouger");
    }


    
}
