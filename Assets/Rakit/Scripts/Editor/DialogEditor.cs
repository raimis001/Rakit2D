using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Dialogs))]
public class DialogEditor : Editor
{
  SerializedProperty actorProp;
  SerializedProperty canvasProp;
  SerializedProperty kindProp;
  SerializedProperty captionProp;
  SerializedProperty dialogsProp;
  SerializedProperty txtCaptionProp;
  SerializedProperty txtTextProp;
  SerializedProperty txtChoiceProp;

  Dialogs dialog;
  bool dialogFold;
  bool uiFold;
  private void OnEnable()
  {
    actorProp = serializedObject.FindProperty("actor");
    canvasProp = serializedObject.FindProperty("canvas");
    kindProp = serializedObject.FindProperty("kind");
    captionProp = serializedObject.FindProperty("caption");
    dialogsProp = serializedObject.FindProperty("dialogs");

    txtCaptionProp = serializedObject.FindProperty("textCaption");
    txtTextProp = serializedObject.FindProperty("textText");
    txtChoiceProp = serializedObject.FindProperty("textChoice");

    dialog = (Dialogs)target;
  }
  public override void OnInspectorGUI()
  {
    //base.OnInspectorGUI();
    serializedObject.Update();

    EditorGUILayout.PropertyField(actorProp);
    EditorGUILayout.PropertyField(canvasProp);
    EditorGUILayout.PropertyField(kindProp);

    EditorGUILayout.Space();
    EditorGUILayout.PropertyField(captionProp);

    EditorGUILayout.Space();

    uiFold = EditorGUILayout.Foldout(uiFold, "UI");
    if (uiFold)
    {
      EditorGUILayout.PropertyField(txtCaptionProp);
      EditorGUILayout.PropertyField(txtTextProp);
      EditorGUILayout.PropertyField(txtChoiceProp);

      EditorGUILayout.Space();
    }

    dialogFold = EditorGUILayout.Foldout(dialogFold, "Dialogs");

    if (dialogFold)
    {
      int delete = -1;
      for (int i = 0; i < dialog.dialogs.Count; i++)
      {
        delete = DrawItem(i, dialog.dialogs[i]);
      }
      if (delete > -1)
        dialog.dialogs.RemoveAt(delete);
    }

    if (GUILayout.Button("Add dialog"))
    {
      dialog.dialogs.Add(new DialogItem());
    }

    serializedObject.ApplyModifiedProperties();
    EditorUtility.SetDirty(target);
  }

  private int DrawItem(int id, DialogItem item)
  {
    int delete = -1;
    EditorGUILayout.BeginVertical("box");
    EditorGUI.BeginChangeCheck();

    EditorGUILayout.BeginHorizontal();
    string ns = EditorGUILayout.TextField("Dialog ID", item.idString);
    if (GUILayout.Button("-", GUILayout.Width(40)))
    {
      delete = id;
    }
    EditorGUILayout.EndHorizontal();

    var ele = dialogsProp.GetArrayElementAtIndex(id);
    EditorGUILayout.PropertyField(ele.FindPropertyRelative("itemReward"), new GUIContent("Reward"));

    string ds = EditorGUILayout.TextArea(item.description, GUILayout.Height(50));

    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Changed item " + id);
      item.description = ds;
      item.idString = ns;
    }

    DrawChoices(item);

    EditorGUILayout.EndVertical();
    EditorGUILayout.Space();

    return delete;
  }

  private void DrawChoices(DialogItem item)
  {
    int delete = -1;
    if (item.choises == null)
    {
      item.choises = new List<DialogChoise>();
      item.choises.Add(new DialogChoise());
    }

    for (int i = 0; i < item.choises.Count; i++)
    {

      DialogChoise choice = item.choises[i];
      EditorGUI.BeginChangeCheck();

      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField("choice - " + i);
      if (GUILayout.Button("-",GUILayout.Width(40)))
      {
        delete = i;
      }
      EditorGUILayout.EndHorizontal();

      int previousIndentLevel = EditorGUI.indentLevel;
      EditorGUI.indentLevel = 2;

      string cs = EditorGUILayout.TextField("Text", choice.caption);
      string ns = EditorGUILayout.TextField("Next dialog ID", choice.nextDialog);

      EditorGUI.indentLevel = previousIndentLevel;

      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(target, "Changed choice " + i);
        choice.caption = cs;
        choice.nextDialog = ns;
      }
    }
    if (item.choises.Count < 3)
    {
      EditorGUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      if (GUILayout.Button("+", GUILayout.Width(40)))
      {
        item.choises.Add(new DialogChoise());
      }
      EditorGUILayout.EndHorizontal();
    }

    if (delete > -1)
      item.choises.RemoveAt(delete);

  }
}