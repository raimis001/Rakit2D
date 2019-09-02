using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyNode
{
  public Vector3 position;
  public float waitOnNode = 0;
}

public class Enemy : Interact
{
  public Animator anim;
  public Transform body;
  public float speed = 1;

  public List<EnemyNode> nodes = new List<EnemyNode>();

  private bool isRight {
    set
    {
      if (anim.GetBool("isRight") == value)
        return;

      anim.SetBool("isRight", value);
      anim.SetTrigger("turn");
    }
    get { return anim.GetBool("isRight"); }
  }
  private Coroutine patroll;
  private void Start()
  {
    if (!anim)
      anim = GetComponentInChildren<Animator>();
    patroll = StartCoroutine(Patrolling());
  }

  IEnumerator Patrolling()
  {
    if (nodes.Count < 1)
    {
      patroll = null;
      yield break;
    }

    int targetNode = 0;
    int direction = 1;
    while (true)
    {
      anim.SetFloat("speed", 1);
      yield return MoveToNode(targetNode);
      yield return ProcessNode(targetNode);

      if (direction == -1 && targetNode == 0 ||  direction == 1 && targetNode == nodes.Count - 1)
      {
        //Reach end node
        direction *= -1;
      }

      targetNode += direction;

    }
  }

  IEnumerator ProcessNode(int node)
  {
    if (nodes[node].waitOnNode == 0)
      yield break;

    anim.SetFloat("speed", 0);
    yield return new WaitForSeconds(nodes[node].waitOnNode);
  }
  IEnumerator MoveToNode(int node)
  {
    Vector3 targetPos = nodes[node].position;

    isRight = body.localPosition.x < targetPos.x;

    while (Vector3.Distance(body.localPosition, targetPos) > 0.1f)
    {
      body.localPosition = Vector3.MoveTowards(body.localPosition, targetPos, Time.deltaTime * speed);
      yield return null;
    }

    body.localPosition = targetPos;
  }

  public override void Attacked(int weapon)
  {
    base.Attacked(weapon);
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    Debug.Log(collision.name);
  }
}
