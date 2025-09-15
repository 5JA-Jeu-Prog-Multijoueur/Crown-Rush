using UnityEngine;
using Unity.Netcode;

public class paletteMouvement : NetworkBehaviour
{

    public int limiteGauche;
    public int limiteDroite;

    public Vector2 positionPaletteHOSTDepart;
    public Vector2 positionPaletteCLIENTDepart;


    public override void OnNetworkSpawn()
    {
        // Les positionnes aux positions de départ
        if (IsServer)
        {
            transform.position = positionPaletteHOSTDepart;
        }
        else
        {
            transform.position = positionPaletteCLIENTDepart;
        }
    }
    void Update()
    {
        // Gerer les inputs
        deplacement();
    }


    void deplacement()
    {
        // Mouvement gauche et droite A et D 
        float horizontal = Input.GetAxis("Horizontal"); // Valeur entre -1 et 1 avec les touches (A et D)
        Vector3 position = transform.position; // Position actuelle
        position.x += horizontal * 10f * Time.deltaTime; // Vitesse de déplacement
        position.x = Mathf.Clamp(position.x, limiteGauche, limiteDroite); // Bouger mais ne pas dépasser les limites
        transform.position = position; // Bouger l'objet
    }
}
