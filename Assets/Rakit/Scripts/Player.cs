using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Interact
{
  private static Vector2 checkPoint;
  public static Vector3 position => SM.player.transform.position;

  public Animator animator;

  [Header("Weapons")]
  public bool isMeele;
  public WeaponTrigger meeleTrigger;
  public float meeleCooldown = 1;

  public bool isRange;
  public Projectile rangeProjectile;
  public Transform projectileStart;
  public float projectileForce = 1;
  public bool destroyAfterShot;
  public float destroyTime = 1;

  private int _currentWeapon;
  internal int currentWeapon
  {
    set
    {
      if (_currentWeapon == value)
        return;

      _currentWeapon = value;
      animator.SetInteger("Weapon", _currentWeapon);
      animator.SetTrigger("SwitchWeapon");

    }
  }

  private PlayerMove body;

  public static bool IsPlayer(Collider2D collider)
  {
    if (SM.IsDeath)
      return false;

    return collider.GetComponent<Player>() != null;
  }

  private void Awake()
  {
    SM.player = this;
    body = GetComponent<PlayerMove>();
    if (!body)
      Debug.LogError("No player move defined");

    checkPoint = transform.position;
  }

  private void Start()
  {
    //Time.timeScale = 0.1f;\
    meeleTrigger.coolDown = meeleCooldown;
  }


  private void Update()
  {
    if (SM.IsDeath)
    {
      if (SM.keyJump)
      {
        Respawn();
      }
      return;
    }

    if (coolDownHp > 0)
      coolDownHp -= Time.deltaTime;

    //SM.SetHp(Time.deltaTime / 5f);

    if (SM.dialogOpened)
      return;

    if (isMeele && SM.keyWeapon1)
    {

      currentWeapon = _currentWeapon == 1 ? 0 : 1;
      return;
    }
    if (isRange && SM.keyWeapon2)
    {
      currentWeapon = _currentWeapon == 2 ? 0 : 2;
      return;
    }

    Vector3 rot1 = projectileStart.eulerAngles;
    if (!body.isRight)
      rot1.z = 180 - rot1.z;

    Debug.DrawRay(projectileStart.position, projectileStart.right * (body.isRight ? 3 : -3), Color.green);

    if (_currentWeapon > 0 && SM.keyAttack)
    {
      if (_currentWeapon == 1)
      {
        if (meeleTrigger.attacking)
          return;

        meeleTrigger.coolDown = meeleCooldown;
        meeleTrigger.attacking = true;
      }

      if (_currentWeapon == 2)
      {
        Projectile.Shot(
          rangeProjectile,
          projectileStart,
          0,
          body.isRight,
          projectileForce,
          body.isRight ? (body.defaultIsRight ? -1 : 1) : (body.defaultIsRight ? 1 : -1),
          destroyAfterShot ? destroyTime : -1
        );
      }

      animator.SetTrigger("Attack");
      return;
    }

  }

  public void Death()
  {
    if (SM.IsDeath)
      return;

    SM.IsDeath = true;

    body.StopBody();

    Debug.Log("Player is death");
  }
  public void SetSpawn(Vector2 pos)
  {
    if (!SM.IsDeath)
      return;

    SM.IsDeath = false;
    transform.position = pos;

    body.RestoreBody();

    Debug.Log("Player is respawn");
  }
  public static void Respawn()
  {
    SM.player.SetSpawn(checkPoint);
  }
  public static void Save()
  {
    Debug.Log("Save position");
    checkPoint = SM.player.transform.position;
  }

  private float coolDownHp = 0;
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (coolDownHp > 0)
      return;

    Enemy enemy = collision.GetComponentInParent<Enemy>();
    if (!enemy)
      return;

    SM.SetHp(enemy.attackDamage / 100f);
    coolDownHp = 1;

  }

  public override bool Attacked(int weapon, float damage = 0)
  {
    SM.SetHp(damage / 100f);

    return true;
  }
}