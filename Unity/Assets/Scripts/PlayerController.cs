﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{

	public Transform playerNode;
	public Transform player;
	public PlanetHandler planet;
	public PlanetHandler closestPlanet;
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
	public Transform stomach;
	public float catchTime;
	public float eatTime;
	public int maxCatching;
	public float stomachRadius;
	[Space]
	public FlobbliHandler touchedFlobbli;
	public FlobbliHandler eatingFlobbli;
	public List<FlobbliHandler> flobbliCatched;
	float flobbliCatchProgress;
	float flobbliCatchTimer;
	float flobbliEatProgress;

	float currentVerticalSpeed;
	public float MaxDistance = 200f;
	public bool touchedGround;

    private Animator anim;
    private Transform sprite;

    private void Start()
	{
		eatingFlobbli = null;
		touchedFlobbli = null;
		flobbliCatched = new List<FlobbliHandler>();
		playerDistance = planet.PlanetSize * 2;

        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>().gameObject.transform;
	}
	[Space]
	public float playerRotation;
	public float playerDistance;

	[Space]
	Vector3 changeForce;
	float changeForceFalloff;
	public float changeForceTime = 2f;
	public float changeForcePower = 2f;

	private void Move()
	{
		playerRotation += (baseSpeed * direction) * Time.fixedDeltaTime;
		if (playerRotation >= 360f)
			playerRotation -= 360f;
		else if (playerRotation <= -360f)
			playerRotation += 360f;

		CalculateVerticalMovement();
		playerDistance += currentVerticalSpeed;

        //animation-stuff
        anim.SetFloat("ySpeed", currentVerticalSpeed);
        sprite.localScale = new Vector3(Math.Abs(sprite.localScale.x) * direction * -1, sprite.localScale.y, sprite.localScale.z);

		Vector3 vector = Quaternion.Euler(0, 0, playerRotation) * Vector3.up;
		playerNode.position = planet.planet.transform.position + (vector * playerDistance);
		playerNode.rotation = Quaternion.Euler(playerNode.rotation.eulerAngles.x, playerNode.rotation.eulerAngles.y, playerRotation);

		if (changeForceFalloff > 0f)
		{
			playerNode.position += Falloff;
			changeForceFalloff -= Time.deltaTime / changeForceTime;

			playerDistance = Vector3.Distance(planet.planet.position, playerNode.position);
		}
		if (playerDistance > MaxDistance)
			playerDistance = MaxDistance;
	}

	public void CatchedSomething(FlobbliHandler flobbli)
	{
		if (planet.isHome || !flobbli.isFree)
			return;
		int amount = flobbliCatched.Count;
		planet.tierchen--;
		if (eatingFlobbli != null)
			amount++;
		if (touchedFlobbli == null && amount < maxCatching)
		{
			flobbli.node.position = flobbli.flobbli.position;
			touchedFlobbli = flobbli;
			flobbli.doRotate = true;
			flobbliCatchTimer = catchTime;
			flobbli.isFree = false;
		}
	}

	private void Update()
	{
		if (touchedGround)
			LooseMinions();
		touchedGround = false;
		CatchProgres();
		EatProgress();
		InStomachStuff();
		//var vec = planet.transform.position - player.transform.position;
	}

	[Header("FlobbliReleaseParameters")]
	public float throwSpeed;
	public float throwSpeedVar;
	public float throwHeight;
	public float throwHeightVar;
	void LooseMinions()
	{
        //animation-stuff
        anim.SetTrigger("bounce");

        FlobbliHandler fh = null;
		if (planet.isHome)
		{
			while (flobbliCatched.Count != 0)
			{
				ByeFlobbli(GetFlobbli());
			}
		} else
		{
			if (flobbliCatched.Count != 0)
			{
				ByeFlobbli(GetFlobbli());
			} else if (eatingFlobbli != null)
			{
				ByeFlobbli(eatingFlobbli);
				eatingFlobbli = null;
			}
		}
	}

	void ByeFlobbli(FlobbliHandler fh)
	{
		planet.tierchen++;
		fh.planet = planet;
		fh.node.rotation = Quaternion.Euler(0, 0, playerRotation);
		fh.flobbli.localPosition = new Vector3(0, playerDistance, 0);
		fh.GetLost(UnityEngine.Random.Range(-throwSpeedVar, throwSpeedVar) + throwSpeed, UnityEngine.Random.Range(-throwHeightVar, throwHeightVar) + throwHeight);
	}

	FlobbliHandler GetFlobbli()
	{
		FlobbliHandler fh = null;
		int id = 0;
		while (fh == null && flobbliCatched.Count != 0)
		{
			if (flobbliCatched[id] != null)
			{
				fh = flobbliCatched[id];
				flobbliCatched.RemoveAt(id);
			}
		}

		return fh;
	}

	void CatchProgres()
	{
        //animation-stuff
        anim.SetBool("absorbing", touchedFlobbli != null);

        if (touchedFlobbli != null)
		{
			flobbliCatchTimer -= Time.deltaTime;


			touchedFlobbli.transform.position = Vector3.MoveTowards(touchedFlobbli.transform.position, catchEnd.transform.position, Vector3.Distance(touchedFlobbli.transform.position, catchEnd.transform.position) / (flobbliCatchTimer / Time.deltaTime));
			touchedFlobbli.flobbli.transform.position = touchedFlobbli.transform.position;

			if (flobbliCatchTimer <= 0)
			{
				eatingFlobbli = touchedFlobbli;
				touchedFlobbli = null;
				flobbliEatProgress = 0f;
			}
		}

	}

	void EatProgress()
	{

		if (eatingFlobbli != null)
		{
			flobbliEatProgress += Time.deltaTime / eatTime;
			eatingFlobbli.transform.position = Vector3.Lerp(catchEnd.transform.position, stomach.transform.position, flobbliEatProgress);
			eatingFlobbli.flobbli.transform.position = eatingFlobbli.transform.position;
			if (flobbliEatProgress >= 1f)
			{
                //animation-stuff
                anim.SetTrigger("absorb");

                eatingFlobbli.transform.SetParent(stomach);
				eatingFlobbli.maxFloat = stomachRadius * player.lossyScale.x;
				eatingFlobbli.doFloat = true;
				eatingFlobbli.floatDirection = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0f).normalized;
				flobbliCatched.Add(eatingFlobbli);
				eatingFlobbli = null;

			}
		}

	}

	void InStomachStuff()
	{

	}

	public Vector3 Falloff
	{
		get { return changeForce * changeForceFalloff * changeForcePower + changeForce * changeForceFalloff * changeForcePower * maxSpeedMod * MinMaxDistance; }
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

	public float DistanceToSurface
	{
		get { return DistanceToPlanet - planet.PlanetSize; }
	}

	float FindRotation(Vector3 pos, float distance, Vector3 node)
	{
		Vector3 min = node + ((Quaternion.Euler(0, 0, 0) * Vector3.up) * distance);
		float f = 0;
		Vector3 p;
		for (float i = 1; i <= 359; i++)
		{
			p = node + ((Quaternion.Euler(0, 0, i) * Vector3.up) * distance);
			if (Vector3.Distance(pos, p) < Vector3.Distance(min, p))
			{
				min = p;
				f = i;
			}
		}

		return f;
	}

	//hi
	private void FixedUpdate()
	{
		closestPlanet = GetClosestPlanet();
		if (closestPlanet != planet && jumpTimer <= 0f) {
			if (Vector3.Distance(planet.planet.position, closestPlanet.planet.position) >=
				Vector3.Distance(planet.planet.position, player.position) +
				Vector3.Distance(player.position, closestPlanet.planet.position) * 0.99f)
			{
				planet = closestPlanet;
				playerDistance = Vector3.Distance(planet.planet.position, playerNode.position);
				playerRotation += 180f;

				jumpTimer = jumpTimeout;
				direction *= -1f;
				currentVerticalSpeed *= -1f;
				inputIsBlocked = true;
			}

		}
		jumpTimer -= Time.fixedDeltaTime;
		


		/*
		if ((planet == null || closestPlanet != planet) && jumpTimer <= 0f)
		{
			planet = closestPlanet;
			playerDistance = Vector3.Distance(planet.planet.position, playerNode.position);
			changeForce = Quaternion.Euler(0, 0, playerRotation + 90 * direction) * Vector3.up;
			changeForceFalloff = 1f;
			playerRotation = FindRotation(player.position, playerDistance, planet.planet.position);

			jumpTimer = jumpTimeout;
			direction *= -1f;
			currentVerticalSpeed *= -1f;
			inputIsBlocked = true;
		}
		jumpTimer -= Time.fixedDeltaTime;
		*/


		/*
		Vector3 tempPos = player.position;

		if ((planet == null || closestPlanet != planet) && jumpTimer <= 0f)
		{
			//Debug.Log(String.Format("Old Position: {0} {1} {2}", playerNode.GetChild(0).transform.position.x, playerNode.transform.position.y, playerNode.transform.position.z));
			jumpTimer = jumpTimeout;
			direction *= -1f;

			var savePos = playerNode.transform.position;
			//Debug.Log("Save Pos: " + savePos);

			playerNode.transform.position = closestPlanet.planet.position;
			player.transform.position = savePos;

			var newDownVec = closestPlanet.transform.position - player.GetChild(0).transform.position;

			var angle = Vector3.Angle(newDownVec, playerNode.transform.localEulerAngles);

			player.transform.Rotate(Vector3.forward, angle);

			currentVerticalSpeed *= -1f;
			inputIsBlocked = true;
			planet = closestPlanet;
			Debug.Log(player.transform.position);
			Debug.Log(playerNode.rotation);
		}
		else
		{
			playerNode.transform.position = planet.planet.position;
		}
		if (player.transform.position.y < 0f)
		{
			player.transform.position = new Vector3(-player.transform.position.x, -player.transform.position.y);
			playerNode.rotation = Quaternion.Euler(playerNode.rotation.eulerAngles.x, playerNode.rotation.eulerAngles.y, playerNode.rotation.eulerAngles.z + 180f);
			Debug.Log(player.transform.position);
			Debug.Log(playerNode.rotation);
		}
		jumpTimer -= Time.fixedDeltaTime;
		CalculateVerticalMovement();
		player.localPosition = new Vector3(player.localPosition.x, player.localPosition.y + currentVerticalSpeed, player.localPosition.z);
		if (player.localPosition.y > MaxDistance)
			player.localPosition = new Vector3(player.localPosition.x, MaxDistance, player.localPosition.z);
		playerNode.rotation = Quaternion.Euler(playerNode.rotation.eulerAngles.x, playerNode.rotation.eulerAngles.y, playerNode.rotation.eulerAngles.z + ((baseSpeed * direction) * Time.fixedDeltaTime));
		*/

		Move();
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
                //animation-stuff
                anim.SetTrigger("push");

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
			touchedGround = true;
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
