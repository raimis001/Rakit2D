using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : TriggerInteract
{
  public override bool Operate(bool isKeyboard)
  {
    Player.Save();
    return true;
  }
  
}