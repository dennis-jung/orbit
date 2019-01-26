using UnityEngine;
using System.Collections;
using System;

public class Gravity : MonoBehaviour
{

    [SerializeField]
    private float initalForce = 15.0f;

    [SerializeField]
    private float gravity = 2.0f;

    [SerializeField]
    private float strength = 500.0f;

    [SerializeField]
    private float drag = 1.5f;
    
    private GameObject player;

    private GameObject nearestPlanet;

    private GameObject[] allPlanets;

    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        player = transform.Find("Player").gameObject;
        player.GetComponent<Rigidbody2D>().AddForce(Vector3.up * initalForce);

        allPlanets = GameObject.FindGameObjectsWithTag("Planet");
    }

    private void FindNearestPlanet()
    {
        float minDist = 10000.0f;
        foreach (var planet in allPlanets)
        {
            float dist = Vector3.Distance(planet.transform.position, transform.position);
            if (dist < minDist)
            {
                nearestPlanet = planet;
                minDist = dist;
            }
        }
        //Debug.Log("Nearest Plant found: "+ nearestPlanet.name);
    }

    void FixedUpdate()
    {
        FindNearestPlanet();

        float dist = Vector3.Distance(player.transform.position, transform.position);

        var v = transform.position - player.transform.position;
        //player.transform.localPosition.Set(player.transform.localPosition.x - 0.05f, player.transform.localPosition.y, 0.0f);
        player.GetComponent<Rigidbody2D>().AddForce(v.normalized * gravity);

        Debug.DrawRay(player.transform.position, player.GetComponent<Rigidbody2D>().velocity, Color.red);

        // drag
        player.GetComponent<Rigidbody2D>().AddForce(-player.GetComponent<Rigidbody2D>().velocity *  (1/drag));

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