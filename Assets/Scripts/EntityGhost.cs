using UnityEngine;
using UnityEngine.AI; // 必須引用 AI 命名空間

[RequireComponent(typeof(NavMeshAgent))]
public class EntityGhost : MonoBehaviour
{
    [Header("Parameters")]
    public float patrolSpeed = 2.0f;
    public float chaseSpeed = 4.5f; 
    public float detectionRadius = 10f; 
    public float fieldOfViewAngle = 110f;

    [Header("Patrol Parameters")]
    public float wanderRadius = 15f; 
    public float waitTimeAtPoint = 2f;

    private enum GhostState { Deactivated, Patrol, Chase }
    [SerializeField]
    private GhostState currentState = GhostState.Deactivated;

    private NavMeshAgent agent;
    private float timer;

    private Collider _collider;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        timer = waitTimeAtPoint; // 初始計時器
        _collider = GetComponent<CapsuleCollider>();
    }


    public void Activate()
    {
        currentState = GhostState.Patrol;
    }

    public void SetPosition(Vector3 position)
    {
        if (agent != null)
        {
            agent.Warp(position);
        }
    }
    
    void Update()
    {
        switch (currentState)
        {
            case GhostState.Patrol:
                PatrolBehavior();
                CheckForPlayer();
                break;

            case GhostState.Chase:
                ChaseBehavior();
                CheckIfLostPlayer();
                break;
        }
    }


    void PatrolBehavior()
    {
        agent.speed = patrolSpeed;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            timer += Time.deltaTime;

            if (timer >= waitTimeAtPoint)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
            }
        }
    }

    void ChaseBehavior()
    {
        agent.speed = chaseSpeed;
        
        if (GameManager.Instance.GetPlayer() != null)
        {
            agent.SetDestination(GameManager.Instance.GetPlayer().gameObject.transform.position);
        }
    }


    void CheckForPlayer()
    {
        
        var playerTarget =  GameManager.Instance.GetPlayer();
        var playerPosition = playerTarget.transform.position;
        
        if (playerTarget == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);

        if (distanceToPlayer < detectionRadius)
        {
            currentState = GhostState.Chase;
        }
    }

    void CheckIfLostPlayer()
    {       
        var playerTarget =  GameManager.Instance.GetPlayer();
        var playerPosition = playerTarget.transform.position;
        
        if (playerTarget == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);

        if (distanceToPlayer > detectionRadius * 1.2f)
        {
            currentState = GhostState.Patrol;
        }
    }
    
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
    
    
    /**
     *
     *  GIZMOS
     * 
     */


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Vector3 viewAngleA = DirFromAngle(-fieldOfViewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(fieldOfViewAngle / 2, false);

        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * detectionRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * detectionRadius);
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}