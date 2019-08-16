using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InteractEvent : UnityEvent<Interact> { }
public class Interact : MonoBehaviour
{

  public virtual bool Operate(bool isKeyboard)
  {
    return true;
  }
  
}
