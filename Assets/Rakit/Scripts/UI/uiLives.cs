using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class uiLives : MonoBehaviour
{
  public uiLive livesPrefab;

  private List<uiLive> lives = new List<uiLive>();


  private void Start()
  {
    if (SM.MaxLives < 0)
    {
      livesPrefab.gameObject.SetActive(false);
      return;
    }

    lives.Add(livesPrefab);


    for (int i = 1; i < SM.MaxLives; i++)
    {
      uiLive live = Instantiate(livesPrefab, transform);
      live.gameObject.SetActive(true);
      lives.Add(live);
    }
    
  }

  private void OnEnable()
  {
    SM.OnLivesChange += OnLivesChange;
  }
  private void OnDisable()
  {
    SM.OnLivesChange -= OnLivesChange;
  }

  private void OnLivesChange()
  {
   // Debug.LogFormat("L:{0} H:{1}", SM.Lives, SM.Hp);

    if (lives.Count  < SM.MaxLives)
    {
      //TODO make new live
      uiLive live = Instantiate(livesPrefab, transform);
      live.gameObject.SetActive(true);
      lives.Add(live);
      return;
    }

    for (int i = 0; i < lives.Count; i++)
    {
      uiLive live = lives[i];
      live.gameObject.SetActive(i < SM.Lives);
      live.progress = i == SM.Lives - 1 ? SM.Hp : 1;
    }
  }
}
