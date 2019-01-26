using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetHandler : MonoBehaviour
{
	public Transform planetNode;
	public Transform planet;

	float BasePlanetSize = 10f;
	public float planetSizeFactor;

	public float PlanetSize
	{
		get { return BasePlanetSize* planetSizeFactor; }
	}
}
