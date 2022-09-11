using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyBehavior : MonoBehaviour
{

    public EnemyPathFinding npcScript;

    public int health = 50;

    bool isDead;

    private Animator npcAnimator;

    public Transform rayCast;
    public LayerMask raycastMask;

    public float rayCastLength;
    public float attackDistance;
    private float distance;
    private RaycastHit2D hitLeft;
    private RaycastHit2D hitRight;
    private GameObject target;

    private bool onCooldown;
    private float attackCooldown = 2f;
    private float intCooldownTime;

    public int batonDamage = 15;
    public Transform attackPoint;
    public Transform firePoint;
    public LayerMask playerLayer;
    public float attackRange = 0.5f;

    public AudioSource npcSoundSource;
    public AudioClip batonSwing;
    public AudioClip gunShot;
    public AudioClip impactSound;
    public AudioClip deathSound;

    public GameObject bulletTracer;

    bool attacking = false;

    //=================Animation State===========//
    private string currentAnimation;
    private float hurtTime = 0.5f;
    private float attackTime = 0.5f;
    protected const string HENCHMAN_DIE = "Henchman_Die";
    protected const string HENCHMAN_HURT = "Henchman_Hurt";
    protected const string HENCHMAN_IDLE = "Henchman_Idle";
    protected const string HENCHMAN_MELEE_ATTACK = "Henchman_Melee_Attack";
    protected const string HENCHMAN_PISTOL_SHOOT = "Henchman_Pistol_shoot";
    protected const string HENCHMAN_RUN = "Henchman_Run";

    protected void Start()
    {
        StopMove();
        isDead = false;
        onCooldown = false;

        target = GameObject.FindWithTag("Player");
        //npcAnimator.SetBool("IsDead", isDead);
    }

    protected void Awake()
    {
        intCooldownTime = attackCooldown;
        npcAnimator = GetComponent<Animator>();
    }

    protected void Update()
    {
        hitLeft = Physics2D.Raycast(rayCast.position, Vector2.left, rayCastLength, raycastMask);
        hitRight = Physics2D.Raycast(rayCast.position, Vector2.right, rayCastLength, raycastMask);

        if (isDead == false)
        {
            if (hitLeft.collider != null || hitRight.collider != null)
            {
                EnemyDecision();
            }
            else if (hitLeft.collider == null || hitRight.collider == null)
            {
                StopMove();
            }
        }
    }

    protected void EnemyDecision()
    {
        distance = Vector2.Distance(rayCast.position, target.transform.position);

        if (distance > attackDistance)
        {
            Move();
        }
        else if (distance <= attackDistance && onCooldown == false)
        {
            if(gameObject.tag == "Enemy")
            {
                Attack();
            }
            if(gameObject.tag =="EnemyRange")
            {
                Shoot();
            }
        }
        if (onCooldown)
        {
            AttackCooldown();
        }
    }

    protected virtual void Attack()
    {
        //npcAnimator.SetTrigger("Attack");
        npcSoundSource.PlayOneShot(batonSwing);
        
        attackCooldown = intCooldownTime;
        StopMove();

        attacking = true;

        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<PlayerController>().TakeDamage(batonDamage);
        }
        Invoke("AttackComplete", attackTime);
        onCooldown = true;
    }
    protected virtual void Shoot()
    {
        npcSoundSource.PlayOneShot(gunShot);
        Instantiate(bulletTracer, firePoint.position, firePoint.rotation);
        attackCooldown = intCooldownTime;
        StopMove();

        attacking = true;

        
        Invoke("AttackComplete", attackTime);
        onCooldown = true;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    protected void AttackCooldown()
    {
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0 && onCooldown)
        {
            onCooldown = false;
            attackCooldown = intCooldownTime;
        }
    }
    protected void AttackComplete()
    {
        if (attacking == true)
        {
            ChangeAnimationState(HENCHMAN_IDLE);
            attacking = false;
        }
    }
    protected void Move()
    {
        npcScript.enabled = true;
        ChangeAnimationState(HENCHMAN_RUN);
    }

    protected void StopMove()
    {
        npcScript.enabled = false;
        ChangeAnimationState(HENCHMAN_IDLE);
        //npcAnimator.SetBool("Run", false);
    }

    public void TakeDamage(int damage)
    {
        if (health > 0)
        {
            health -= damage;
            npcSoundSource.PlayOneShot(impactSound);
            ChangeAnimationState(HENCHMAN_HURT);
            Invoke("HurtComplete", hurtTime);
            //npcAnimator.SetTrigger("Hurt");
        }

        if (health <= 0)
        {
            isDead = true;
            npcSoundSource.PlayOneShot(deathSound);
            //Level01ObjManager.KilledCount(1);
            //npcAnimator.SetBool("IsDead", isDead);            
            StopMove();
            
        }
    }
    protected void HurtComplete()
    {
        if (isDead == false && health > 0)
        {
            ChangeAnimationState(HENCHMAN_IDLE);
        }
        if(health <= 0 && isDead == true)
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
            StartCoroutine(Die());
        }
    }

    protected IEnumerator Die()
    {
        //npcAnimator.SetTrigger("Die");
        ChangeAnimationState(HENCHMAN_DIE);
        LevelManagerChapter0.enemyAlive--;

        yield return new WaitForSeconds(0.1f);
        this.enabled = false;

        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
    }

    void RaycastDebugger()
    {
        if (distance > attackDistance)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.red);
        }
        else if (distance < attackDistance)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.green);
        }
    }
    protected void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation)
            return;
        npcAnimator.Play(newAnimation);
        currentAnimation = newAnimation;
    }
}
