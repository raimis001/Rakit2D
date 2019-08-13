using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
  public Sprite icon;
  public Color iconTint = Color.white;
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
	public static void Add(string item, int count = 1)
	{
		if (items.ContainsKey(item))
		{
			items[item] += count;
		} else
		{
			items.Add(item, count);
		}
		Debug.Log("Item added:" + item);
		SM.inventory.OnInventoryChange.Invoke(item, items[item]);

	}
	public static void Remove(string item, int count = 1)
	{
		if (items.ContainsKey(item))
		{
			items[item] -= count;
			SM.inventory.OnInventoryChange.Invoke(item, items[item]);
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
}
