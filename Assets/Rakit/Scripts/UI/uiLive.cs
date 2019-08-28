using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class uiLive : MonoBehaviour
{
  public Image progressImage;

  public float progress
  {
    set
    {
      progressImage.fillAmount = Mathf.Clamp(value, 0, 1);
    }
  }

}
