using UnityEngine;
using System.Collections;

public class SyncTransform : MonoBehaviour
{
    public Transform ObjectToCopyPosRot;
        
    void Start()
    {

    }
    
    void FixedUpdate()
    {
        transform.position = ObjectToCopyPosRot.position;
        transform.rotation = ObjectToCopyPosRot.rotation;
    }
}
