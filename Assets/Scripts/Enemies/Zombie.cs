using UnityEngine;

public class Zombie : Enemy
{
    [SerializeField] private float aggroRange = 10f;
    private bool isAggro = false;
    
    protected override void Update()
    {
        if (player == null)
            return;
            
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        // Check if player is within aggro range
        if (distanceToPlayer <= aggroRange)
        {
            isAggro = true;
        }
        
        if (isAggro)
        {
            base.Update();
            
            // Rotate to face player
            Vector2 direction = (player.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
} 