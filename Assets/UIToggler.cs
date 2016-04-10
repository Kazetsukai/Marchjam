using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class UIToggler : MonoBehaviour {

    NetworkManagerHUD _hud;

	// Use this for initialization
	void Start () {
        _hud = GetComponent<NetworkManagerHUD>();
    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.F2))
        {

            _hud.showGUI = !_hud.showGUI;
        }
	}
}
