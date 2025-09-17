using UnityEngine;
using Unity.Netcode;

public class Balle : NetworkBehaviour
{

    public float vitesseBalle = 2f;

    private Vector2 positionDepart;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        Debug.Log("Balle Start de : " + gameObject.name);

        // Balle: Bouge en ligne droite, pas affecter par la gravite et aucune friction de l'air


        positionDepart = transform.position;
        gameObject.SetActive(true);
        GetComponent<Rigidbody2D>().gravityScale = 0;
        

        // Force vers le bas si en bas de l'écran, vers le haut si en haut
        if (positionDepart.y < 0)
        {
            Debug.Log("Balle lancée vers le bas dans 2 secondes");
            Invoke("pousseBalles", 2f);
        }
        else
        {
            Debug.Log("Balle lancée vers le haut dans 2 secondes");
            Invoke("pousseBalles", 2f);
        }
    }

    private void pousseBalles()
    {
        Debug.Log("Pousse les balles");
        System.Random random = new System.Random();
        float aleaX = random.Next(0, 2) == 0 ? -vitesseBalle : vitesseBalle;
        float aleaY = random.Next(0, 2) == 0 ? -vitesseBalle : vitesseBalle;

        if (positionDepart.y < 0)
        {
            aleaY = Mathf.Abs(aleaY); // Force vers le haut
        }
        else
        {
            aleaY = -Mathf.Abs(aleaY); // Force vers le bas
        }

        GetComponent<Rigidbody2D>().AddForce(new Vector2(aleaX, aleaY), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        // Puis on donne/deal avec les effets selon le type de bloc touché (+ delete objet toucher)
        switch (collision.gameObject.tag)
        {
            case "Normal":
                Debug.Log("Balle a touché un bloc normal");
                // RIEN
                Destroy(collision.gameObject);
                break;
            case "DoubleHP":
                Debug.Log("Balle a touché un bloc doubleHP");
                // Change le tag + sprite en bloc normal
                collision.gameObject.tag = "Normal";
                collision.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Modele_Bloc_Base_0");

                break;
            case "Cactus":
                Debug.Log("Balle a touché un bloc cactus");
                // On ralentit la balle de 15% pour 5 secondes
                GetComponent<Rigidbody2D>().linearVelocity = GetComponent<Rigidbody2D>().linearVelocity * 0.85f;
                // Fonction dans 5 secondes pour reset la vitesse
                Invoke("ResetVitesse", 5f);
                Destroy(collision.gameObject);

                break;
            case "Speed":
                Debug.Log("Balle a touché un bloc speed");
                // On accélère la balle de 15%
                GetComponent<Rigidbody2D>().linearVelocity = GetComponent<Rigidbody2D>().linearVelocity * 1.15f;
                Destroy(collision.gameObject);
                // Fonction dans 5 secondes pour reset la vitesse
                Invoke("ResetVitesse", 5f);
                break;
            default:
                Debug.Log("Balle a touché un objet non géré : " + collision.gameObject.name + " avec le tag: " + collision.gameObject.tag);
                break;
        }




    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si on touche le vide (HorsJeu)
        if (collision.gameObject.tag == "HorsJeu")
        {
            // Desactive la balle, 5 secondes d'attente, replace au position de depart 
            gameObject.SetActive(false);
            Invoke("RepositionnerBalle", 5f);

        }
    }

    private void ResetVitesse()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = rb.linearVelocity * vitesseBalle;
    }

    private void RepositionnerBalle()
    {
        // Replace la balle au centre, reset la vitesse, et réactive la balle
        gameObject.SetActive(true);
        transform.position = positionDepart;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        System.Random random = new System.Random();
        float aleaX = random.Next(0, 2) == 0 ? -vitesseBalle : vitesseBalle;
        float aleaY = random.Next(0, 2) == 0 ? -vitesseBalle : vitesseBalle;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(aleaX, aleaY), ForceMode2D.Impulse);
    }
}
