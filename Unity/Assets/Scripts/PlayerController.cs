using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{

	public Transform playerNode;
	public Transform player;
	public PlanetHandler planet;
	[Space]
	public float maxDownSpeed;
	public float maxUpSpeed;
	[Space]
	public float downForce;
	public float upForce;
	[Space]
	public float bounciness;
	public float bounceLimit;
	bool inputIsBlocked;
	[Space]
	public float jumpTimeout;
	float jumpTimer;
	[Space]
	public float boostForce;
	public float boostCooldown;
	float boostTime;
	[Space]
	public float maxSpeedMod;
	public float minBonusDistance;
	[Space]
	public float doubleTapTime;
	float tapTime;
	[Space]
	bool boostActive;
	[Space]
	public float baseSpeed; // °in seconds
	public float direction; // 1 = counter, -1 clockwise
	[Space]
	public Transform catchStarter;
	public Transform catchEnd;
	public float catchTime;
	public int maxCatching;
	[Space]
	public FlobbliHandler touchedFlobbli;
	public List<FlobbliHandler> flobbliCatched;
	float flobbliCatchProgress;

	float currentVerticalSpeed;
	float MaxDistance = 200f;



	public void CatchedSomething(FlobbliHandler flobbli)
	{
		if (touchedFlobbli == null)
		{
			touchedFlobbli = flobbli;
			flobbliCatchProgress = 0f;
			flobbli.isFree = false;
		}
	}

	private void Update()
	{
		if (touchedFlobbli != null)
		{
			flobbliCatchProgress += Time.deltaTime / catchTime;
			touchedFlobbli.transform.position = Vector3.Lerp(catchStarter.position, catchEnd.position, flobbliCatchProgress);
		}

        var vec = planet.transform.position - player.GetChild(0).transform.position;
        Debug.DrawRay(player.GetChild(0).transform.position, vec, Color.red);
	}

	public float MinMaxDistance
	{
		get { return Mathf.InverseLerp(planet.planetSizeFactor + minBonusDistance, MaxDistance, player.localPosition.y - planet.planetSizeFactor); }
	}

	public float BoostForce
	{
		get { return boostForce + boostForce * maxSpeedMod * MinMaxDistance; }
	}

	public float MaxDownSpeed
	{
		get { return maxDownSpeed + maxDownSpeed * maxSpeedMod * MinMaxDistance; }
	}

	public float MaxUpSpeed
	{
		get { return maxUpSpeed + maxUpSpeed * maxSpeedMod * MinMaxDistance; }
	}

	public float DownForce
	{
		get { return downForce + downForce * maxSpeedMod * MinMaxDistance; }
	}

	public float UpForce
	{
		get { return upForce + upForce * maxSpeedMod * MinMaxDistance; }
	}

	public float DistanceToPlanet
	{
		get { return Vector3.Distance(player.position, planet.planet.position) - player.localScale.x/2f; }
	}

	private void FixedUpdate()
	{
		PlanetHandler closestPlanet = GetClosestPlanet();
		Vector3 tempPos = player.position;

		if ((planet == null || closestPlanet != planet) && jumpTimer <= 0f)
		{
            //Debug.Log(String.Format("Old Position: {0} {1} {2}", playerNode.GetChild(0).transform.position.x, playerNode.transform.position.y, playerNode.transform.position.z));
			jumpTimer = jumpTimeout;
            direction *= -1f;

            var savePos = playerNode.GetChild(0).transform.position;
            //Debug.Log("Save Pos: " + savePos);

            playerNode.transform.position = closestPlanet.planet.position;
            playerNode.GetChild(0).transform.position = savePos;

            var newDownVec = closestPlanet.transform.position - player.GetChild(0).transform.position;

            var angle = Vector3.Angle(newDownVec, playerNode.transform.localEulerAngles);

            playerNode.GetChild(0).transform.Rotate(Vector3.forward, angle);

            currentVerticalSpeed *= -1f;
			inputIsBlocked = true;
			planet = closestPlanet;
		}
		else
		{
			playerNode.transform.position = planet.planet.position;
		}
		jumpTimer -= Time.fixedDeltaTime;
		CalculateVerticalMovement();
		player.localPosition = new Vector3(player.localPosition.x, player.localPosition.y + currentVerticalSpeed, player.localPosition.z);
		if (player.localPosition.y > MaxDistance)
			player.localPosition = new Vector3(player.localPosition.x, MaxDistance, player.localPosition.z);
		playerNode.rotation = Quaternion.Euler(playerNode.rotation.eulerAngles.x, playerNode.rotation.eulerAngles.y, playerNode.rotation.eulerAngles.z + ((baseSpeed * direction) * Time.fixedDeltaTime));
	}

	float GetAngle(Vector3 planetA, Vector3 planetB, Vector3 player)
	{
		float a = Mathf.Pow(Vector3.Distance(planetA, planetB), 2f);
		float b = Mathf.Pow(Vector3.Distance(planetA, player), 2f);
		float c = Mathf.Pow(Vector3.Distance(planetB, player), 2f);

		Debug.Log(a);
		Debug.Log(b);
		Debug.Log(c);

		Debug.Log(Mathf.Acos((-0.5f * a + 0.5f * b + 0.5f * c) / (b * c)) * Mathf.Rad2Deg);

		return Mathf.Acos ((-0.5f * a + 0.5f * b + 0.5f * c) / (b * c));

	}


	PlanetHandler GetClosestPlanet()
	{
		PlanetHandler closest = planet;
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("PlanetNode"))
		{
			var tempPlanet = go.GetComponent<PlanetHandler>();
			if ( Vector3.Distance(player.GetChild(0).position, tempPlanet.planet.position) < Vector3.Distance(player.GetChild(0).position, closest.planet.position))
				closest = tempPlanet;
		}
		return closest;
	}

    void CalculateVerticalMovement()
	{
		if (IsUpDown())
		{
			if (Time.time < tapTime + doubleTapTime && Time.time > boostTime + boostCooldown)
			{
				boostTime = Time.time;
				currentVerticalSpeed = boostForce;
			}
			tapTime = Time.time;
			inputIsBlocked = false;

		} else if (IsUpPress() && currentVerticalSpeed < MaxUpSpeed && !inputIsBlocked)
		{
			currentVerticalSpeed += UpForce;
		}

		currentVerticalSpeed -= DownForce;

		if (currentVerticalSpeed < -MaxDownSpeed)
			currentVerticalSpeed = -MaxDownSpeed;

		if (currentVerticalSpeed < 0f && planet.PlanetSize >= DistanceToPlanet)
		{
			if (Mathf.Abs(currentVerticalSpeed) < MaxDownSpeed * bounceLimit)
				currentVerticalSpeed = 0f;
			else
				currentVerticalSpeed *= -bounciness;
		}

	}

	bool IsUpDown()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			return true;

		if (Input.GetMouseButtonDown(0))
			return true;

		return false;
	}

	bool IsUpPress()
	{
		if (Input.GetKey(KeyCode.Space))
			return true;

		if (Input.GetMouseButton(0))
			return true;

		return false;
	}

}
