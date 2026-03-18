using UnityEngine;

using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class MeleeAI : MonoBehaviour
{
    public enum AIState { Chase, Attack, Evade }


    public Transform player;
    public Health health;

    private CharacterController controller;


    public float moveSpeed = 3f;
    public float sprintSpeed = 6f;


    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public float damage = 20f;


    public float evadeRadius = 15f;
    public float waitAfterEvade = 3f;

    private AIState currentState;
    private float lastAttackTime;
    private Vector3 evadeTarget;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        health.OnDamageTaken += OnHit;
    }

    private void OnDisable()
    {
        health.OnDamageTaken -= OnHit;
    }

    private void Update()
    {
        switch (currentState)
        {
            case AIState.Chase:
                HandleChase();
                break;

            case AIState.Attack:
                HandleAttack();
                break;

            case AIState.Evade:
                HandleEvade();
                break;
        }
    }

 
    // After him, chase him down and kill him
    void HandleChase()
    {
        Vector3 direction = player.position - transform.position;

   
        Vector3 moveDir = direction.normalized;


        controller.Move(moveDir * moveSpeed * Time.deltaTime);

        RotateTowards(player.position);

        if (direction.magnitude <= attackRange)
        {
            currentState = AIState.Attack;
        }
    }

   
    // Obliterate the player until he dies

    void HandleAttack()
    {
        Vector3 direction = player.position - transform.position;

        RotateTowards(player.position);

        if (direction.magnitude > attackRange)
        {
            currentState = AIState.Chase;
            return;
        }

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            PerformAttack();
            lastAttackTime = Time.time;
        }
    }

    void PerformAttack()
    {
        // hit check
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up;

        if (Physics.Raycast(origin, transform.forward, out hit, attackRange))
        {
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }
    }


    // Evade the flipping player, for melee


    void HandleEvade()
    {
        Vector3 direction = evadeTarget - transform.position;

        if (direction.magnitude < 1f)
            return;

        Vector3 moveDir = direction.normalized;

        controller.Move(moveDir * sprintSpeed * Time.deltaTime);
        RotateTowards(evadeTarget);
    }

    void OnHit(float dmg)
    {
        StopAllCoroutines();
        StartCoroutine(EvadeRoutine());
    }

    IEnumerator EvadeRoutine()
    {
        currentState = AIState.Evade;

        // Bolt to a random position on map 
        Vector2 random = Random.insideUnitCircle * evadeRadius;
        evadeTarget = new Vector3(random.x, 0, random.y);

        // keep bolting until reached
        while (Vector3.Distance(transform.position, evadeTarget) > 1f)
        {
            yield return null;
        }

        // breathe ( because we are tired after bolting )
        yield return new WaitForSeconds(waitAfterEvade);

        currentState = AIState.Chase;
    }

    // Rotate towards the player, for melee
    void RotateTowards(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        dir.y = 0;

        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
        }
    }
}