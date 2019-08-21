using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyType
{
	Key, Button, Axis
}
public enum KeyKind
{
	Down, Up, Key
}
[System.Serializable]
public struct KeyBind
{
	public KeyType type;
	public string inputName;
	public float axisCheck;
	public KeyCode key;
	public bool isShift;
	public bool isControl;
	public KeyKind kind;
	public bool IsKey => CheckKey();

	private bool CheckShift()
	{
		return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
	}
	private bool CheckControl()
	{
		return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) ||
					Input.GetKey(KeyCode.LeftCommand) || !Input.GetKey(KeyCode.RightCommand);
	}
	private bool CheckKey()
	{
		if (type == KeyType.Button)
		{
			switch (kind)
			{
				case KeyKind.Down:
					if (!Input.GetButtonDown(inputName))
						return false;
					break;
				case KeyKind.Up:
					if (!Input.GetButtonUp(inputName))
						return false;
					break;
				default:
					if (!Input.GetButton(inputName))
						return false;
					break;
			}

			if (isShift && !CheckShift())
				return false;

			if (isControl && !CheckControl())
				return false;

			return true;
		}

		if (type == KeyType.Axis)
		{

			if (axisCheck < 0)
				return Input.GetAxis(inputName) < axisCheck;

			if (axisCheck > 0)
				return Input.GetAxis(inputName) > axisCheck;

			return Mathf.Abs(Input.GetAxis(inputName)) > 0.1f;
		}

		switch (kind)
		{
			case KeyKind.Down:
				if (!Input.GetKeyDown(key))
					return false;
				break;
			case KeyKind.Up:
				if (!Input.GetKeyUp(key))
					return false;
				break;
			default:
				if (!Input.GetKey(key))
					return false;
				break;
		}

		if (isShift && !CheckShift())
			return false;

		if (isControl && !CheckControl())
			return false;

		return true;
	}
}

public class SM : MonoBehaviour
{
	public static Inventory inventory;
	public static Player player;

	public static SM instance;

	[Header("Key bindings")]
	public string horizontal = "Horizontal";
	public KeyBind interactKey = new KeyBind() { type = KeyType.Key, key = KeyCode.E, kind = KeyKind.Down };
	public KeyBind jumpKey = new KeyBind() { type = KeyType.Button, inputName="Jump", kind = KeyKind.Down };
	public KeyBind downKey = new KeyBind() { type = KeyType.Axis, inputName = "Vertical", axisCheck = -0.1f };

  [Header("Dialog choices")]
  public KeyBind choice1 = new KeyBind() { type = KeyType.Key, key = KeyCode.Alpha1, kind = KeyKind.Down };
  public KeyBind choice2 = new KeyBind() { type = KeyType.Key, key = KeyCode.Alpha2, kind = KeyKind.Down };
  public KeyBind choice3 = new KeyBind() { type = KeyType.Key, key = KeyCode.Alpha3, kind = KeyKind.Down };

  private void Awake()
	{
		instance = this;
	}

	public static float keyMove => Input.GetAxis(instance.horizontal);
	public static bool keyInteract => instance.interactKey.IsKey;
	public static bool keyJump => instance.jumpKey.IsKey;
	public static bool keyDown => instance.downKey.IsKey;

  public static bool dialogChoice1 => instance.choice1.IsKey;
  public static bool dialogChoice2 => instance.choice2.IsKey;
  public static bool dialogChoice3 => instance.choice3.IsKey;

  public static bool dialogOpened;

  public void TestInventory(string item, int cnt)
  {

  }
}
