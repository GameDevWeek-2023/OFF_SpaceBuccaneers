using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkDebugManager : MonoBehaviour
{
    NetworkManager _networkManager;
    private bool _host;
    private UnityTransport _utp;
    private void Awake()
    {
        _networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        _utp = _networkManager.GetComponent<UnityTransport>();
        var networkDataCarrier = FindObjectOfType<NetworkDataCarrier>();
        if (networkDataCarrier == null)
        {
            Debug.LogError("No NetworkDataCarrier found");
            return;
        }
        
        
        if (networkDataCarrier.Host)
        {
            _host = true;
            GameManager.Instance.Host = true;
            _utp.ConnectionData.ServerListenAddress = networkDataCarrier.IP;
            _utp.ConnectionData.Port = (ushort)networkDataCarrier.Port;
            _utp.ConnectionData.Address = networkDataCarrier.IP;
        }
        else
        {
            _host = false;
            GameManager.Instance.Host = false;
            _utp.ConnectionData.Address = networkDataCarrier.IP;
            _utp.ConnectionData.Port = (ushort)networkDataCarrier.Port;
        }
    }


    private void Start()
    {
        if (_host)
        {
            _networkManager.StartHost();
        }
        else
        {
            _networkManager.StartClient();
        }
    }
}
