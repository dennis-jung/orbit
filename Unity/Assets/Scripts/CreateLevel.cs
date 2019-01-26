using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLevel : MonoBehaviour
{

    [SerializeField]
    private Transform planet;

    [SerializeField]
    private int countLevel1 = 5;

    [SerializeField]
    private int countLevel2 = 10;

    [SerializeField]
    private int countLevel3 = 20;

    [SerializeField]
    private float minDistance = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < countLevel1; i++)
        {
            var vec = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), 0f);
            vec.Normalize();
            var newPos = vec * 200.0f;
            if (CheckDistances(newPos))
            {
                Instantiate(planet, newPos, Quaternion.identity);
            }
        }

        for (int i = 0; i < countLevel2; i++)
        {
            var vec = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), 0f);
            vec.Normalize();
            var newPos = vec * 400.0f;
            if (CheckDistances(newPos))
            {
                Instantiate(planet, newPos, Quaternion.identity);
            }
        }

        for (int i = 0; i < countLevel3; i++)
        {
            var vec = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), 0f);
            vec.Normalize();
            var newPos = vec * 600.0f;
            if (CheckDistances(newPos))
            {
                Instantiate(planet, newPos, Quaternion.identity);
            }
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
