using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
}
