using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class uiInventoryLine : MonoBehaviour
{
  public Image icon;
  public TMP_Text caption;
  
  public void SetLine(InventoryItem item)
  {
    icon.sprite = item.icon;
    icon.color = item.iconTint;
    caption.text = item.name;
  }

}
