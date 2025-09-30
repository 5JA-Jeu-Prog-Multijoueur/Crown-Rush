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

        int direction = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1; // Direction de depart au hasard


        while (true) // Toujours en mouvement (avec lerp) - Va vers la gauche/droite pour debuter et ensuite fait des allers-retours entre limiteGauche et limiteDroite
        {
            if (direction == 1)
            { // Deplacement vers la droite
                yield return null;
                Debug.Log("Deplacement etoile vers la droite");
                while (transform.position.x < limiteDroite)
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(limiteDroite, transform.position.y), Time.deltaTime * vitesseEtoile);
                    yield return null;
                }
            }
            else // Deplacement vers la gauche
            {
                yield return null;
                Debug.Log("Deplacement etoile vers la gauche");
                while (transform.position.x > limiteGauche)
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(limiteGauche, transform.position.y), Time.deltaTime * vitesseEtoile);
                    yield return null;
                }
            }

            direction = direction * -1; // Inverse la direction

        }

    }


 
}
