using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteract : TriggerInteract
{
  [Header("Reward")]
  public InventoryItemName itemReward;
  public bool destroyOnGet = true;

	bool markAsDestroy;

  public override bool Operate(bool isKeyboard)
  {
    if (markAsDestroy)
      return false;

    if (itemReward.itemName != "")
      Inventory.Add(itemReward.itemName);

    if (destroyOnGet)
    {
      markAsDestroy = true;
      Destroy(gameObject, 1f);
    }
    return true;
  }
}
