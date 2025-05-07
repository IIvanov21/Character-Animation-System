using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkButtons : MonoBehaviour
{
    public Button hostBtn, serverBtn, clientBtn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hostBtn.onClick.AddListener
            (() => { NetworkManager.Singleton.StartHost(); });
        serverBtn.onClick.AddListener
            (() => { NetworkManager.Singleton.StartServer(); });
        clientBtn.onClick.AddListener
            (() => { NetworkManager.Singleton.StartClient(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
