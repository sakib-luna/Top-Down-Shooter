using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected float moveSpeed = 3f;
    [SerializeField] protected int damage = 10;
    [SerializeField] protected float attackRate = 1f;
    [SerializeField] protected int pointsValue = 10;
    
    protected int currentHealth;
    protected Transform player;
    protected NavMeshAgent agent;
    protected float nextAttackTime = 0f;
    
    public delegate void EnemyDeath(int points);
    public static event EnemyDeath OnEnemyDeath;
    
    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        
        // Configure NavMeshAgent for 2D
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;
        
        // Additional NavMeshAgent settings
        agent.avoidancePriority = Random.Range(1, 100); // Randomize priority to avoid crowding
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance; // For better performance
        agent.radius = GetComponent<Collider2D>().bounds.extents.x; // Match collider size
    }
    
    protected virtual void Update()
    {
        if (player == null)
            return;
            
        if (agent.isOnNavMesh)
        {
            // Check if the destination is on the NavMesh
            NavMeshHit hit;
            // Try to find the nearest valid position on the NavMesh within 5 units of the player
            // Parameters:
            // - player.position: The target position to check
            // - hit: Stores the result of the sampling
            // - 5f: Maximum distance to check for a valid position
            // - NavMesh.AllAreas: Check all NavMesh areas (walkable surfaces)
            if (NavMesh.SamplePosition(player.position, out hit, 5f, NavMesh.AllAreas))
            {
                // If a valid position was found, set it as the agent's destination
                // This ensures the enemy only moves to valid locations on the NavMesh
                agent.SetDestination(hit.position);
            }
        }
        else
        {
            // Direct movement if NavMesh is not available
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            
            // Rotate to face player
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackRate;
            Attack(collision.gameObject);
        }
    }
    
    protected virtual void Attack(GameObject target)
    {
        PlayerController playerController = target.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(damage);
        }
    }
    
    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    protected virtual void Die()
    {
        if (OnEnemyDeath != null)
            OnEnemyDeath(pointsValue);
            
        Destroy(gameObject);
    }
} 