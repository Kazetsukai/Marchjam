using UnityEngine;
using System.Collections;
using Assets;
using System.Linq;
using UnityEngine.Networking;

public class NetworkClientController : MonoBehaviour {
    private int _connectionId;
    private int _hostId;
    private byte _reliableChannel;
    private byte _stateChannel;

    public string Address { get; set; }

	// Use this for initialization
	void Start () {
        var netObjects = FindObjectsOfType<GameObject>().Where(o => o.GetComponent<INetworkEntity>() != null).ToList();

        Debug.Log("Destroying " + netObjects.Count() + " net objects because I am a client");
        foreach (var obj in netObjects)
            Destroy(obj.gameObject);

        Debug.Log("Server address: " + Address);

        var config = new ConnectionConfig();
        _reliableChannel = config.AddChannel(QosType.Reliable);
        _stateChannel = config.AddChannel(QosType.StateUpdate);

        var topology = new HostTopology(config, 1);

        _hostId = NetworkTransport.AddHost(topology);

        byte error;
        _connectionId = NetworkTransport.Connect(_hostId, Address, Marchjam.Networking.NetworkDiscovery.CONNECTION_PORT, 0, out error);
	}
	
	// Update is called once per frame
	void Update () {

        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;
        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        switch (recData)
        {
            case NetworkEventType.Nothing:         //1
                break;
            case NetworkEventType.ConnectEvent:    //2
                break;
            case NetworkEventType.DataEvent:       //3
                Debug.Log("Data event received!");
                break;
            case NetworkEventType.DisconnectEvent: //4
                break;
        }

    }
}
