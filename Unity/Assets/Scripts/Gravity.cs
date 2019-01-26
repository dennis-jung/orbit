using UnityEngine;
using System.Collections;
using System;

public class Gravity : MonoBehaviour
{

    //public float maxGravDist = 4.0f;
    [SerializeField]
    private float initalForce = 15.0f;

    [SerializeField]
    private float maxGravity = 2.0f;

    [SerializeField]
    private float strength = 500.0f;

    [SerializeField]
    private float drag = .5f;


    public GameObject player;

    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        player = transform.Find("Player").gameObject;
        player.GetComponent<Rigidbody2D>().AddForce(Vector3.up * initalForce);
    }

    void FixedUpdate()
    {

        float dist = Vector3.Distance(player.transform.position, transform.position);

        var v = transform.position - player.transform.position;
        //player.transform.localPosition.Set(player.transform.localPosition.x - 0.05f, player.transform.localPosition.y, 0.0f);
        player.GetComponent<Rigidbody2D>().AddForce(v.normalized * (1.0f / dist) * maxGravity);

        Debug.DrawRay(player.transform.position, player.GetComponent<Rigidbody2D>().velocity, Color.red);

        Debug.DrawRay(player.transform.position, v.normalized, Color.blue);

        Debug.DrawRay(player.transform.position, calculateTangent(v), Color.yellow);


        //Debug.DrawRay(player.transform.position, player.transform.TransformDirection(Vector3.up), Color.green);

        //var newPos = v.normalized * (1.0f / dist) * maxGravity;
        //player.transform.position.Set(newPos.x, newPos.y, 0.0f);





    }

    public void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            float dist = Vector3.Distance(player.transform.position, transform.position);
            var v = transform.position - player.transform.position;
            var force = -v.normalized * (1.0f / dist) * strength;

            var velo = player.GetComponent<Rigidbody2D>().velocity;
            player.GetComponent<Rigidbody2D>().AddForce(velo * strength);

            var tangent = calculateTangent(v);
            Debug.DrawRay(player.transform.position, tangent, Color.yellow);

            player.GetComponent<Rigidbody2D>().AddForce(force);
            player.GetComponent<Rigidbody2D>().AddForce(tangent.normalized * 1/tangent.magnitude);
            //Debug.Log("Jump");
            Debug.DrawRay(player.transform.position, force, Color.white);
        }
    }

    private Vector3 calculateTangent(Vector3 towardsCenter)
    {

        return Quaternion.Euler(0, 0, -90) * towardsCenter;
    }
}