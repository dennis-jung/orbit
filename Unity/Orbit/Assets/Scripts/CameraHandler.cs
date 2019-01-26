using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
	public Transform player;
	public PlayerController playerController;
	public Camera cam;

	private void LateUpdate()
	{
		transform.position = new Vector3(player.position.x, player.position.y, -50);
		cam.orthographicSize = player.localPosition.y;
	}
}
