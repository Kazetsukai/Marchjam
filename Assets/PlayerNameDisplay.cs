using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PlayerNameDisplay : MonoBehaviour {
    private RectTransform _textRect;

    public Transform Target;
    private Text _text;

    // Use this for initialization
    void Start () {
        _textRect = GetComponentInChildren<RectTransform>();
        _text = GetComponent<Text>();

        _text.text = Environment.MachineName;
    }
	
	// Update is called once per frame
	void Update () {
        var screenPoint = Camera.main.WorldToScreenPoint(Target.position + Vector3.up * 1.5f);
        _textRect.anchoredPosition = screenPoint;
        Debug.Log(screenPoint);
	}
}
