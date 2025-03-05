using UnityEngine;

public class ArmoredZombie : Zombie
{
    [SerializeField] private int armorValue = 50;
    
    protected override void Awake()
    {
        base.Awake();
        
        // Armored zombies have more health but move slower
        maxHealth = 200;
        currentHealth = maxHealth;
        moveSpeed = 2f;
        
        // Update NavMeshAgent speed
        agent.speed = moveSpeed;
    }
    
    public override void TakeDamage(int damage)
    {
        // Reduce damage by armor value
        int reducedDamage = Mathf.Max(1, damage - armorValue);
        base.TakeDamage(reducedDamage);
    }
} 