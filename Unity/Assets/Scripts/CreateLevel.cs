using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CreateLevel : MonoBehaviour
{

    [SerializeField]
    private Transform[] planets;

    [SerializeField]
    private Transform[] asteroids;

    [SerializeField]
    private int countLevel1 = 5;

    [SerializeField]
    private int countLevel2 = 10;

    [SerializeField]
    private int countLevel3 = 20;

    [SerializeField]
    private float minDistance = 100.0f;

    private int count = 0;

    private Transform GetNextPlanet()
    {
        var nextCount = ++count % planets.Length;
        return planets[nextCount];
    }

    private Transform GetNextAsteroid()
    {
        var nextCount = ++count % asteroids.Length;
        return asteroids[nextCount];
    }
    // Start is called before the first frame update
    void Start()
    {
        CreatePlanets(countLevel1, 100.0f);
        CreatePlanets(countLevel2, 200.0f);
        CreatePlanets(countLevel3, 300.0f);

        CreateAsteroids(10, 50.0f);
    }

    private void CreatePlanets(int count, float distanceFromCenter)
    {
        for (int i = 0; i < count; i++)
        {
            var vec = new Vector3(UnityEngine.Random.Range(-10.0f, 10.0f), UnityEngine.Random.Range(-10.0f, 10.0f), 0f);
            vec.Normalize();
            var newPos = vec * distanceFromCenter;
            if (CheckDistances(newPos))
            {
                Instantiate(GetNextPlanet(), newPos, Quaternion.identity);
            }
        }
    }

    private void CreateAsteroids(int count, float distanceFromCenter)
    {
        for (int i = 0; i < count; i++)
        {
            var vec = new Vector3(UnityEngine.Random.Range(-10.0f, 10.0f), UnityEngine.Random.Range(-10.0f, 10.0f), 0f);
            vec.Normalize();
            var newPos = vec * (distanceFromCenter + UnityEngine.Random.Range(-10.0f, 10.0f));
            Instantiate(GetNextAsteroid(), newPos, Quaternion.identity);
        }
    }

    private bool CheckDistances (Vector3 newPos)
    {
        var planets = GameObject.FindGameObjectsWithTag("PlanetNode");

        foreach (var planet in planets)
        {
            if (Vector3.Distance(newPos, planet.transform.position) < minDistance )
            {
                return false;
            }
        }

        return true;
    }


}
