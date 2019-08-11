using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(PlatformMoving))]
public class PlatformMovingEditor : Editor
{

	PlatformMoving platform;
	Transform transform => platform.transform;

	float testSlider;
	private void OnEnable()
	{
		platform = target as PlatformMoving;
		testSlider = 0;
		platform.platform.localPosition = Vector3.zero;
	}
	public override void OnInspectorGUI()
	{

		base.OnInspectorGUI();

		Vector3 last = platform.nodes.Count > 0 ? platform.nodes[platform.nodes.Count - 1].position : Vector3.zero;
		if (GUILayout.Button("Add Node"))
		{
			Undo.RecordObject(target, "added node");
			last += Vector3.up;
			platform.nodes.Add(new PlatformNode() { position = last });
		}

		testSlider = EditorGUILayout.Slider("Preview position", testSlider, 0.0f, 1.0f);
		PreviewPlatform();

	}

	private void OnSceneGUI()
	{

		//Handles.PositionHandle(transform.position, Quaternion.identity);
		Vector3 lastNodePos = transform.position;
		for (int i = 0; i < platform.nodes.Count; i++)
		{
			PlatformNode node = platform.nodes[i];

			Vector3 wPos = transform.TransformPoint(node.position);
			Vector3 nPos = Handles.PositionHandle(wPos, Quaternion.identity);
			node.position = transform.InverseTransformPoint(nPos);


			Handles.DrawDottedLine(lastNodePos, nPos, 10);
			lastNodePos = nPos;
		}
		PreviewPlatform();
	}
	void PreviewPlatform()
	{
		if (Application.isPlaying)
			return;

		platform.MovePlatform(testSlider);
	
		SceneView.RepaintAll();
	}
}
