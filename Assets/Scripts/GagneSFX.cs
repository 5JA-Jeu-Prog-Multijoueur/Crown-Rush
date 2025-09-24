using UnityEngine;

public class GagneSFX : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (NetworkSFXManager.Instance != null)
        {
            NetworkSFXManager.Instance.JoueGagne();
        }
    }
}
