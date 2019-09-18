﻿using System.Collections;
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
    Color defaultColor = GUI.backgroundColor;

    //base.OnInspectorGUI();
    bool editing = false;
    //Body transform
    EditorGUI.BeginChangeCheck();
    Transform bTransform = EditorGUILayout.ObjectField("Body", enemy.body, typeof(Transform), true) as Transform;
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Changed body");
      enemy.body = bTransform;
      editing = true;
    }
    #region SPEED
    EditorGUILayout.LabelField("Speed", EditorStyles.boldLabel);
    EditorGUI.indentLevel = 1;
    GUI.backgroundColor = HexColor("#B3B3B3");
    EditorGUILayout.BeginVertical("Box");

    //Speed
    EditorGUI.BeginChangeCheck();
    float bSpeed = EditorGUILayout.FloatField("Speed", enemy.speed);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Changed speed");
      enemy.speed = bSpeed;
      editing = true;
    }

    //speed boost
    EditorGUI.BeginChangeCheck();
    float bSpBoost = EditorGUILayout.Slider("Sign speed boost", enemy.speedBooster, 1, 3);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Changed speed boost");
      enemy.speedBooster = bSpBoost;
      editing = true;
    }
    EditorGUILayout.EndVertical();
    GUI.backgroundColor = defaultColor;
    EditorGUI.indentLevel = 0;
    #endregion

    #region DISTANCE
    EditorGUILayout.LabelField("Distances", EditorStyles.boldLabel);
    EditorGUI.indentLevel = 1;
    GUI.backgroundColor = HexColor("#B3B3B3");
    EditorGUILayout.BeginVertical("Box");

    //Direction
    EditorGUI.BeginChangeCheck();
    bool bDir = EditorGUILayout.Toggle("Both direction", enemy.bothDirection);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Changed sign direction");
      enemy.bothDirection = bDir;
      editing = true;
    }
    //Distance
    EditorGUI.BeginChangeCheck();
    float bDist = EditorGUILayout.Slider("Sign distance", enemy.viewDistance, 1, 10);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Changed sign distance");
      enemy.viewDistance = bDist;
      editing = true;
    }
    //Attack distance
    EditorGUI.BeginChangeCheck();
    float bADist = EditorGUILayout.Slider("Attack distance", enemy.attackDistance, 1, enemy.viewDistance);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Changed sign distance");
      enemy.attackDistance = bADist;
      editing = true;
    }
    EditorGUI.indentLevel = 0;
    GUI.backgroundColor = defaultColor;
    EditorGUILayout.EndVertical();
    #endregion

    #region ATTACK
    EditorGUILayout.LabelField("Attack", EditorStyles.boldLabel);
    EditorGUI.indentLevel = 1;
    GUI.backgroundColor = HexColor("#B3B3B3");
    EditorGUILayout.BeginVertical("Box");

    //Attack damage
    EditorGUI.BeginChangeCheck();
    float bADam = EditorGUILayout.Slider("Attack damage", enemy.attackDamage, 1, 100);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Changed damage ");
      enemy.attackDamage = bADam;
      editing = true;
    }
    //Attack cooldown
    EditorGUI.BeginChangeCheck();
    float bCool = EditorGUILayout.FloatField("Attack cool dwon", enemy.attackCoolDown);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Changed cooldown");
      enemy.attackDamage = bCool;
      editing = true;
    }
    EditorGUI.indentLevel = 0;
    GUI.backgroundColor = defaultColor;
    EditorGUILayout.EndVertical();
    #endregion


    #region DAMAGE
    EditorGUILayout.LabelField("Damage", EditorStyles.boldLabel);
    EditorGUI.indentLevel = 1;
    GUI.backgroundColor = HexColor("#B3B3B3");
    EditorGUILayout.BeginVertical("Box");

    //Meele damage
    EditorGUI.BeginChangeCheck();
    float mDam = EditorGUILayout.Slider("Meele damage", enemy.damageMeele, 1, 100);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Meele damage");
      enemy.damageMeele = mDam;
      editing = true;
    }

    //Range damage
    EditorGUI.BeginChangeCheck();
    float rDam = EditorGUILayout.Slider("Range damage", enemy.damageRange, 1, 100);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Range damage");
      enemy.damageRange = rDam;
      editing = true;
    }
    //Destroy on ded
    EditorGUI.BeginChangeCheck();
    bool bDestr = EditorGUILayout.Toggle("Destroy on ded", enemy.destroyOnDed);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Range damage");
      enemy.destroyOnDed= bDestr;
      editing = true;
    }

    //Destroy time
    EditorGUI.BeginChangeCheck();
    float fDestr = EditorGUILayout.FloatField("Destroy time", enemy.destroyTime);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Range damage");
      enemy.destroyTime= fDestr;
      editing = true;
    }

    EditorGUI.indentLevel = 0;
    enemy.destroyOnDed = bDestr;
    EditorGUILayout.EndVertical();
    #endregion


    if (editing)
      EditorUtility.SetDirty(target);

    //Nodes
    int delete = -1;
    nodesFold = EditorGUILayout.Foldout(nodesFold, "Nodes", new GUIStyle(EditorStyles.foldout) { fontStyle = FontStyle.Bold });
    if (nodesFold)
    {

      EditorGUILayout.BeginVertical();

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

        EditorGUILayout.BeginHorizontal();

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
        Vector3 last = enemy.nodes[enemy.nodes.Count - 1].position + new Vector3(2, 0);
        enemy.nodes.Add(new EnemyNode() { position = last });
        EditorUtility.SetDirty(target);
      }

      EditorGUILayout.EndVertical();

      if (delete > -1)
      {
        Undo.RecordObject(target, "Delete node " + delete);
        enemy.nodes.RemoveAt(delete);
        EditorUtility.SetDirty(target);
      }


    }
    //EditorUtility.SetDirty(target);
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
  private Color HexColor(string hex)
  {
    ColorUtility.TryParseHtmlString(hex, out Color color);
    return color;
  }

  private Texture2D MakeTex(int width, int height, Color col)
  {
    Color[] pix = new Color[width * height];

    for (int i = 0; i < pix.Length; i++)
      pix[i] = col;

    Texture2D result = new Texture2D(width, height);
    result.SetPixels(pix);
    result.Apply();

    return result;
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












