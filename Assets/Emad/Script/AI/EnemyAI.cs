using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class EnemyAI : MonoBehaviour
{
 
    public Transform player;
    public IWeapon weapon;
    public Health health;

    private CharacterController controller;

   
    public float moveSpeed = 3f;
    public float sprintSpeed = 6f;

  
    public float attackRange = 5f;

    
    public float evadeRadius = 15f;
    public float waitAfterEvade = 3f;

    private AIState currentState;
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
    // State change 
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

   
    // Chasing the flipping player
   
    void HandleChase()
    {
        Vector3 direction = (player.position - transform.position);

        //movement directio
        Vector3 moveDir = direction.normalized;

    //movement step
        controller.Move(moveDir * moveSpeed * Time.deltaTime);

        RotateTowards(player.position);

        if (direction.magnitude <= attackRange)
        {
            currentState = AIState.Attack;
        }
    }

     //Attack the flipping player
    void HandleAttack()
    {
        Vector3 direction = (player.position - transform.position);

        RotateTowards(player.position);

        if (direction.magnitude > attackRange)
        {
            currentState = AIState.Chase;
            return;
        }

        // Ai Fire weapon 
        weapon.OnFireStarted();
    }


    // Evade the flipping player

    void HandleEvade()
    {
        Vector3 direction = (evadeTarget - transform.position);

        if (direction.magnitude < 1f)
            return;

        Vector3 moveDir = direction.normalized;

        controller.Move(moveDir * sprintSpeed * Time.deltaTime);
        RotateTowards(evadeTarget);
    }


    //On hit  start evading

    void OnHit(float dmg)
    {
        StopAllCoroutines();
        StartCoroutine(EvadeRoutine());
    }

    IEnumerator EvadeRoutine()
    {
        currentState = AIState.Evade;

        // Bolt to random location on map
        Vector2 randomCircle = Random.insideUnitCircle * evadeRadius;
        evadeTarget = new Vector3(randomCircle.x, 0, randomCircle.y);

        // Keep bolting until we reach the target
        while (Vector3.Distance(transform.position, evadeTarget) > 1f)
        {
            yield return null;
        }

        // Stop firing (bullets are expensive)
        weapon.OnFireCanceled();

        // Wait 3 seconds
        yield return new WaitForSeconds(waitAfterEvade);

        // GET THE FLIPPING PLAYER AGAIN
        currentState = AIState.Chase;
    }


    // Look at me ( im the captain now )

    void RotateTowards(Vector3 target)
    {
        Vector3 lookDir = (target - transform.position).normalized;
        lookDir.y = 0;

        if (lookDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
        }
    }
}