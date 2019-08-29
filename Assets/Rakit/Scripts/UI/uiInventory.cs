using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class uiInventory : MonoBehaviour
{
  public uiInventoryLine linePrefab;
  public RectTransform content;

  private float lineHeight;
  private Vector2 contentSize;
  private Dictionary<string, uiInventoryLine> lines = new Dictionary<string, uiInventoryLine>();

  private IEnumerator Start()
  {
    linePrefab.gameObject.SetActive(false);
    Debug.Log(((RectTransform)linePrefab.transform).sizeDelta);

    lineHeight = ((RectTransform)linePrefab.transform).sizeDelta.y;

    VerticalLayoutGroup group = content.GetComponent<VerticalLayoutGroup>();
    if (group)
      lineHeight += group.spacing;

    contentSize = content.sizeDelta;
    contentSize.y = lineHeight;

    yield return null;
    content.sizeDelta = contentSize;
    gameObject.SetActive(false);
  }

  public void AddLine(InventoryItem item)
  {
    uiInventoryLine line = Instantiate(linePrefab, content);
    line.SetLine(item);
    line.gameObject.SetActive(true);

    lines.Add(item.name, line);
    contentSize.y += lineHeight;
  }

  public void OnInventoryChange(string itemName, int cnt)
  {
    contentSize.y = lineHeight;

    Inventory.GetDefine(itemName, out InventoryItem item);
    if (item == null)
      return;

    if (cnt == 0)
    {
      if (!lines.ContainsKey(item.name))
        return;

      Destroy(lines[item.name].gameObject);
      lines.Remove(item.name);
      return;
    }

    if (lines.ContainsKey(item.name))
      return;

    AddLine(item);
    content.sizeDelta = contentSize;
  }
}
