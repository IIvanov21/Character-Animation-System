using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    float horizontalInput, verticalInput;
    Vector3 movement;
    public float playerSpeed;

    NetworkVariable<int> playerScore = new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //playerScore.Value= 25;
    }

    public override void OnNetworkSpawn()
    {
        playerScore.Value = 0;
        playerScore.OnValueChanged += (oldValue, newValue) => { Debug.Log($"{OwnerClientId}Score channged: {oldValue}->{newValue}"); };

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

        if(IsOwner && Input.GetKeyDown(KeyCode.Space))
        {
            playerScore.Value = Random.Range(0, 100);
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

    [Rpc(SendTo.Server)]
    void TestToServerRpc()
    {
        Debug.Log($"[Rpc->Server] From client: {OwnerClientId}");
    }

    [Rpc(SendTo.Everyone)]
    void TestToEveryoneRpc()
    {
        Debug.Log("[Rpc->Cleint] Called from server.");

    }
}
