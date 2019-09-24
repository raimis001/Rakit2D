using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EnemyNode
{
  public Vector3 position;
  public float waitOnNode = 0;
}

public enum EnemyType
{
  meele, range
}
public class Enemy : Interact
{
  public Animator anim;
  public Transform body;
  public bool defaultIsRight = true;

  public float speed = 1;
  public float speedBooster = 1;

  [Range(0.0f, 20f)]
  public float viewDistance = 3;
  public bool bothDirection;

  public EnemyType attackType = EnemyType.meele;
  public float attackDistance = 1;
  public float attackDamage = 1;
  public float attackCoolDown = 2;

  public Projectile rangeProjectile;
  public Transform projectileStart;
  public bool destroyProjectile;
  public float destroyProjectileTime;
  public float projectileForce;

  public float damageMeele = 33f;
  public float damageRange = 2f;
  public bool destroyOnDeath = false;
  public float destroyTime = 1;

  public GameObject canvas;
  public Image progress;
  public bool alwaysShowCanvas;

  public LayerMask seeCheckLayer;

  internal float hitpoints = 1;
  internal bool isDeath = false;

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

  private void Start()
  {
    if (!anim)
      anim = GetComponentInChildren<Animator>();
    StartCoroutine(Patrolling());

    if (canvas)
      canvas.SetActive(alwaysShowCanvas);
  }

  #region MOVE
  IEnumerator Patrolling()
  {

    int targetNode = 0;
    int direction = 1;
    while (!isDeath)
    {
      if (nodes.Count < 1)
      {
        anim.SetFloat("speed", 0);
        yield return new WaitForSeconds(0.5f);
        CheckForPlayer();
        continue;
      }

      anim.SetFloat("speed", 1);

      yield return MoveToNode(targetNode);
      yield return ProcessNode(targetNode);

      if (direction == -1 && targetNode == 0 || direction == 1 && targetNode == nodes.Count - 1)
        direction *= -1;

      targetNode += direction;
    }
  }
  IEnumerator ProcessNode(int node)
  {
    if (nodes[node].waitOnNode == 0)
      yield break;

    anim.SetFloat("speed", 0);
    float wait = nodes[node].waitOnNode;
    while (wait > 0)
    {
      yield return null;
      wait -= Time.deltaTime;
      CheckForPlayer();
    }
  }
  IEnumerator MoveToNode(int node)
  {
    Vector3 targetPos = nodes[node].position;

    isRight = body.localPosition.x < targetPos.x;

    while (Vector3.Distance(body.localPosition, targetPos) > 0.1f)
    {
      body.localPosition = Vector3.MoveTowards(body.localPosition, targetPos, Time.deltaTime * speed);
      yield return null;
      CheckForPlayer();
    }

    body.localPosition = targetPos;
  }
  #endregion

  #region PLAYER visibility
  void CheckForPlayer()
  {
    if (SeePlayer())
    {
      StopAllCoroutines();
      if (attackType == EnemyType.meele)
        StartCoroutine(GotoPlayer());
      else
        StartCoroutine(RangeAttack());
    }
  }
  private bool SeePlayer()
  {
    if (Player.IsDeath)
      return false;

    Vector2 player = Player.position;
    Vector2 self = body.position;

    if (Mathf.Abs(player.y - self.y) > 1)
      return false;

    float dist = Vector2.Distance(player, self);

    if (dist > viewDistance)
      return false;

    self.y += 0.5f;
    player.y += 0.5f;

    Vector2 dir = player - self;
    RaycastHit2D hit = Physics2D.Raycast(self, dir, dist, seeCheckLayer);

    Debug.DrawRay(self, dir);
    if (hit)
      return false;

    if (bothDirection)
      return true;

    bool right = isRight;
    bool pright = player.x - self.x >= 0;

    return right == pright;
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
        if (SeePlayer())
        {
          anim.SetFloat("speed", 1);
          body.position = Vector3.MoveTowards(body.position, new Vector3(Player.position.x, body.position.y, body.position.z), Time.deltaTime * speed * speedBooster);
        }
        else
        {
          StopAllCoroutines();
          StartCoroutine(Patrolling());
        }
      }
      else
      {
        anim.SetFloat("speed", 0);
        if (coolDown <= 0)
        {
          //Debug.Log("ATTACKIG!");
          anim.SetTrigger("attack");
          coolDown = attackCoolDown;
        }
      }

      yield return null;
      if (coolDown > 0)
        coolDown -= Time.deltaTime;

    }

  }
  #endregion

  #region ATTACK
  IEnumerator RangeAttack()
  {

    bool oldRight = isRight;
    while (true)
    {
      if (!SeePlayer())
        break;

      //Debug.Log("Attack range");
      anim.SetTrigger("attack");
      bool right = body.position.x < Player.position.x;
      if (oldRight != right)
      {
        oldRight = isRight = right;
        yield return new WaitForSeconds(0.5f);
      }

      Projectile.Shot(
        rangeProjectile,
        projectileStart,
        isRight,
        projectileForce,
        isRight ? -1 : 1,
        destroyProjectile ? destroyProjectileTime : -1
        );

      yield return new WaitForSeconds(attackCoolDown);
    }

    StopAllCoroutines();
    StartCoroutine(Patrolling());
  }
  private bool AttackPlayer()
  {
    if (Player.IsDeath)
      return false;

    return Vector3.Distance(body.position, Player.position) <= attackDistance;
  }
  #endregion

  #region ATTACKED
  public override bool Attacked(int weapon)
  {

    if (isDeath)
      return false;

    hitpoints -= (weapon == 1 ? damageMeele : damageRange) / 100f;
    if (canvas && progress)
    {
      canvas.SetActive(true);
      progress.fillAmount = hitpoints;
    }

    if (hitpoints <= 0)
    {
      isDeath = true;
      StopAllCoroutines();
      anim.SetTrigger("death");
      if (destroyOnDeath)
        Destroy(gameObject, destroyTime);
    }
    return true;
  }
  #endregion

  #region EDITOR
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
      offset.x = ((Application.isPlaying ? isRight : body.localScale.x > 0) ? viewDistance : -viewDistance) / 2f;
    }

    Vector3 size = new Vector3(bothDirection ? viewDistance * 2 : viewDistance, 1);

    Gizmos.color = new Color(0, 1, 0, 0.4f);
    Gizmos.DrawCube(body.position + offset, size);

    if (attackType == EnemyType.meele)
    {
      UnityEditor.Handles.color = new Color(1.0f, 0, 0, 0.2f);
      UnityEditor.Handles.DrawSolidArc(body.position, -Vector3.forward, Vector3.left, 180, attackDistance);
    }
  }
#endif
  #endregion
}
