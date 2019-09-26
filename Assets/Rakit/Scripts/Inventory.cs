using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public enum InventoryItemKind
{
  Item, MeeleWeapon, RangeWeapon, LivePotion, HealthPotion
}

[Serializable]
public class InventoryEvent : UnityEvent<string, int> { }

[Serializable]
public class InventoryItemName
{
  public string itemName;
}

[Serializable]
public class InventoryItem
{
  public string name;
  public InventoryItemKind kind;
  public Sprite icon;
  public Color iconTint = Color.white;
  public GameObject prefab;
}

public class Inventory : MonoBehaviour
{
	private static readonly Dictionary<string, int> items = new Dictionary<string, int>();

  public List<InventoryItem> itemsDefine = new List<InventoryItem>();

	public InventoryEvent OnInventoryChange;

	private void Awake()
	{
		SM.inventory = this;
	}
	public static void Add(string itemName, int count = 1)
	{

    if (!GetDefine(itemName, out InventoryItem item))
      return;

    if (item.kind == InventoryItemKind.LivePotion)
    {
      SM.AddLive();
      return;
    }

    if (items.ContainsKey(itemName))
		{
			items[itemName] += count;
		} else
		{
			items.Add(itemName, count);
		}
		Debug.Log("Item added:" + itemName);
		SM.inventory.OnInventoryChange.Invoke(itemName, items[itemName]);

	}
	public static void Remove(string itemName, int count = 1)
	{
		if (items.ContainsKey(itemName))
		{
			items[itemName] -= count;
			SM.inventory.OnInventoryChange.Invoke(itemName, items[itemName]);
		}
	}

	public static int Have(string item)
	{
		if (items.ContainsKey(item))
		{
			return items[item];
		}
		return 0;
	}

  public void DropItem(Vector3 position, string itemName)
  {
    if (!GetDefine(itemName, out InventoryItem item))
      return;

    position.x += Random.Range(0.5f, 1) * Mathf.Sign(Random.Range(-1,1));
    Instantiate(item.prefab, position, Quaternion.identity);
  }

  internal static void Drop(Vector3 position, string itemName)
  {
    SM.inventory.DropItem(position,itemName);
  }
  internal static bool GetDefine(string itemName, out InventoryItem item)
  {
    item = SM.inventory.itemsDefine.Find(itm => itm.name == itemName);
    return item != null;
  }
  internal static IEnumerable<InventoryItem> GetItems()
  {
    foreach (string s in items.Keys)
    {
      InventoryItem item = SM.inventory.itemsDefine.Find(itm => itm.name == s);
      yield return item;
    }
  }
}
