using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogKind
{
  OnEnter, OnKey, FadeInOut
}

[System.Serializable]
public class DialogChoise
{
  public string caption;
  public string nextDialog;
}

[System.Serializable]
public class DialogItem
{
  public string idString;
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
  public DialogKind kind;

  public string caption;
  public List<DialogItem> dialogs;

  CanvasGroup group;
  Transform cameraTransform;

  float distance => Vector2.Distance(cameraTransform.position, transform.position);

  private void Start()
  {
    if (!canvas)
    {
      canvas = GetComponentInChildren<Canvas>();
    }
    if (!canvas)
    {
      Debug.LogError("Dialog canvas not find");
      return;
    }

    if (!canvas.worldCamera)
    {
      canvas.worldCamera = Camera.main;
    }

    cameraTransform = Camera.main.transform;
    group = canvas.GetComponent<CanvasGroup>();
  }

  private void Update()
  {
    if (!canvas)
      return;
    if (!cameraTransform)
      return;

    switch (kind)
    {
      case DialogKind.FadeInOut:
      case DialogKind.OnEnter:
        CheckDistance();
        break;
      case DialogKind.OnKey:
        CheckKey();
        break;
    }      
  }



  void CheckKey()
  {
    if (distance > 2)
    {
      SM.dialogOpened = false;
      canvas.gameObject.SetActive(false);
      return;
    }

    if (SM.dialogOpened)
      return;

    if (!SM.keyInteract)
      return;

    SM.dialogOpened = true;
    canvas.gameObject.SetActive(true);

  }

  void CheckDistance()
  {
    if (!group)
      return;

    float dist = distance;

    if (dist > 4)
    {
      SM.dialogOpened = false;
      group.alpha = 0;
      canvas.gameObject.SetActive(false);
      return;
    }

    if (!canvas.gameObject.activeInHierarchy)
      canvas.gameObject.SetActive(true);

    if (dist < 2)
    {
      SM.dialogOpened = true;
      group.alpha = 1;
      return;
    }

    if (kind == DialogKind.OnEnter)
    {
      SM.dialogOpened = false;
      group.alpha = 0;
      canvas.gameObject.SetActive(false);
      return;
    }

    dist -= 2;
    group.alpha = Mathf.Lerp(0, 1, 1 - dist / 2f);
  }
}