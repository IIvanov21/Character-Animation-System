using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    float horizontalInput, verticalInput;
    Vector3 movement;
    public float playerSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        horizontalInput = Input.GetAxis("Horizontal");//Value between -1 and 1 on x axis
        verticalInput = Input.GetAxis("Vertical");//Value between -1 and 1 on y axis

        movement.x=horizontalInput;
        movement.y = 0.0f;
        movement.z =verticalInput;

        transform.Translate(movement*Time.deltaTime*playerSpeed);

        if(IsOwner && Input.GetKeyDown(KeyCode.T))
        {
            TestServerRpc();
        }

        if(IsServer && Input.GetKeyDown(KeyCode.Y))
        {
            TestClientRpc();
        }

    }

    [ServerRpc]
    void TestServerRpc()
    {
        Debug.Log($"Server RPc called by client {OwnerClientId}");
    }

    [ClientRpc]
    void TestClientRpc()
    {
        Debug.Log("ClientRpc called from server");
    }
}
