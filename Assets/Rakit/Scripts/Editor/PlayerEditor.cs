using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
  Player player;


  SerializedProperty weaponTrigger;
  SerializedProperty rangeProjectile;
  SerializedProperty projectileStart;
  SerializedProperty groundFilter;
  SerializedProperty animator;
  private void OnEnable()
  {
    player = (Player)target;

    weaponTrigger = serializedObject.FindProperty("meeleTrigger");
    rangeProjectile = serializedObject.FindProperty("rangeProjectile");
    projectileStart = serializedObject.FindProperty("projectileStart");
    groundFilter = serializedObject.FindProperty("groundFilter");
    animator = serializedObject.FindProperty("animator");
  }

  public override void OnInspectorGUI()
  {
    //base.OnInspectorGUI();
    serializedObject.Update();

    bool editing = false;
    Color defaultColor = GUI.backgroundColor;

    EditorGUI.BeginChangeCheck();
    EditorGUILayout.PropertyField(animator);
    if (EditorGUI.EndChangeCheck())
    {
      editing = true;
    }

    #region SPEED
    EditorGUILayout.LabelField("Speed", EditorStyles.boldLabel);
    EditorGUI.indentLevel = 1;
    GUI.backgroundColor = RaStyle.HexColor("#B3B3B3");
    EditorGUILayout.BeginVertical("Box");


    //Range damage
    //float rangeX = Mathf.FloorToInt(player.speed - 0.5f);
    //float rangeY = Mathf.CeilToInt(player.speed + 0.5f);
    EditorGUI.BeginChangeCheck();
    float speed = EditorGUILayout.Slider("Speed", player.speed, 0, 10);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Speed change");
      player.speed = speed;
      editing = true;
    }

    EditorGUI.BeginChangeCheck();
    float jSpeed = EditorGUILayout.Slider("Jump speed", player.jumpSpeed, 0.5f, 5f);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Jump speed change");
      player.jumpSpeed = jSpeed;
      editing = true;
    }


    EditorGUI.indentLevel = 0;
    GUI.backgroundColor = defaultColor;
    EditorGUILayout.EndVertical();
    #endregion

    #region ATTACK
    //EditorGUILayout.LabelField("Attack", EditorStyles.boldLabel);

    //Meele 
    EditorGUI.BeginChangeCheck();
    bool weapon = EditorGUILayout.ToggleLeft("Meele attack",player.isMeele);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Attack change");
      player.isMeele = weapon;
      editing = true;
    }

    if (player.isMeele)
    {
      EditorGUI.indentLevel = 1;
      GUI.backgroundColor = RaStyle.HexColor("#B3B3B3");
      EditorGUILayout.BeginVertical("Box");

      EditorGUI.BeginChangeCheck();
      float mCool = EditorGUILayout.Slider("Cool down time", player.meeleCooldown, 0f, 5f);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(target, "Cool down change");
        player.meeleCooldown = mCool;
        editing = true;
      }


      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(weaponTrigger);
      if (EditorGUI.EndChangeCheck())
      {
        editing = true;
      }

      EditorGUI.indentLevel = 0;
      GUI.backgroundColor = defaultColor;
      EditorGUILayout.EndVertical();
    }

    EditorGUI.BeginChangeCheck();
    weapon = EditorGUILayout.ToggleLeft("Range attack", player.isRange);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Range attack change");
      player.isRange = weapon;
      editing = true;
    }

    if (player.isRange)
    {
      EditorGUI.indentLevel = 1;
      GUI.backgroundColor = RaStyle.HexColor("#B3B3B3");
      EditorGUILayout.BeginVertical("Box");

      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(rangeProjectile);
      if (EditorGUI.EndChangeCheck())
      {
        Debug.Log("Change projectile");
        editing = true;
      }

      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(projectileStart);
      if (EditorGUI.EndChangeCheck())
      {
        Debug.Log("Change projectile start");
        editing = true;
      }

      EditorGUI.BeginChangeCheck();
      float mCool = EditorGUILayout.Slider("Projectile force", player.projectileForce, 0.1f, 10f);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(target, "Force change");
        player.projectileForce= mCool;
        editing = true;
      }


      EditorGUI.BeginChangeCheck();
      weapon = EditorGUILayout.Toggle("Destroy after shot", player.destroyAfterShot);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(target, "Destroy on shot change");
        player.destroyAfterShot = weapon;
        editing = true;
      }

      if (player.destroyAfterShot)
      {
        EditorGUI.BeginChangeCheck();
        mCool = EditorGUILayout.Slider("Dstroy time", player.destroyTime, 1f, 10f);
        if (EditorGUI.EndChangeCheck())
        {
          Undo.RecordObject(target, "Destroy time change");
          player.destroyTime = mCool;
          editing = true;
        }
      }
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
