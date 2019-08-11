using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catcher : MonoBehaviour
{
	private class CatcherNode
	{
		public Rigidbody2D rigi;
		public bool isConnect;

		private bool attached;
		private Transform transform => rigi.transform;

		public void Reparent(Transform parent)
		{
			if (isConnect && attached)
				return;
			if (!isConnect && !attached)
				return;

			transform.SetParent(isConnect ? parent : null);
			attached = isConnect;

		}
	}

	public ContactFilter2D contactFilter;

	readonly List<CatcherNode> nodes = new List<CatcherNode>();
	Rigidbody2D body;
	ContactPoint2D[] contactPoints = new ContactPoint2D[20];
	private void Awake()
	{
		body = GetComponent<Rigidbody2D>();
	}
	private void Start()
	{
	}

	private void Update()
	{
		

	}

	void FixedUpdate()
	{
		foreach (CatcherNode node in nodes)
		{
			node.isConnect = false;
		}

		int contactCount = body.GetContacts(contactFilter, contactPoints);
		for (int i = 0; i < contactCount; i++)
		{
			ContactPoint2D contact = contactPoints[i];
			
			//Contact with self
			if (contact.rigidbody == body)
				continue;

			float dot = Vector2.Dot(contact.normal, Vector2.down);
			//No top contact
			if (dot < 0.8f)
				continue;

			Rigidbody2D rigi = contact.rigidbody;
			//Contact not with rigidbody
			if (rigi == null)
				continue;

			CatcherNode node = nodes.Find((n) => n.rigi == rigi);
			if (node == null)
			{
				node = new CatcherNode()
				{
					rigi = rigi
				};
				nodes.Add(node);
				Debug.Log(contact.collider.name);
			}

			node.isConnect = true;

		}

		foreach (CatcherNode node in nodes)
		{
			node.Reparent(transform);
		}
	}
}
