using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using System.Text;

namespace Marchjam.Networking
{
    public class NetworkDiscovery : MonoBehaviour
    {
        private byte _reliableChannelId;
        private byte _stateUpdateChannelId;

        private bool _isServer = false;
        private bool _isClient = false;
        private int _hostId = -1;

        byte[] _msgOutBuffer = null;
        byte[] _msgInBuffer = null;
        private readonly int kMaxBroadcastMsgSize = 1024;
        private int _broadcastPort = 41326;
        private int _broadcastKey = 1337;
        private int _broadcastVersion = 1;
        private int _broadcastSubVersion = 0;
        private bool _running;
        private string _broadcastData = "HELLO";
        private NetworkClientController _client;
        private NetworkServerController _server;

        public bool Connected { get; private set; }


        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }


        // Use this for initialization
        void Start()
        {


            if (NetworkTransport.IsStarted)
                NetworkTransport.Shutdown();

            StartCoroutine(InitializeNetwork());

        }

        private bool Initialize()
        {
            if (_broadcastData.Length >= kMaxBroadcastMsgSize)
            {
                Debug.LogError("NetworkDiscovery Initialize - data too large. max is " + kMaxBroadcastMsgSize);
                return false;
            }

            if (!NetworkTransport.IsStarted)
            {
                NetworkTransport.Init();
            }


            if (NetworkManager.singleton != null)
            {
                _broadcastData = "NetworkManager:" + NetworkManager.singleton.networkAddress + ":" + NetworkManager.singleton.networkPort;
            }

            DontDestroyOnLoad(gameObject);
            _msgOutBuffer = GetBytes(_broadcastData);
            _msgInBuffer = new byte[kMaxBroadcastMsgSize];
            return true;
        }

        private IEnumerator InitializeNetwork()
        {
            Initialize();

            StartAsClient();

            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(1);

                Debug.Log("Checking messages...");
                ListenBroadcast((add, msg) => Debug.Log("Address: " + add + ", Message: " + msg));
            }

            StopBroadcast();


            Debug.Log("I am become server!");
            StartAsServer();
        }

        // Update is called once per frame
        void Update()
        {

        }


        private void ListenBroadcast(Action<string, string> addressAndMessageResponse)
        {
            if (_hostId == -1)
                return;

            if (_isServer)
                return;

            int connectionId;
            int channelId;
            int receivedSize;
            byte error;
            NetworkEventType networkEvent = NetworkEventType.DataEvent;

            do
            {
                networkEvent = NetworkTransport.ReceiveFromHost(_hostId, out connectionId, out channelId, _msgInBuffer, kMaxBroadcastMsgSize, out receivedSize, out error);

                if (networkEvent == NetworkEventType.BroadcastEvent)
                {
                    NetworkTransport.GetBroadcastConnectionMessage(_hostId, _msgInBuffer, kMaxBroadcastMsgSize, out receivedSize, out error);

                    string senderAddr;
                    int senderPort;
                    NetworkTransport.GetBroadcastConnectionInfo(_hostId, out senderAddr, out senderPort, out error);

                    addressAndMessageResponse(senderAddr, GetString(_msgInBuffer));
                }
            } while (networkEvent != NetworkEventType.Nothing);

        }


        // listen for broadcasts
        private bool StartAsClient()
        {
            if (_hostId != -1 || _running)
            {
                Debug.LogWarning("NetworkDiscovery StartAsClient already started");
                return false;
            }

            ConnectionConfig cc = new ConnectionConfig();
            cc.AddChannel(QosType.Unreliable);
            HostTopology defaultTopology = new HostTopology(cc, 1);

            _hostId = NetworkTransport.AddHost(defaultTopology, _broadcastPort);
            if (_hostId == -1)
            {
                Debug.LogError("NetworkDiscovery StartAsClient - addHost failed");
                return false;
            }

            byte error;
            NetworkTransport.SetBroadcastCredentials(_hostId, _broadcastKey, _broadcastVersion, _broadcastSubVersion, out error);

            _running = true;
            _isClient = true;
            Debug.Log("StartAsClient Discovery listening");
            return true;
        }

        // perform actual broadcasts
        private bool StartAsServer()
        {
            if (_hostId != -1 || _running)
            {
                Debug.LogWarning("NetworkDiscovery StartAsServer already started");
                return false;
            }

            ConnectionConfig cc = new ConnectionConfig();
            cc.AddChannel(QosType.Unreliable);
            HostTopology defaultTopology = new HostTopology(cc, 1);

            _hostId = NetworkTransport.AddHost(defaultTopology, 0);
            if (_hostId == -1)
            {
                Debug.LogError("NetworkDiscovery StartAsServer - addHost failed");
                return false;
            }

            byte err;
            if (!NetworkTransport.StartBroadcastDiscovery(_hostId, _broadcastPort, _broadcastKey, _broadcastVersion, _broadcastSubVersion, _msgOutBuffer, _msgOutBuffer.Length, 1000, out err))
            {
                Debug.LogError("NetworkDiscovery StartBroadcast failed err: " + err);
                return false;
            }

            _running = true;
            _isServer = true;
            Debug.Log("StartAsServer Discovery broadcasting");
            return true;
        }

        private void StopBroadcast()
        {
            if (_hostId == -1)
            {
                Debug.LogError("NetworkDiscovery StopBroadcast not initialized");
                return;
            }

            if (!_running)
            {
                Debug.LogWarning("NetworkDiscovery StopBroadcast not started");
                return;
            }
            if (_isServer)
            {
                NetworkTransport.StopBroadcastDiscovery();
            }

            NetworkTransport.RemoveHost(_hostId);
            _hostId = -1;
            _running = false;
            _isServer = false;
            _isClient = false;
            Debug.Log("Stopped Discovery broadcasting");
        }
    }
}