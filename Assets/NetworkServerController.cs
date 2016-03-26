using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkServerController : MonoBehaviour {
    private int _hostId;
    private byte _reliableChannel;
    private byte _stateChannel;

    // Use this for initialization
    void Start () {
        var config = new ConnectionConfig();
        _reliableChannel = config.AddChannel(QosType.Reliable);
        _stateChannel = config.AddChannel(QosType.StateUpdate);

        var topology = new HostTopology(config, 1);

        _hostId = NetworkTransport.AddHost(topology);
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
