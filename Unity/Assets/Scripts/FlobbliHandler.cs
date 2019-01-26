using System.Collections;
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

	public float DistanceToPlanet
	{
		get { return Vector3.Distance(flobbli.position, planet.planet.position) - flobbli.localScale.x / 2f; }
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

	private void FixedUpdate()
	{
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

		if (currentVerticalSpeed < -maxDownSpeed)
			currentVerticalSpeed = -maxDownSpeed;

		if (currentVerticalSpeed < 0f && planet.PlanetSize >= DistanceToPlanet)
		{
			if (Mathf.Abs(currentVerticalSpeed) < maxDownSpeed * bounceLimit)
				currentVerticalSpeed = 0f;
			else
				currentVerticalSpeed *= -bounciness;
		}

	}

}
