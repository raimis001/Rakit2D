using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{

  private Enemy enemy;
  private Transform transform => enemy.transform;
  private bool nodesFold;
  private void OnEnable()
  {
    enemy = (Enemy)target;
  }
  public override void OnInspectorGUI()
  {
    //base.OnInspectorGUI();

    EditorGUI.BeginChangeCheck();
    Transform bTransform = EditorGUILayout.ObjectField("Body", enemy.body, typeof(Transform), true) as Transform;
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Changed body");
      enemy.body = bTransform;
    }
    EditorGUI.BeginChangeCheck();
    float bSpeed = EditorGUILayout.FloatField("Speed", enemy.speed);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Changed speed");
      enemy.speed = bSpeed;
    }

    int delete = -1;
    nodesFold = EditorGUILayout.Foldout(nodesFold, "Nodes");
    if (nodesFold)
    {

      EditorGUILayout.BeginVertical("Textfield");

      for (int i = 0; i < enemy.nodes.Count; i++)
      {
        EditorGUI.BeginChangeCheck();
        EnemyNode node = enemy.nodes[i];

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

      EditorGUILayout.EndVertical();
    }
  }

  private void OnSceneGUI()
  {

    //Handles.PositionHandle(transform.position, Quaternion.identity);

    Vector3 lastNodePos = transform.position;
    for (int i = 0; i < enemy.nodes.Count; i++)
    {
      EnemyNode node = enemy.nodes[i];

      EditorGUI.BeginChangeCheck();
      Vector3 wPos = transform.TransformPoint(node.position);
      Vector3 nPos = Handles.PositionHandle(wPos, Quaternion.identity);

      Handles.DrawDottedLine(lastNodePos, nPos, 10);
      Handles.ConeHandleCap(i, nPos + new Vector3(0, 0.25f, 0), Quaternion.Euler(90, 0, 0), 0.5f, EventType.Repaint);

      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(target, "Changed node pos " + i);
        node.position = transform.InverseTransformPoint(nPos);
      }

      lastNodePos = nPos;
    }

    //EditorUtility.SetDirty(target);

  }
}












