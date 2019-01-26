using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatcherController : MonoBehaviour
{
	public PlayerController player;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log(collision.gameObject.name);
		if (collision.gameObject.tag == "Flobbli")
		{
			FlobbliHandler handler = collision.transform.parent.GetComponent<FlobbliHandler>();
			if (handler != null)
				player.CatchedSomething(handler);
		}
	}
}
