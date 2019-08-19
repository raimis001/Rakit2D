using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogChoise
{
  public int choise;
  public string caption;
  public string nextDialog;
}

[System.Serializable]
public class DialogItem
{
  [TextArea]
  public string description;

  public List<DialogChoise> choises;

  internal bool showing;
  internal bool completed;
}

public class Dialogs : MonoBehaviour
{
  public Transform actor;
  public Canvas canvas;
  public string caption;
  public List<DialogItem> dialogs;

  CanvasGroup group;

  private void Start()
  {
    group = canvas.GetComponent<CanvasGroup>();
  }
  private void Update()
  {
    //Debug.Log((Camera.main.transform.position - transform.position).sqrMagnitude);
    float dist = Vector2.Distance(Camera.main.transform.position, transform.position);
    if (dist > 4)
    {
      group.alpha = 0;
      return;
    }
    if (dist < 2)
    {
      group.alpha = 1;
      return;
    }
    dist -= 2;
    group.alpha = Mathf.Lerp(0, 1, 1 - dist / 2f);
  }
}