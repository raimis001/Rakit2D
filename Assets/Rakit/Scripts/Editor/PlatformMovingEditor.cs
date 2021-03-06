﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/*
 //Auto fit styles:
 EditorGUILayout.BeginVertical("Box");
 EditorGUILayout.BeginVertical("Button");
 EditorGUILayout.BeginVertical("TextArea");
 EditorGUILayout.BeginVertical("Window");
 EditorGUILayout.BeginVertical("Textfield");
 EditorGUILayout.BeginVertical("HorizontalScrollbar"); //Fixed height
 EditorGUILayout.BeginVertical("Label"); //No style
 EditorGUILayout.BeginVertical("Toggle"); //Just puts a non usable CB to the left 
 EditorGUILayout.BeginVertical("Toolbar"); //Fixed height
 */

[CustomEditor(typeof(PlatformMoving))]
public class PlatformMovingEditor : Editor
{

	PlatformMoving platform;
	Transform transform => platform.transform;

	float testSlider;
  bool nodesFold;
	private void OnEnable()
	{
		platform = target as PlatformMoving;
		testSlider = 0;
		platform.platform.localPosition = Vector3.zero;
	}
	public override void OnInspectorGUI()
	{

    //base.OnInspectorGUI();
    EditorGUI.BeginChangeCheck();
    bool startMove = EditorGUILayout.Toggle("Move on start", platform.moveOnStart);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Changed Moving Platform action");
      platform.moveOnStart = startMove;
    }

    EditorGUI.BeginChangeCheck();
		PlatformMovingKind kind = (PlatformMovingKind)EditorGUILayout.EnumPopup("Looping", platform.kind);
		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(target, "Changed Moving Platform type");
			platform.kind = kind;
		}

		EditorGUI.BeginChangeCheck();
		float newSpeed = EditorGUILayout.FloatField("Speed", platform.speed);
		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(target, "Changed Speed");
			platform.speed = newSpeed;
		}

		EditorGUI.BeginChangeCheck();
		platform.platform = EditorGUILayout.ObjectField("Platform", platform.platform, typeof(Transform), true) as Transform;
		if (EditorGUI.EndChangeCheck())
			Undo.RecordObject(target, "Changed platform");

		EditorGUILayout.Separator();

    int delete = -1;
    nodesFold = EditorGUILayout.Foldout(nodesFold, "Nodes");
    if (nodesFold)
    {

      EditorGUILayout.BeginVertical("Textfield");

      EditorGUILayout.LabelField("Node 0");

      EditorGUILayout.BeginHorizontal("Box");
      EditorGUILayout.LabelField("", GUILayout.Width(5));
      EditorGUI.BeginChangeCheck();
      float fDelay = EditorGUILayout.FloatField("Wait Time", platform.zeroDelay);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(target, "Changed zero delay");
        platform.zeroDelay = fDelay;
      }

      EditorGUILayout.EndHorizontal();
      EditorGUILayout.EndVertical();

      for (int i = 0; i < platform.nodes.Count; i++)
      {
        EditorGUI.BeginChangeCheck();
        PlatformNode node = platform.nodes[i];

        EditorGUILayout.BeginVertical("Textfield");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Node " + (i + 1).ToString() + "   " + string.Format("x:{0:0.000} y:{1:0.000}", node.position.x, node.position.y));
        if (GUILayout.Button("Delete", GUILayout.Width(50)))
        {
          delete = i;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal("Box");

        EditorGUILayout.LabelField("", GUILayout.Width(5));
        float wDelay = EditorGUILayout.FloatField("Wait Time", node.waitOnNode);

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
          Undo.RecordObject(target, "Changed node " + i);
          node.waitOnNode = wDelay;
        }
      }
    }

		Vector3 last = platform.nodes.Count > 0 ? platform.nodes[platform.nodes.Count - 1].position : Vector3.zero;
		if (GUILayout.Button("Add Node"))
		{
			Undo.RecordObject(target, "added node");
			last += Vector3.up;
			platform.nodes.Add(new PlatformNode() { position = last });
		}

		testSlider = EditorGUILayout.Slider("Preview position", testSlider, 0.0f, 1.0f);
		PreviewPlatform();

		if (delete > -1)
		{
			Undo.RecordObject(target, "Delete node " + delete);
			platform.nodes.RemoveAt(delete);
		}
		EditorUtility.SetDirty(target);
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
		EditorUtility.SetDirty(target);

	}
	void PreviewPlatform()
	{
		if (Application.isPlaying)
			return;

		platform.MovePlatform(testSlider);

		SceneView.RepaintAll();
	}
}
