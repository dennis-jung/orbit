using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPlanet : MonoBehaviour
{
    [SerializeField]
    GameObject[] plants;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var plant in plants)
        {
            for (int i = 0; i < Random.Range(0,4); i++)
            {
                var vec = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), 0.0f);
                vec.Normalize();
                var newPos = vec * Random.Range(9.0f, 10.0f);
                GameObject.Instantiate(plant, newPos + transform.position, Quaternion.FromToRotation(Vector3.up, newPos));
            }
            
        }
    }

}
