using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SM : MonoBehaviour
{
  public static SM instance;

  public static Inventory inventory;
  public static Player player;

  #region KEYS
  public static float keyMove => Input.GetAxis(Keys.instance.horizontal);
  public static bool keyInteract => Keys.instance.interactKey.IsKey;
  public static bool keyJump => Keys.instance.jumpKey.IsKey;
  public static bool keyDown => Keys.instance.downKey.IsKey;

  public static bool dialogChoice1 => Keys.instance.choice1.IsKey;
  public static bool dialogChoice2 => Keys.instance.choice2.IsKey;
  public static bool dialogChoice3 => Keys.instance.choice3.IsKey;

  public static bool keyAttack => Keys.instance.attack.IsKey;
  public static bool keyWeapon1 => Keys.instance.weapon1.IsKey;
  public static bool keyWeapon2 => Keys.instance.weapon2.IsKey;
  #endregion

  public static bool dialogOpened;

  public delegate void LivesChange();
  public static event LivesChange OnLivesChange;

  public static int Lives = 3;
  public static float Hp = 1;

  public int maxLives = 3;

  public static void SetHp(float hp)
  {
    Hp -= hp;
    if (Hp > 0)
    {
      OnLivesChange.Invoke();
      return;
    }

    Lives--;
    if (Lives < 0)
    {
      //TODO ded
      OnLivesChange.Invoke();
      return;
    }

    Hp = 1;
    OnLivesChange.Invoke();

  }

  private void Awake()
  {
    instance = this;
    Lives = maxLives;
  }
  public void TestInventory(string itemName, int cnt)
  {
    if (!Inventory.GetDefine(itemName, out InventoryItem item))
      return;

    if (item.kind == InventoryItemKind.MeeleWeapon)
    {
      player.isMeele = true;
      player.currentWeapon = 1;
    }

    if (item.kind == InventoryItemKind.RangeWeapon)
    {
      player.isRange = true;
      player.currentWeapon = 2;
    }
  }
}
