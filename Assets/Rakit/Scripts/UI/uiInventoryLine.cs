using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LineClickEvent : UnityEvent<InventoryItem> { }

public class uiInventoryLine : MonoBehaviour, IPointerClickHandler
{
  public Image icon;
  public TMP_Text caption;

  public LineClickEvent OnLineClick = new LineClickEvent();

  private InventoryItem currentItem;

  public void OnPointerClick(PointerEventData eventData)
  {
    if (OnLineClick != null)
      OnLineClick.Invoke(currentItem);
  }

  public void SetLine(InventoryItem item)
  {
    currentItem = item;

    icon.sprite = item.icon;
    icon.color = item.iconTint;
    caption.text = item.name;
  }

}
