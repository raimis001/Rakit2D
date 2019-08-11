using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
	public static bool IsPlatformEffector(this Collider2D coll, out PlatformEffector2D effector)
	{
		effector = coll.GetComponent<PlatformEffector2D>();
		return effector != null;
	}
}
