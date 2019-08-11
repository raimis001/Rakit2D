using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DedZone : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!Player.IsPlayer(collision))
			return;

		Player.Ded();

	}
}
