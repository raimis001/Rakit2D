using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
  Player player;
  SerializedProperty groundFilter;

  private void OnEnable()
  {
    player = (Player)target;

    groundFilter = serializedObject.FindProperty("groundFilter");
  }

  public override void OnInspectorGUI()
  {
    //base.OnInspectorGUI();
    serializedObject.Update();

    bool editing = false;
    Color defaultColor = GUI.backgroundColor;

    if (RaStyle.ObjectField(target, "Animator", ref player.animator))
      editing = true;

    #region SPEED
    EditorGUILayout.LabelField("Speed", EditorStyles.boldLabel);
    EditorGUI.indentLevel = 1;
    GUI.backgroundColor = RaStyle.HexColor("#B3B3B3");
    EditorGUILayout.BeginVertical("Box");

    //Speed
    //float rangeX = Mathf.FloorToInt(player.speed - 0.5f);
    //float rangeY = Mathf.CeilToInt(player.speed + 0.5f);
    if (RaStyle.Slider(target, "Speed", ref player.speed, 0, 10))
      editing = true;

    //Jump speed
    if (RaStyle.Slider(target, "Jump speed", ref player.jumpSpeed, 0.5f, 5f))
      editing = true;

    EditorGUI.indentLevel = 0;
    GUI.backgroundColor = defaultColor;
    EditorGUILayout.EndVertical();
    #endregion

    #region ATTACK
    //EditorGUILayout.LabelField("Attack", EditorStyles.boldLabel);

    //Meele 
    if (RaStyle.ToggleLeft(target, "Meele attack", ref player.isMeele))
      editing = true;

    if (player.isMeele)
    {
      EditorGUI.indentLevel = 1;
      GUI.backgroundColor = RaStyle.HexColor("#B3B3B3");
      EditorGUILayout.BeginVertical("Box");

      //Attack cool down
      if (RaStyle.Slider(target, "Cool down time", ref player.meeleCooldown, 0f, 5f))
        editing = true;

      if (RaStyle.ObjectField(target, "Weapon collider", ref player.meeleTrigger))
        editing = true;

      EditorGUI.indentLevel = 0;
      GUI.backgroundColor = defaultColor;
      EditorGUILayout.EndVertical();
    }

    if (RaStyle.ToggleLeft(target, "Range attack", ref player.isRange))
      editing = true;

    if (player.isRange)
    {
      EditorGUI.indentLevel = 1;
      GUI.backgroundColor = RaStyle.HexColor("#B3B3B3");
      EditorGUILayout.BeginVertical("Box");

      //Projectiole prefab
      if (RaStyle.ObjectField(target, "Projectile prefab", ref player.rangeProjectile))
        editing = true;

      //Projectile start transform
      if (RaStyle.ObjectField(target, "Start transform", ref player.projectileStart))
        editing = true;

      if (RaStyle.Slider(target, "Projectile force", ref player.projectileForce, 0.1f, 10f))
        editing = true;

      if (RaStyle.Toggle(target, "Destroy after shot", ref player.destroyAfterShot))
        editing = true;

      if (player.destroyAfterShot)
        if (RaStyle.Slider(target, "Destroy time", ref player.destroyTime, 1f, 10f))
          editing = true;

      EditorGUI.indentLevel = 0;
      GUI.backgroundColor = defaultColor;
      EditorGUILayout.EndVertical();
    }
    #endregion

    EditorGUI.BeginChangeCheck();
    EditorGUILayout.PropertyField(groundFilter.FindPropertyRelative("layerMask"), new GUIContent("Ground mask"));
    if (EditorGUI.EndChangeCheck())
    {
      Debug.Log("Change ground filter");
      editing = true;
    }

    if (editing)
    {
      serializedObject.ApplyModifiedProperties();
      EditorUtility.SetDirty(target);
    }
  }
}
