using UnityEngine;
using System.Collections;

public class StaticCamera : MonoBehaviour
{

    bool usingStaticCamera = false;

    Camera previousMainCam;

    void Start()
    {
        previousMainCam = Camera.main;
        GetComponent<Camera>().enabled = false;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            usingStaticCamera = !usingStaticCamera;

            if (usingStaticCamera)
            {
                GetComponent<Camera>().enabled = true;
                previousMainCam.enabled = false;
                this.tag = "MainCamera";
            }
            else
            {
                GetComponent<Camera>().enabled = false;
                previousMainCam.enabled = true;
                this.tag = "Untagged";
            }
        }
    }
}
