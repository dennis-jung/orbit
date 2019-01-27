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
		float distance = player.DistanceToSurface;
		float maxDistance = level.minDistance / 2f;

		if (distance > maxDistance)
			distance = maxDistance;
		return distance + distance * distanceMod * Mathf.InverseLerp(0f, maxDistance, distance) + baseSize;
	}
}
