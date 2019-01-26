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

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < countLevel1; i++)
        {
            var vec = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), 0f);
            vec.Normalize();
            Instantiate(planet, vec * 200.0f, Quaternion.identity);
        }

        for (int i = 0; i < countLevel2; i++)
        {
            var vec = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), 0f);
            vec.Normalize();
            Instantiate(planet, vec * 400.0f, Quaternion.identity);
        }

        for (int i = 0; i < countLevel3; i++)
        {
            var vec = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), 0f);
            vec.Normalize();
            Instantiate(planet, vec * 600.0f, Quaternion.identity);
        }
    }



}
