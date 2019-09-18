using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{

  public Transform followObject;
  public Vector2 delta;

  // Update is called once per frame
  void Update()
  {
    if (followObject)
      transform.localPosition = followObject.localPosition + new Vector3(delta.x, delta.y);
  }
}
