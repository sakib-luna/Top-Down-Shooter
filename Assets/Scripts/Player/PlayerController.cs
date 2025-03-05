using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 mousePosition;
    
    public delegate void PlayerHealthChanged(int currentHealth, int maxHealth);
    public static event PlayerHealthChanged OnHealthChanged;
    
    public delegate void PlayerDeath();
    public static event PlayerDeath OnPlayerDeath;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }
    
    private void Start()
    {
        if (OnHealthChanged != null)
            OnHealthChanged(currentHealth, maxHealth);
    }
    
    private void Update()
    {
        // Get input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;
        
        // Get mouse position in world space
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // Rotate player to face mouse
        Vector2 lookDirection = mousePosition - rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
    
    private void FixedUpdate()
    {
        // Move player
        rb.velocity = moveDirection * moveSpeed;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (OnHealthChanged != null)
            OnHealthChanged(currentHealth, maxHealth);
            
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        if (OnPlayerDeath != null)
            OnPlayerDeath();
            
        // Disable player controls
        this.enabled = false;
        rb.velocity = Vector2.zero;
    }
    
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
} 