using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Inventory))]
public class InventoryEditor : Editor
{
  Inventory inventory;
  bool itemsFold;
  List<bool> previewFold = new List<bool>();

  private void OnEnable()
  {
    inventory = target as Inventory;
  }

  public override void OnInspectorGUI()
  {
    //base.OnInspectorGUI();

    serializedObject.Update();
    EditorGUILayout.PropertyField(this.serializedObject.FindProperty("OnInventoryChange"), true);

    int delete = -1;
    itemsFold = EditorGUILayout.Foldout(itemsFold, "Iems");
    if (itemsFold)
    {
      for (int i = 0; i < inventory.itemsDefine.Count; i++)
      {
        InventoryItem item = inventory.itemsDefine[i];

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginVertical("Textfield");
        EditorGUILayout.BeginVertical("Box");


        EditorGUILayout.BeginHorizontal("Box");

        Sprite iIcon = (Sprite)EditorGUILayout.ObjectField(
          item.icon, typeof(Sprite), false,
          GUILayout.Height(65), GUILayout.Width(65)
        );

        EditorGUILayout.BeginVertical();
        string iName = EditorGUILayout.TextField("Item name",item.name);
        Color iColor = EditorGUILayout.ColorField("Tint color", item.iconTint);

        GameObject prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", item.prefab, typeof(GameObject), true);

        if (GUILayout.Button("Delete")) 
        {
          delete = i;
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        if (i >= previewFold.Count)
          previewFold.Add(false);

        int previousIndentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 2;
        previewFold[i] = EditorGUILayout.Foldout(previewFold[i], "Preview");
        EditorGUI.indentLevel = previousIndentLevel;

        if (previewFold[i] && item.icon)
        {
          SetTextureImporterFormat(item.icon.texture, true);

          Texture2D sourceTex = item.icon.texture;
          Texture2D destTex = new Texture2D(sourceTex.width, sourceTex.height);
          Color[] pix = sourceTex.GetPixels();

          //Tint color
          for (int c = 0; c < pix.Length; ++c)
          {
            pix[c].r = pix[c].r - (1.0f - iColor.r);
            pix[c].g = pix[c].g - (1.0f - iColor.g);
            pix[c].b = pix[c].b - (1.0f - iColor.b);

            //Color32 Tint
            //pix[c].r = (byte)((int)pix[c].r * (int)item.iconTint.r / 255);
            //pix[c].g = (byte)((int)pix[c].g * (int)item.iconTint.g / 255);
            //pix[c].b = (byte)((int)pix[c].b * (int)item.iconTint.b / 255);
          }

          destTex.SetPixels(pix);
          destTex.Apply();

          float ratio = sourceTex.width / sourceTex.height;

          GUILayout.Box(destTex,GUILayout.Width(65 * ratio), GUILayout.Height(65));
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
          Undo.RecordObject(target, "Changed item " + i);
          item.name = iName;
          item.icon = iIcon;
          item.iconTint = iColor;
          item.prefab = prefab;
        }
      }
    }
    if (GUILayout.Button("Add item"))
    {
      Undo.RecordObject(target, "added node");
      inventory.itemsDefine.Add(new InventoryItem() { name = "New item" });
      itemsFold = true;
    }

    if (delete >= 0)
    {
      inventory.itemsDefine.RemoveAt(delete);
    }

    serializedObject.ApplyModifiedProperties();
    EditorUtility.SetDirty(target);

  }
    
  private void OnSceneGUI()
  {

  }

  public static void SetTextureImporterFormat(Texture2D texture, bool isReadable)
  {
    if (texture == null)
      return;
    if (texture.isReadable == isReadable)
      return;

    string assetPath = AssetDatabase.GetAssetPath(texture);
    var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
    if (tImporter != null)
    {
      tImporter.textureType = TextureImporterType.Sprite;
      if (tImporter.isReadable == isReadable)
        return;

      tImporter.isReadable = isReadable;

      AssetDatabase.ImportAsset(assetPath);
      AssetDatabase.Refresh();
    }
  }
}