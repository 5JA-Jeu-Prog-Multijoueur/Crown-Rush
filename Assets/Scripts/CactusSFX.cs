using UnityEngine;

public class CactusSFX : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (NetworkSFXManager.Instance != null)
        {
            NetworkSFXManager.Instance.JoueCactus();
        }
    }
}
