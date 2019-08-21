using System.Collections;
using System.Collections.Generic;
using TMPro;
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
  public InventoryItemName itemReward;

  public List<DialogChoise> choises;

  internal bool completed;
}

public class Dialogs : MonoBehaviour
{
  public Transform actor;
  public Canvas canvas;
  public DialogKind kind;

  public string caption;
  public List<DialogItem> dialogs;

  public TMP_Text textCaption;
  public TMP_Text textText;
  public TMP_Text textChoice;

  CanvasGroup group;
  Transform cameraTransform;
  int currentDialog = -1;
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

    if (dialogs != null && dialogs.Count > 0)
      currentDialog = 0;
  }

  private void Update()
  {
    if (actor)
      transform.position = actor.position;

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

    if (!SM.dialogOpened)
      return;
    if (currentDialog < 0)
      return;

    DialogItem item = dialogs[currentDialog];
    if (item.choises.Count < 1)
      return;

    if (SM.dialogChoice1)
    {
      HitChoice(0);
      return;
    }

    if (SM.dialogChoice2)
    {
      HitChoice(1);
      return;
    }

    if (SM.dialogChoice3)
    {
      HitChoice(2);
      return;
    }

  }

  void HitChoice(int choice)
  {

    DialogItem item = dialogs[currentDialog];

    if (!item.completed && item.itemReward.itemName != "")
    {
      item.completed = true;
      Inventory.Drop(transform.position, item.itemReward.itemName);
    }

    if (item.choises.Count < choice + 1)
      return;

    int next = FindDialogId(item.choises[choice].nextDialog);
    if (next < 0)
      return;

    currentDialog = next;

    DrawDialog();

  }

  int FindDialogId(string id)
  {
    if (id == "")
      return -1;

    for (int i = 0; i < dialogs.Count; i++)
    {
      if (dialogs[i].idString == id)
        return i;
    }

    return -1;
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
    DrawDialog();

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
    {
      SM.dialogOpened = true;
      canvas.gameObject.SetActive(true);
      DrawDialog();
    }

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

  void DrawDialog()
  {
    textCaption.text = caption;
    if (currentDialog < 0)
      return;

    DialogItem item = dialogs[currentDialog];
    textText.text = item.description;

    string choice = "";
    for (int i = 0; i < item.choises.Count; i++)
    {
      if (item.choises[i].caption != "")
        choice += string.Format("{0} {1}\n", i + 1, item.choises[i].caption);
    }

    textChoice.text = choice;

  }
}