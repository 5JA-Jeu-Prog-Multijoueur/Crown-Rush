using UnityEngine;

public class EclairSFX : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (NetworkSFXManager.Instance != null)
        {
            NetworkSFXManager.Instance.JoueEclair();
        }
    }
}
