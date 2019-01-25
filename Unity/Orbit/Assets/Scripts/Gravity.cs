using UnityEngine;
using System.Collections;
using System;

public class Gravity : MonoBehaviour
{

    //public float maxGravDist = 4.0f;
    public float maxGravity = 2.0f;

    public GameObject nearestPlanet;

    void Start()
    {
        //planets = GameObject.FindGameObjectsWithTag("Planet");
    }

    void FixedUpdate()
    {

        float dist = Vector3.Distance(nearestPlanet.transform.position, transform.position);

        Vector3 v = nearestPlanet.transform.position - transform.position;
        GetComponent<Rigidbody2D>().AddForce(v.normalized * (1.0f / dist ) * maxGravity);

    }
}