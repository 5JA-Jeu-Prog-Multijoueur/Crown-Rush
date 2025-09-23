using UnityEngine;

public class BallSFX : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (NetworkSFXManager.Instance != null)
        {
            NetworkSFXManager.Instance.JoueCollision();
        }
    }
}
