using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
  Player player;

  private void OnEnable()
  {
    player = (Player)target;
  }

  public override void OnInspectorGUI()
  {
    bool editing = false;
    Color defaultColor = GUI.backgroundColor;

    if (RaStyle.ObjectField(target, "Animator", ref player.animator))
      editing = true;

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

    if (editing)
    {
      EditorUtility.SetDirty(target);
    }
  }
}
