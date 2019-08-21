using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(InventoryItemName))]
public class InventoryItemProperty : PropertyDrawer
{

  int selected = 0;
  Inventory inventory;
  public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
  {
    if (!inventory)
    {
      inventory = GameObject.FindObjectOfType<Inventory>();
    }

    if (!inventory)
    {
      GUI.Label(position, "No inventory in scene");
      return;
    }

    EditorGUI.BeginProperty(position, label, prop);

    string val = prop.FindPropertyRelative("itemName").stringValue;

    string[] options = new string[inventory.itemsDefine.Count+1];
    options[0] = "None";
    selected = 0;
    for (int i = 0; i < inventory.itemsDefine.Count; i++)
    {
      options[i+1] = inventory.itemsDefine[i].name;
      if (options[i+1] == val)
        selected = i+1;
    }

    selected = EditorGUI.Popup(position, label.text, selected, options);

    prop.FindPropertyRelative("itemName").stringValue = selected == 0 ? "" : options[selected];

    EditorGUI.EndProperty();
  }



}