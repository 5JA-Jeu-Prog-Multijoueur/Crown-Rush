using UnityEngine;
using Unity.Netcode;

public class Blocs : NetworkBehaviour
{
    public override void OnNetworkDespawn()
    {
        gameObject.SetActive(false);
    }
}
