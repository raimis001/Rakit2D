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

  public static void Clear(this Transform transform)
  {
    Transform[] ts = transform.GetComponentsInChildren<Transform>(true);
    foreach (Transform t in ts)
      GameObject.Destroy(t.gameObject);      
  }

}
