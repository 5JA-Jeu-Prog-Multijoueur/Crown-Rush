using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class Lancement : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Lancement de la scène de HUB dès le lancement de l'application
            SceneManager.LoadScene("Hub");

    }

}
