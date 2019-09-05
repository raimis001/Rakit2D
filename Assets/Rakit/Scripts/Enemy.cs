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
  public float speedBooster = 1;

  [Range(0.0f, 20f)]
  public float viewDistance = 3;
  public bool bothDirection;

  public float attackDistance = 1;


  public float attackDamage = 1; 


  public List<EnemyNode> nodes = new List<EnemyNode>();

  private bool isRight
  {
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

      if (direction == -1 && targetNode == 0 || direction == 1 && targetNode == nodes.Count - 1)
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

      if (SeePlayer())
      {
        Debug.Log("I see player");
        patroll = null;
        StopAllCoroutines();
        StartCoroutine(GotoPlayer());
      }

    }

    body.localPosition = targetPos;
  }
  IEnumerator GotoPlayer()
  {
    bool attackSequence = true;
    float coolDown = 0;

    while (attackSequence)
    {
      isRight = body.position.x < Player.position.x;
      if (!AttackPlayer())
      {
        //TODO moving
        anim.SetFloat("speed", 1);
        body.position = Vector3.MoveTowards(body.position, Player.position, Time.deltaTime * speed * speedBooster);
      }
      else
      {
        anim.SetFloat("speed", 0);
        if (coolDown <= 0)
        {
          //Debug.Log("ATTACKIG!");
          anim.SetTrigger("attack");
          coolDown = 3;
        }
      }

      yield return null;
      if (coolDown > 0)
        coolDown -= Time.deltaTime;

    }

  }
  private bool SeePlayer()
  {
    Vector2 player = Player.position;
    Vector2 self = body.position;

    if (Mathf.Abs(player.y - self.y) > 1)
      return false;

    if (!bothDirection)
    {
      bool right = body.localScale.x > 0;
      bool pright = player.x - self.x >= 0;
      if (right && pright)
        return Vector2.Distance(player, self) <= viewDistance;
      if (!right && !pright)
        return Vector2.Distance(player, self) <= viewDistance;
      return false;
    }

    return Vector2.Distance(player, self) <= viewDistance;
  }
  private bool AttackPlayer()
  {
    return Vector3.Distance(body.position, Player.position) <= attackDistance;
  }
  public override void Attacked(int weapon)
  {
    base.Attacked(weapon);
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    Debug.Log(collision.name);
  }

#if UNITY_EDITOR
  private void OnDrawGizmos()
  {
    //Draw nodes point
    UnityEditor.Handles.color = new Color(1, 1, 0, 0.5f);
    for (int i = 0; i < nodes.Count; i++)
    {
      UnityEditor.Handles.ConeHandleCap(i, transform.TransformPoint(nodes[i].position) + new Vector3(0, 0.25f, 0), Quaternion.Euler(90, 0, 0), 0.5f, EventType.Repaint);
    }

    if (!body)
    {
      UnityEditor.Handles.Label(transform.position + new Vector3(0.5f, 1.2f), "No body assign");
      return;
    }

    Vector3 offset = new Vector3(0, 0.5f);
    if (!bothDirection)
    {
      offset.x = (body.localScale.x > 0 ? viewDistance : -viewDistance) / 2f;
    }

    Vector3 size = new Vector3(bothDirection ? viewDistance * 2 : viewDistance, 1);

    Gizmos.color = new Color(0, 1, 0, 0.4f);
    Gizmos.DrawCube(body.position + offset, size);

    UnityEditor.Handles.color = new Color(1.0f, 0, 0, 0.2f);
    UnityEditor.Handles.DrawSolidArc(body.position, -Vector3.forward, Vector3.left, 180, attackDistance);

  }
#endif

}
