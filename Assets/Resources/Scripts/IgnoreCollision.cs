using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IgnoreCollision : MonoBehaviour
{
    public List<Collider> IgnoreCollisionList;
    Collider collider;

    void Start()
    {
        collider = GetComponent<Collider>();               
    }
    
    void Update()
    {
        foreach (Collider col in IgnoreCollisionList)
        {
            Physics.IgnoreCollision(collider, col, true);
        }
    }
}
