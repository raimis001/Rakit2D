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

    //Body transform
    EditorGUI.BeginChangeCheck();
    Transform bTransform = EditorGUILayout.ObjectField("Body", enemy.body, typeof(Transform), true) as Transform;
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Changed body");
      enemy.body = bTransform;
    }

    //Speed
    EditorGUI.BeginChangeCheck();
    float bSpeed = EditorGUILayout.FloatField("Speed", enemy.speed);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Changed speed");
      enemy.speed = bSpeed;
    }

    //Distance
    EditorGUI.BeginChangeCheck();
    float bDist = EditorGUILayout.Slider("Sign distance", enemy.viewDistance, 1, 10);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Changed sign distance");
      enemy.viewDistance = bDist;
    }
    //Attack distance
    EditorGUI.BeginChangeCheck();
    float bADist = EditorGUILayout.Slider("Attack distance", enemy.attackDistance, 1, enemy.viewDistance);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Changed sign distance");
      enemy.attackDistance = bADist;
    }
    //Direction
    EditorGUI.BeginChangeCheck();
    bool bDir = EditorGUILayout.Toggle("Both direction", enemy.bothDirection);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Changed sign direction");
      enemy.bothDirection = bDir;
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

      if (GUILayout.Button("Add Node"))
      {
        Undo.RecordObject(target, "Add node");
        Vector3 last = enemy.nodes[enemy.nodes.Count - 1].position + new Vector3(1, 0);
        enemy.nodes.Add(new EnemyNode() { position = last });
      }

      EditorGUILayout.EndVertical();

      if (delete > -1)
      {
        Undo.RecordObject(target, "Delete node " + delete);
        enemy.nodes.RemoveAt(delete);
      }

   
    }
    EditorUtility.SetDirty(target);
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

      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(target, "Changed node pos " + i);
        node.position = transform.InverseTransformPoint(nPos);
      }

      lastNodePos = nPos;
    }


  }

  //#if UNITY_EDITOR
  //  private void OnDrawGizmosSelected()
  //  {
  //    //draw the cone of view
  //    Vector3 forward = spriteFaceLeft ? Vector2.left : Vector2.right;
  //    forward = Quaternion.Euler(0, 0, spriteFaceLeft ? -viewDirection : viewDirection) * forward;

  //    if (GetComponent<SpriteRenderer>().flipX) forward.x = -forward.x;

  //    Vector3 endpoint = transform.position + (Quaternion.Euler(0, 0, viewFov * 0.5f) * forward);

  //    Handles.color = new Color(0, 1.0f, 0, 0.2f);
  //    Handles.DrawSolidArc(transform.position, -Vector3.forward, (endpoint - transform.position).normalized, viewFov, viewDistance);

  //    //Draw attack range
  //    Handles.color = new Color(1.0f, 0, 0, 0.1f);
  //    Handles.DrawSolidDisc(transform.position, Vector3.back, meleeRange);
  //  }
  //#endif
}












