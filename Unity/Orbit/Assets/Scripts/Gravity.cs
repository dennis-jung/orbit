using UnityEngine;
using System.Collections;
using System;

public class Gravity : MonoBehaviour
{

    //public float maxGravDist = 4.0f;
    public float maxGravity = 2.0f;

    public GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {

        float dist = Vector3.Distance(player.transform.position, transform.position);

        var v = transform.position - player.transform.position;
        player.GetComponent<Rigidbody2D>().AddForce(v.normalized * (1.0f / dist ) * maxGravity);
        //var newPos = v.normalized * (1.0f / dist) * maxGravity;
        //player.transform.position.Set(newPos.x, newPos.y, 0.0f);

    }
}