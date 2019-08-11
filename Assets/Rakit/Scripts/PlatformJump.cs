using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformJump : MonoBehaviour
{
	public bool canClimbDown = true;
	[Tooltip("Filter for checking player")]
	public ContactFilter2D checkFilter;

	Rigidbody2D body;
	PlatformEffector2D effector;

	bool isPlayer => body.IsTouching(checkFilter);

	private void Awake()
	{
		body = GetComponent<Rigidbody2D>();
		effector = GetComponent<PlatformEffector2D>();
	}

	private void Update()
	{
		if (!canClimbDown)
			return;

		if (!SM.keyDown)
			return;

		if (!isPlayer)
			return;

		effector.rotationalOffset = 180;
		Invoke("ReleaseEffector",1);
	}
	void ReleaseEffector()
	{
		effector.rotationalOffset = 0;
	}

}