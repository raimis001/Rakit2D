using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public static class RaStyle 
{

  public static Color HexColor(string hex)
  {
    ColorUtility.TryParseHtmlString(hex, out Color color);
    return color;
  }

  public static Texture2D MakeTex(int width, int height, Color col)
  {
    Color[] pix = new Color[width * height];

    for (int i = 0; i < pix.Length; i++)
      pix[i] = col;

    Texture2D result = new Texture2D(width, height);
    result.SetPixels(pix);
    result.Apply();

    return result;
  }

  public static bool Slider(Object target,string caption,ref float value, float min, float max)
  {
    EditorGUI.BeginChangeCheck();
    float val = EditorGUILayout.Slider(caption, value, min , max);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Changed " + caption);
      value = val;
      return true;
    }

    return false;
  }

  public static bool Toggle(Object target, string caption, ref bool value)
  {
    EditorGUI.BeginChangeCheck();
    bool val = EditorGUILayout.Toggle(caption, value);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Changed " + caption);
      value = val;
      return true;
    }

    return false;
  }
  public static bool ToggleLeft(Object target, string caption, ref bool value)
  {
    EditorGUI.BeginChangeCheck();
    bool val = EditorGUILayout.ToggleLeft(caption, value);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Changed " + caption);
      value = val;
      return true;
    }

    return false;
  }

  public static bool FloatField(Object target, string caption, ref float value)
  {
    EditorGUI.BeginChangeCheck();
    float val = EditorGUILayout.FloatField(caption, value);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Changed " + caption);
      value = val;
      return true;
    }

    return false;
  }

  public static bool ObjectField<T>(Object target, string caption, ref T value) where T : Object
  {
    EditorGUI.BeginChangeCheck();
    var val = EditorGUILayout.ObjectField(caption, value, typeof(T), true);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Changed " + caption);
      value = (T)val;
      return true;
    }
    return false;
  }

  public static bool EnumPopup<T>(Object target, string caption, ref T value) where T : System.Enum
  {
    EditorGUI.BeginChangeCheck();
    var val = EditorGUILayout.EnumPopup(caption, value);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Changed " + caption);
      value = (T)val;
      return true;
    }
    return false;
  }

  public static bool LayerMask(Object target, string caption, ref LayerMask value)
  {

    LayerMask FieldToLayerMask(int field)
    {
      LayerMask mask = 0;
      var layers = InternalEditorUtility.layers;
      for (int c = 0; c < layers.Length; c++)
      {
        if ((field & (1 << c)) != 0)
        {
          mask |= 1 << UnityEngine.LayerMask.NameToLayer(layers[c]);
        }
      }
      return mask;
    }
    // Converts a LayerMask to a field value
    int LayerMaskToField(LayerMask mask)
    {
      int field = 0;
      var layers = InternalEditorUtility.layers;
      for (int c = 0; c < layers.Length; c++)
      {
        if ((mask & (1 << UnityEngine.LayerMask.NameToLayer(layers[c]))) != 0)
        {
          field |= 1 << c;
        }
      }
      return field;
    }


    EditorGUI.BeginChangeCheck();
    LayerMask val = EditorGUILayout.MaskField(caption, LayerMaskToField(value), InternalEditorUtility.layers);
    if (EditorGUI.EndChangeCheck())
    {
      Undo.RecordObject(target, "Changed " + caption);
      value = FieldToLayerMask(val);
      return true;
    }
    return false;
  }
}
