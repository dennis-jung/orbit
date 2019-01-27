using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
	public PlayerController player;
	public CreateLevel levelGenerator;
	public Camera cam;

	private void LateUpdate()
	{
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -50);
		cam.orthographicSize = player.playerDistance;
	}

	float lastSize;
	public float sizeDamp;
	public float sizeDampLimit;
	void UpdateSize()
	{
		cam.orthographicSize = player.playerDistance;
	}

	[Header ("CamDistanceMods")]
	public float closeUpMod;
	public float farAwayMod;
	public float midPoint;
	float GetTargetSize()
	{
		float targetSize = 1f;
		float maxDistance = levelGenerator.minDistance;

		if (player.playerDistance > maxDistance)
			//cam.orthographicSize 
			cam.orthographicSize = player.playerDistance;


		return targetSize;
	}
}
