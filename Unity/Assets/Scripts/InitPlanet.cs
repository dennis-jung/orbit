using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPlanet : MonoBehaviour
{
    [SerializeField]
    GameObject[] outerPlants;

    [SerializeField]
    GameObject[] innerPlants;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var plant in outerPlants)
        {
            for (int i = 0; i < Random.Range(0,4); i++)
            {
                var vec = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-1.0f, 1.0f));
                vec.Normalize();
                var newPos = vec * 10.0f; //Random.Range(9.8f, 10.0f);
                GameObject.Instantiate(plant, newPos + transform.position, Quaternion.FromToRotation(Vector3.up, newPos));
            }
            
        }

        foreach (var plant in innerPlants)
        {
            for (int i = 0; i < Random.Range(0, 4); i++)
            {
                var vec = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), 0.0f);
                vec.Normalize();
                var newPos = vec * Random.Range(1.0f, 7.0f);
                GameObject.Instantiate(plant, newPos + transform.position, Quaternion.FromToRotation(Vector3.up, newPos));
            }

        }
    }

}
