using UnityEngine;

public class FastZombie : Zombie
{
    protected override void Awake()
    {
        base.Awake();
        
        // Fast zombies have less health but move faster
        maxHealth = 50;
        currentHealth = maxHealth;
        moveSpeed = 5f;
        
        // Update NavMeshAgent speed
        agent.speed = moveSpeed;
    }
} 