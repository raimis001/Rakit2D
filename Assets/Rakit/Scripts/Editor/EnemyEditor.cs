using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

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
    bool editing = false;

    //Body transform
    if (RaStyle.ObjectField(target, "Body", ref enemy.body))
      editing = true;

    //Projectile parent
    if (RaStyle.ObjectField(target, "Projectile transform", ref enemy.rangeParent))
      editing = true;

    //Default rotation
    if (RaStyle.Toggle(target, "Default is right?", ref enemy.defaultIsRight))
      editing = true;

    #region SPEED
    EditorGUILayout.LabelField("Speed", EditorStyles.boldLabel);
    EditorGUI.indentLevel = 1;
    GUI.backgroundColor = RaStyle.HexColor("#B3B3B3");
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
    GUI.backgroundColor = RaStyle.HexColor("#B3B3B3");
    EditorGUILayout.BeginVertical("Box");

    //Direction
    if (RaStyle.Toggle(target, "Both direction", ref enemy.bothDirection))
      editing = true;

    //Distance
    if (RaStyle.Slider(target, "Sign distance", ref enemy.viewDistance, 1, 10))
      editing = true;

    //Checked layers
    if (RaStyle.LayerMask(target, "Check layer", ref enemy.seeCheckLayer))
      editing = true;

    EditorGUI.indentLevel = 0;
    GUI.backgroundColor = defaultColor;
    EditorGUILayout.EndVertical();
    #endregion

    #region ATTACK

    EditorGUILayout.LabelField("Attack", EditorStyles.boldLabel);
    EditorGUI.indentLevel = 1;

    //Attacking layer
    if (RaStyle.EnumPopup(target, "Current layer", ref enemy.interactLayer))
      editing = true;

    //Attack type
    if (RaStyle.EnumPopup(target, "Attack type", ref enemy.attackType))
      editing = true;

    GUI.backgroundColor = RaStyle.HexColor("#B3B3B3");
    EditorGUILayout.BeginVertical("Box");

    //Attack damage
    if (RaStyle.Slider(target, "Attack damage", ref enemy.attackDamage, 1f, 100))
      editing = true;

    //Attack cooldown
    if (RaStyle.FloatField(target, "Attack cool dwon", ref enemy.attackCoolDown))
      editing = true;

    //MEELE attack
    if (enemy.attackType == EnemyType.meele)
    {
      //Attack distance
      if (RaStyle.Slider(target, "Projectile force", ref enemy.attackDistance, 1f, enemy.viewDistance))
        editing = true;

    }
    else //RANGE attack
    {
      //Projectile spawnpoint
      if (RaStyle.ObjectField(target, "Projectile spawn point", ref enemy.projectileStart))
        editing = true;

      //Projectile prefab
      if (RaStyle.ObjectField(target, "Projectile prefab", ref enemy.rangeProjectile))
        editing = true;

      //Projectile force
      if (RaStyle.Slider(target, "Projectile force", ref enemy.projectileForce, 0.1f, 10))
        editing = true;

      //Destroy projectile
      if (RaStyle.Toggle(target, "Destroy prejectile", ref enemy.destroyProjectile))
        editing = true;

      //Destroy time
      if (enemy.destroyProjectile)
        if (RaStyle.Slider(target, "Projectile TTL", ref enemy.destroyProjectileTime, 0, 5))
          editing = true;

    }

    EditorGUI.indentLevel = 0;
    GUI.backgroundColor = defaultColor;
    EditorGUILayout.EndVertical();
    #endregion

    #region DAMAGE
    EditorGUILayout.LabelField("Damage", EditorStyles.boldLabel);
    EditorGUI.indentLevel = 1;
    GUI.backgroundColor = RaStyle.HexColor("#B3B3B3");
    EditorGUILayout.BeginVertical("Box");

    //Meele damage
    if (RaStyle.Slider(target, "Meele damage", ref enemy.damageMeele, 1, 100))
      editing = true;

    //Range damage
    if (RaStyle.Slider(target, "Range damage", ref enemy.damageRange, 1, 100))
      editing = true;

    //Destroy on ded
    if (RaStyle.Toggle(target, "Destroy on death", ref enemy.destroyOnDeath))
      editing = true;

    //Destroy time
    if (enemy.destroyOnDeath)
      if (RaStyle.FloatField(target, "Destroy time", ref enemy.destroyTime))
        editing = true;


    EditorGUI.indentLevel = 0;
    GUI.backgroundColor = defaultColor;
    EditorGUILayout.EndVertical();
    #endregion

    #region CANVAS
    EditorGUILayout.LabelField("Canvas", EditorStyles.boldLabel);
    EditorGUI.indentLevel = 1;
    GUI.backgroundColor = RaStyle.HexColor("#B3B3B3");
    EditorGUILayout.BeginVertical("Box");

    //Always show canvas
    if (RaStyle.Toggle(target, "Always show canvas", ref enemy.alwaysShowCanvas))
      editing = true;

    //Canvas
    if (RaStyle.ObjectField(target, "Canvas", ref enemy.canvas))
      editing = true;

    //Progress bar
    if (RaStyle.ObjectField(target, "Progress bar", ref enemy.progress))
      editing = true;

    EditorGUI.indentLevel = 0;
    GUI.backgroundColor = defaultColor;
    EditorGUILayout.EndVertical();
    #endregion


    if (editing)
      EditorUtility.SetDirty(target);

    #region NODES
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
          EditorUtility.SetDirty(target);
        }
      }

      if (GUILayout.Button("Add Node"))
      {
        Undo.RecordObject(target, "Add node");
        Vector3 last = Vector3.right;
        if (enemy.nodes.Count > 0)
          last = enemy.nodes[enemy.nodes.Count - 1].position + new Vector3(2, 0);
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
    #endregion

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

}












