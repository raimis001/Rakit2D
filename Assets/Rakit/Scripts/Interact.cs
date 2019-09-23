using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum InteractLayer
{
  Player, Enemy, None
}

[System.Serializable]
public class InteractEvent : UnityEvent<Interact> { }
public class Interact : MonoBehaviour
{

  public Transform rangeParent;
  public InteractLayer interactLayer;

  public virtual bool Attacked(int weapon)
  {
    return false;
  }

  public virtual bool Operate(bool isKeyboard)
  {
    return true;
  }
  
}
