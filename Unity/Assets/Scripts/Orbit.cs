using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public float multiplier = 50.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        gameObject.transform.Rotate(Vector3.forward, Time.deltaTime * multiplier);
    }
}
