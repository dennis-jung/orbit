using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
	public PlayerController player;
	public CreateLevel level;
	public Camera cam;

	private void LateUpdate()
	{
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -50);
		UpdateSize();
	}

	private IEnumerator Start()
	{
		yield return null;
		cam.orthographicSize = GetTargetSize();
	}

	void UpdateSize()
	{
		float current = cam.orthographicSize;
		float target = GetTargetSize();
		float maxChange = 1f;
		float dif = target - current;
		float dir = 1f;
		if (dif < 0f)
			dir = -1f;

		if (Mathf.Abs(dif) < maxChange)
		{
			cam.orthographicSize = target;
		} else if (Mathf.Abs(dif) > maxChange)
		{
			cam.orthographicSize = current + maxChange * dir;
		}
		else
		{
			cam.orthographicSize = current + dif * dir;
		}





	}

	[Header ("CamDistanceMods")]
	public float distanceMod;
	public float baseSize;
	float GetTargetSize()
	{
<<<<<<< HEAD
		float distance = player.DistanceToSurface;
		float maxDistance = level.minDistance / 2f;
=======
		float targetSize = 1f;
		float maxDistance = levelGenerator.minDistance;

		if (player.playerDistance > maxDistance)
			//cam.orthographicSize 
			cam.orthographicSize = player.playerDistance;

>>>>>>> 6daf515e145e7e06ac82aa844bf8f3134097a675

		if (distance > maxDistance)
			distance = maxDistance;
		return distance + distance * distanceMod * Mathf.InverseLerp(0f, maxDistance, distance) + baseSize;
	}
}
