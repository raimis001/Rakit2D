using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlatformMovingKind
{
	Once, PinPong, Loop
}

[System.Serializable]
public class PlatformNode
{
	public Vector3 position;
	public float waitOnNode = 0;
}
public class PlatformMoving : Interact
{
	public Transform platform;
	public float speed = 1;
	public PlatformMovingKind kind;
	public List<PlatformNode> nodes = new List<PlatformNode>();

	public float zeroDelay;

	int currentNode = 0;
	Vector3[] localNodes;
	bool isMoving;

	public void StartMove(Interact parent)
	{
		if (isMoving)
			return;
		StartCoroutine(Move());
	}

	public void StopMove(Interact parent)
	{
		if (!isMoving)
			return;
		StopAllCoroutines();
		isMoving = false;
	}
	private void Start()
	{
		int nodesCount = nodes.Count;
		localNodes = new Vector3[nodesCount + 1];
		localNodes[0] = Vector3.zero;
		for (int i = 0; i < nodesCount; i++)
			localNodes[i + 1] = nodes[i].position;

		
	}

	IEnumerator Move(float delay = 0)
	{
		isMoving = true;
		if (delay > 0)
			yield return new WaitForSeconds(delay);

		//Debug.Log("Start move:" + currentNode);

		int startNode = currentNode;
		int nodesCount = startNode == 0 ? nodes.Count : 0;
		int sign = startNode == 0 ? 1 : -1;

		while (startNode != nodesCount)
		{
			Vector3 target = localNodes[startNode + sign];
			platform.localPosition = Vector3.MoveTowards(platform.localPosition, target , Time.deltaTime * speed);
			if (Vector3.Distance(platform.localPosition, target) < 0.001f)
			{
				int onNode = sign > 0 ? startNode : startNode - 2;
				//Debug.Log("Node reach:" + onNode);

				if (onNode >= 0 && nodes[onNode].waitOnNode > 0)
					yield return new WaitForSeconds(nodes[onNode].waitOnNode);

				startNode += sign;
				currentNode = startNode;
			}
			yield return null;
		}
		//Debug.Log("End move:" + currentNode);

		if (kind == PlatformMovingKind.Once)
		{
			isMoving = false;
			yield break;
		}

		if (kind == PlatformMovingKind.PinPong)
		{
			if (currentNode == 0)
			{
				isMoving = false;
				yield break;
			}
		}

		if (currentNode == 0 && zeroDelay > 0)
			yield return new WaitForSeconds(zeroDelay);

		StartCoroutine(Move());
	}

#if UNITY_EDITOR
	public void MovePlatform(float distance)
	{

		int nodesCount = nodes.Count;
		if (nodesCount < 1)
		{
			platform.localPosition = Vector3.zero;
			return;
		}

		Vector3[] localNodes = new Vector3[nodesCount + 1];
		localNodes[0] = Vector3.zero;
		for (int i = 0; i < nodesCount; i++)
		{
			localNodes[i + 1] = nodes[i].position;
		}

		float step = 1.0f / nodesCount;
		int starting = Mathf.FloorToInt(distance / step);

		if (starting > nodesCount - 1)
		{
			platform.localPosition = localNodes[localNodes.Length - 1];
			return;
		}

		float localRatio = (distance - (step * starting)) / step;

		Vector3 localPos = Vector3.Lerp(localNodes[starting], localNodes[starting + 1], localRatio);
		platform.localPosition = localPos;

	}
#endif
}
