using UnityEngine;

public class Balle : MonoBehaviour
{
    public float vitesseInitiale = 5f;
    private Vector2 positionDepart;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        Debug.Log("Balle Start de : " + gameObject.name);

        // Balle: Bouge en ligne droite, pas affecter par la gravite et aucune friction de l'air


        positionDepart = transform.position;
        gameObject.SetActive(true);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;

        // Force vers le bas si en bas de l'écran, vers le haut si en haut
        if (positionDepart.y < 0)
        {
            rb.AddForce(new Vector2(1, -1).normalized * vitesseInitiale, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(new Vector2(1, 1).normalized * vitesseInitiale, ForceMode2D.Impulse);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        // Peut importe la collision, on rebondit la balle
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 normal = collision.contacts[0].normal;
        Vector2 newDirection = Vector2.Reflect(rb.linearVelocity.normalized, normal);
        rb.linearVelocity = newDirection * rb.linearVelocity.magnitude;

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
                collision.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Modele_Bloc_Base");

                break;
            case "Cactus":
                Debug.Log("Balle a touché un bloc cactus");
                // On ralentit la balle de 15% pour 5 secondes
                rb.linearVelocity = rb.linearVelocity * 0.85f;
                // Fonction dans 5 secondes pour reset la vitesse
                Invoke("ResetVitesse", 5f);
                Destroy(collision.gameObject);

                break;
            case "Speed":
                Debug.Log("Balle a touché un bloc speed");
                // On accélère la balle de 15%
                rb.linearVelocity = rb.linearVelocity * 1.15f;
                Destroy(collision.gameObject);
                // Fonction dans 5 secondes pour reset la vitesse
                Invoke("ResetVitesse", 5f);
                break;
            default:
                Debug.Log("Balle a touché un objet non géré : " + collision.gameObject.tag);
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
        rb.linearVelocity = rb.linearVelocity.normalized * vitesseInitiale;
    }

    private void RepositionnerBalle()
    {
        // Replace la balle au centre, reset la vitesse, et réactive la balle
        transform.position = positionDepart;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(1, 1).normalized * vitesseInitiale, ForceMode2D.Impulse);
        gameObject.SetActive(true);
    }
}
