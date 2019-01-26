using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
	public Transform node;
	private void FixedUpdate()
	{
		transform.rotation = Quaternion.Euler(0, 0, -node.rotation.eulerAngles.z);
	}
}
