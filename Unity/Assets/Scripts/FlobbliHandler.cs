﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlobbliHandler : MonoBehaviour
{
	public PlanetHandler planet;
	public Transform node;
	public Transform flobbli;
	[Space]
	public float maxDownSpeed;
	public float maxUpSpeed;
	float currentVerticalSpeed;
	[Space]
	public float downForce;
	public float upForce;
	[Space]
	public float bounciness;
	public float bounceLimit;
	[Space]
	public float baseSpeed; // °in seconds
	public float direction; // 1 = counter, -1 clockwise
	public float speedMod;
	[Space]
	public float simplicity;
	public float varyThinking;
	public bool isFree;
	[Space]
	public FlobbliAction[] actions;
	Coroutine decisionMaker;
	[Space]
	public float rotateSpeed;
	public bool doRotate;
	[Space]
	public float floatingSpeed;
	public float maxFloat;
	public Vector3 floatDirection;
	public bool doFloat;
	public float floatRandomizer;

	public float DistanceToPlanet
	{
		get { return Vector3.Distance(flobbli.position, planet.planet.position) - flobbli.localScale.x / 2f; }
	}

	public float DistanceToSurface
	{
		get { return DistanceToPlanet - planet.PlanetSize; }
	}

	private void Start()
	{
		
		decisionMaker = StartCoroutine(makeDecisions());
		flobbli.localPosition = new Vector3(flobbli.localPosition.x, planet.PlanetSize + flobbli.localScale.x / 2f, flobbli.localPosition.z);
		if (UnityEngine.Random.Range(-1f, 1f) < 0f)
			direction = -1f;
		else
			direction = 1f;
		node.rotation = Quaternion.Euler(node.rotation.eulerAngles.x, node.rotation.eulerAngles.y, UnityEngine.Random.Range(0f, 360f));
		
	}

	public void GetLost(float speed, float height)
	{
		doRotate = false;
		doFloat = false;
		StartCoroutine(RotateToNormal());
		StartCoroutine(FlyAway(speed, height));
	}

	public float flobbliRotationSpeed = 2f;
	IEnumerator RotateToNormal()
	{

		while (Mathf.Abs(flobbli.localRotation.eulerAngles.z) > flobbliRotationSpeed)
		{
			if (flobbli.localRotation.eulerAngles.z > 0f)
				flobbli.localRotation = Quaternion.Euler(0, 0, flobbli.localRotation.eulerAngles.z - flobbliRotationSpeed);
			else
				flobbli.localRotation = Quaternion.Euler(0, 0, flobbli.localRotation.eulerAngles.z + flobbliRotationSpeed);
			yield return null;
		}
		flobbli.localRotation = Quaternion.Euler(0, 0, 0);
	}

	IEnumerator FlyAway(float speed, float height)
	{
		if (UnityEngine.Random.Range(-1f, 1f) < 0f)
			direction = -1f;
		else
			direction = 1f;

		currentVerticalSpeed = height;
		speedMod = speed / baseSpeed;


		bool isDone = false;

		while (!isDone)
		{
			if (DistanceToSurface < -1f)
				flobbli.localPosition = new Vector3(flobbli.localPosition.x, planet.PlanetSize + flobbli.localScale.x / 2f, flobbli.localPosition.z);
				CalculateVerticalMovement();
				flobbli.localPosition = new Vector3(flobbli.localPosition.x, flobbli.localPosition.y + currentVerticalSpeed, flobbli.localPosition.z);
				node.rotation = Quaternion.Euler(node.rotation.eulerAngles.x, node.rotation.eulerAngles.y, node.rotation.eulerAngles.z + ((baseSpeed * direction * speedMod) / 60f));
				node.transform.position = planet.planet.position;

			if (speedMod <= 1.05f && currentVerticalSpeed == 0f)
			{
				isFree = true;
				isDone = true;
			}

			speedMod *= 0.99f;
			//Debug.Log(speedMod);

			yield return null;
		}
		decisionMaker = StartCoroutine(makeDecisions());

	}

	private void Update()
	{
		if (doRotate)
		{
			flobbli.rotation = Quaternion.Euler(flobbli.rotation.eulerAngles.x, flobbli.rotation.eulerAngles.y, flobbli.rotation.eulerAngles.z + ((baseSpeed * direction) * Time.fixedDeltaTime));
		}
		if (doFloat)
		{
			if (Vector3.Distance(transform.parent.position, transform.position) >= maxFloat)
			{
				floatDirection = new Vector3(-floatDirection.x + UnityEngine.Random.Range(-floatRandomizer, floatRandomizer), -floatDirection.y + UnityEngine.Random.Range(-floatRandomizer, floatRandomizer));
				floatDirection = floatDirection.normalized;
			}
			node.transform.Translate(floatDirection * floatingSpeed);
		}
	}

	private void FixedUpdate()
	{
		if (DistanceToSurface < -5f)
			flobbli.localPosition = new Vector3(flobbli.localPosition.x, planet.PlanetSize + flobbli.localScale.x / 2f, flobbli.localPosition.z);
		if (isFree)
		{
			CalculateVerticalMovement();
			flobbli.localPosition = new Vector3(flobbli.localPosition.x, flobbli.localPosition.y + currentVerticalSpeed, flobbli.localPosition.z);
			node.rotation = Quaternion.Euler(node.rotation.eulerAngles.x, node.rotation.eulerAngles.y, node.rotation.eulerAngles.z + ((baseSpeed * direction * speedMod) / 60f));
			node.transform.position = planet.planet.position;
		}
}

	IEnumerator makeDecisions()
	{
		float random;
		while (isFree)
		{
			random = UnityEngine.Random.Range(0f, 1f);
			for (int i = 0; i < actions.Length && random > 0f; i++)
			{
				random -= actions[i].probablity;
				if (random <= 0f)
					DoAction(actions[i]);
			}
			yield return new WaitForSeconds(simplicity+UnityEngine.Random.Range(-varyThinking, varyThinking));
		}
	}

	void DoAction(FlobbliAction action)
	{
		switch (action.action)
		{
			case "wait":
				speedMod = 0f;
				break;
			case "move":
				speedMod = 1f;
				break;
			case "direction":
				direction *= -1f;
				break;
			case "slow":
				speedMod = 0.5f;
				break;
			case "run":
				speedMod = 1.5f;
				break;
			case "fireUnderTheAss":
				speedMod = 10f;
				break;
		}
	}

	void CalculateVerticalMovement()
	{
		currentVerticalSpeed -= downForce;

		//if (currentVerticalSpeed < -maxDownSpeed)
			//currentVerticalSpeed = -maxDownSpeed;

		if (currentVerticalSpeed < 0f && DistanceToSurface <= 0f)
		{
			if (Mathf.Abs(currentVerticalSpeed) < bounceLimit)
				currentVerticalSpeed = 0f;
			else
				currentVerticalSpeed *= -bounciness;
		}

	}

}
