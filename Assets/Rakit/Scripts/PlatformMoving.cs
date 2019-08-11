using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlatformNode
{
	public Vector3 position;
}
public class PlatformMoving : Interact
{
	public Transform platform;

	public List<PlatformNode> nodes = new List<PlatformNode>(); 

	public void StartMove(Interact parent)
	{

	}

	public void StopMove(Interact parent)
	{

	}

	public void MovePlatform(float distance)
	{
		if (Application.isPlaying)
			return;

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

}
