using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityStandardAssets.Cameras;
using System.Collections.Generic;
using System.Linq;

public class PlayerNetworkCommands : NetworkBehaviour {

    // Server only - amount of time to run server behind real time (to buffer client input)
    float serverLatency = 0.1f;
    Queue<Vehicle.State> _queuedStates = new Queue<Vehicle.State>();

    float localAdjust = 0;
    private Vehicle _playerVehicle;
    private Vehicle PlayerVehicle
    {
        get
        {
            if (_playerVehicle == null)
                _playerVehicle = GetComponentInChildren<Vehicle>();
            return _playerVehicle;
        }
    }
    
    // Runs on every client for every vehicle
    void Start()
    {
    }

    void Update()
    {
        UpdateVehicle();
    }

    // Only runs when this vehicle is our local player
    public override void OnStartLocalPlayer()
    {
        var camera = GameObject.FindObjectOfType<Camera_StraightLook>();
        if (camera != null)
        {
            camera.Target = PlayerVehicle.gameObject.transform;
            camera.turret = PlayerVehicle.GetComponentInChildren<TurretController_Straight>();
            camera.LockCursor();
        }
            
        StartCoroutine(SyncTime());

        base.OnStartLocalPlayer();
    }


    #region Vehicle Physics
    [SyncVar(hook = "UpdateStateFromServer")]
    Vehicle.State _vehicleState;

    private Queue<Vehicle.State> _predictedStates = new Queue<Vehicle.State>();
    private int _nextInputNumber;

    private void UpdateVehicle()
    {
        var state = PlayerVehicle.GetCurrentState();
        state.ServerTime = Time.unscaledTime;

        if (isServer)
        {
            SetStateForClients(state);
        }
        if (isLocalPlayer)
        {
            state.ServerTime += localAdjust;

            var input = PlayerVehicle.GetPlayerInputs();
            input.InputNumber = _nextInputNumber++;

            // Locally apply inputs for client prediction
            state.Inputs = input;
            PlayerVehicle.SetState(state, false);
            _predictedStates.Enqueue(state);

            ReconcileErrors();

            // Send inputs to server
            CmdSetVehicleState(state);
        }

    }

    private void ReconcileErrors()
    {
        // Adjust for error
        Vector3 posDiff = Vector3.zero;
        foreach (var d in _posDiffs)
            posDiff = (posDiff + d) / 2;

        _playerVehicle.rb.position -= posDiff / 100;

        Vector3 velDiff = Vector3.zero;
        foreach (var d in _velDiffs)
            velDiff = (velDiff + d) / 2;

        _playerVehicle.rb.velocity -= velDiff / 100;

        Vector3 angVelDiff = Vector3.zero;
        foreach (var d in _angVelDiffs)
            angVelDiff = (angVelDiff + d) / 2;

        _playerVehicle.rb.angularVelocity -= angVelDiff / 100;

        Quaternion rotDiff = Quaternion.identity;
        foreach (var d in _rotDiffs)
            rotDiff = Quaternion.Slerp(rotDiff, d, 0.5f);
        rotDiff = Quaternion.Inverse(rotDiff);

        var angleDelta = Quaternion.Angle(Quaternion.identity, rotDiff);
        Debug.Log(angleDelta);
        if (angleDelta > 5)
        {
            _playerVehicle.rb.rotation = _playerVehicle.rb.rotation * rotDiff;
            _rotDiffs.Clear();
        }
        else
            _playerVehicle.rb.rotation = Quaternion.Slerp(_playerVehicle.rb.rotation, _playerVehicle.rb.rotation * rotDiff, 0.01f);
    }

    [Command(channel = 1)]
    private void CmdSetVehicleState(Vehicle.State state)
    {
        state.ServerTime += serverLatency;
        _queuedStates.Enqueue(state);
    }


    [Server]
    private void SetStateForClients(Vehicle.State state)
    {
        // Set the _state which will go to each client
        _vehicleState = state;
    }


    // TERRIBLE PREDICTION CONCILIATION ATTEMPT
    Queue<Vector3> _posDiffs = new Queue<Vector3>();
    Queue<Quaternion> _rotDiffs = new Queue<Quaternion>();
    Queue<Vector3> _velDiffs = new Queue<Vector3>();
    Queue<Vector3> _angVelDiffs = new Queue<Vector3>();
    private void UpdateStateFromServer(Vehicle.State newState)
    {
        if (isLocalPlayer && !isServer)
        {
            // Throw out stale states
            while (_predictedStates.Any() && _predictedStates.Peek().Inputs.InputNumber < newState.Inputs.InputNumber)
                _predictedStates.Dequeue();

            // Calculate predicted current state based on authoritative server state and predicted state
            if (_predictedStates.Any())
            {
                //Debug.Log("## " + newState.Position + " - " + _predictedStates.Peek().Position);
                //Debug.Log("## " + newState.ServerTime + " - " + _predictedStates.Peek().ServerTime + "    _   " + (_predictedStates.Peek().ServerTime - newState.ServerTime));
                
                var lastPred = _predictedStates.Peek();
                
                var diff = (lastPred.ServerTime - newState.ServerTime);

                if (diff > 0.1f)
                {
                    Debug.Log("BAIL!");
                    return;
                }

                Debug.Log(Quaternion.identity.RotationBetween(Quaternion.AngleAxis(0, Vector3.up)) * Vector3.forward);

                // Calculate differences from what we predicted
                var diffPos = lastPred.Position - newState.Position;
                var diffRot = newState.Rotation.RotationBetween(lastPred.Rotation);
                var diffVel = lastPred.Velocity - newState.Velocity;
                var diffAngVel = lastPred.AngularVelocity - newState.AngularVelocity;

                _posDiffs.Enqueue(diffPos);
                _rotDiffs.Enqueue(diffRot);
                _velDiffs.Enqueue(diffVel);
                _angVelDiffs.Enqueue(diffAngVel);
                while (_posDiffs.Count > 10) _posDiffs.Dequeue();
                while (_rotDiffs.Count > 10) _rotDiffs.Dequeue();
                while (_velDiffs.Count > 10) _velDiffs.Dequeue();
                while (_angVelDiffs.Count > 10) _angVelDiffs.Dequeue();
            }
        }
        else
        {
            _playerVehicle.rb.position = newState.Position;
            _playerVehicle.rb.rotation = newState.Rotation;
            _playerVehicle.rb.velocity = newState.Velocity;
            _playerVehicle.rb.angularVelocity = newState.AngularVelocity;
        }
    }

    #endregion

    #region Time Sync
    IEnumerator SyncTime()
    {
        if (isServer) yield break;

        for (int i = 0; i < 5; i++)
        {
            Debug.Log("Bugging server");
            CmdTellServerMyGuessedTime(Time.unscaledTime + localAdjust);

            yield return new WaitForSeconds(2);
        }
    }

    [Command]
    void CmdTellServerMyGuessedTime(float unscaledTime)
    {
        Debug.Log("Server received bugging");
        var serverTime = Time.unscaledTime;
        var diff = serverTime - unscaledTime;

        RpcTellClientTheTime(serverTime, diff);
    }

    [ClientRpc]
    void RpcTellClientTheTime(float unscaledTime, float diff)
    {
        Debug.Log(unscaledTime + " - " + diff);
        localAdjust += diff;
    }
    #endregion


}
