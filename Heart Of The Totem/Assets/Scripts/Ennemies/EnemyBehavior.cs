using UnityEngine;

public class EnemyBehavior : EnemyStats
{
    private Transform totem;
    private float attackCooldown;
    private Rigidbody rb;
    private bool isFrozen = false;
    private float freezeDuration = 0.5f;
    private float freezeTimer = 0f;
    private Animator animator;

    private float attackAnimationDuration;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        if (animator != null)
        {
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == "Attack")
                {
                    attackAnimationDuration = clip.length;
                    break;
                }
            }
        }

        if (attackAnimationDuration > 0)
        {
            attackSpeed = attackAnimationDuration;
        }

        attackCooldown = attackSpeed;
        attackRange *= Parameters.objectScale.magnitude;
        FindTotem();
    }

    void Update()
    {
        if (totem == null)
        {
            FindTotem();
            return;
        }

        if (isFrozen)
        {
            freezeTimer -= Time.deltaTime;
            if (freezeTimer <= 0f)
            {
                isFrozen = false;
                KeepGoing();
            }
            return;
        }

        float distanceToTotem = Vector3.Distance(transform.position, totem.position);

        if (distanceToTotem <= attackRange)
        {
            AttackTotem();
            StopEnemy();
        }
    }

    void AttackTotem()
    {
        attackCooldown -= Time.deltaTime;

        if (attackCooldown <= 0f)
        {
            attackCooldown = attackSpeed;

            TotemBehavior totemBehavior = totem.GetComponent<TotemBehavior>();
            if (totemBehavior != null)
            {       
                if (animator != null)
                {
                    animator.SetBool("isAttacking", true);
                }

                totemBehavior.TakeDamage(damage);
            }
        }
        else if (animator != null)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    public void StopEnemy()
    {
        MoveTowardsTarget mv = GetComponent<MoveTowardsTarget>();
        if (mv != null)
        {
            mv.enabled = false;
        }

        if (animator != null)
        {
            animator.SetBool("isRunning", false);
        }
    }

    public void KeepGoing()
    {
        MoveTowardsTarget mv = GetComponent<MoveTowardsTarget>();
        if (mv != null)
        {
            mv.enabled = true;
        }
    }

    public void FindTotem()
    {
        GameObject totemObject = GameObject.FindGameObjectWithTag("Totem");

        if (totemObject != null)
        {
            totem = totemObject.transform;
        }
    }

    public void Freeze()
    {
        isFrozen = true;
        freezeTimer = freezeDuration;
        StopEnemy();
    }
}
