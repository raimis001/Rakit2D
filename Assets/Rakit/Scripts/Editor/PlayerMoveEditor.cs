using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerMove))]
public class PlayerMoveEditor : Editor
{
  PlayerMove player;
  SerializedProperty groundFilter;

  private void OnEnable()
  {
    player = (PlayerMove)target;
    groundFilter = serializedObject.FindProperty("groundFilter");
  }

  public override void OnInspectorGUI()
  {
    serializedObject.Update();
    bool editing = false;
    Color defaultColor = GUI.backgroundColor;

    if (RaStyle.Toggle(target, "Default is right", ref player.defaultIsRight))
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
