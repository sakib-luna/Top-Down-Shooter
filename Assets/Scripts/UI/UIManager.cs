using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Health UI")]
    [SerializeField] private Slider healthBar;
    
    [Header("Ammo UI")]
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Slider reloadBar;
    
    [Header("Wave UI")]
    [SerializeField] private TextMeshProUGUI waveText;
    
    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    
    private void Start()
    {
        // Subscribe to events
        PlayerController.OnHealthChanged += UpdateHealthUI;
        WeaponController.OnAmmoChanged += UpdateAmmoUI;
        WeaponController.OnReloading += ShowReloadUI;
        SpawnManager.OnWaveChanged += UpdateWaveUI;
        GameManager.OnScoreChanged += UpdateScoreUI;
        
        // Initialize UI
        if (reloadBar != null)
            reloadBar.gameObject.SetActive(false);
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from events
        PlayerController.OnHealthChanged -= UpdateHealthUI;
        WeaponController.OnAmmoChanged -= UpdateAmmoUI;
        WeaponController.OnReloading -= ShowReloadUI;
        SpawnManager.OnWaveChanged -= UpdateWaveUI;
        GameManager.OnScoreChanged -= UpdateScoreUI;
    }
    
    private void UpdateHealthUI(int current, int max)
    {
        if (healthBar != null)
        {
            healthBar.maxValue = max;
            healthBar.value = current;
        }
    }
    
    private void UpdateAmmoUI(int current, int max)
    {
        if (ammoText != null)
        {
            ammoText.text = $"Ammo: {current}/{max}";
        }
    }
    
    private void ShowReloadUI(float reloadTime)
    {
        if (reloadBar != null)
        {
            StartCoroutine(ReloadBarCoroutine(reloadTime));
        }
    }
    
    private System.Collections.IEnumerator ReloadBarCoroutine(float reloadTime)
    {
        reloadBar.gameObject.SetActive(true);
        reloadBar.maxValue = reloadTime;
        
        float timer = 0f;
        while (timer < reloadTime)
        {
            timer += Time.deltaTime;
            reloadBar.value = timer;
            yield return null;
        }
        
        reloadBar.gameObject.SetActive(false);
    }
    
    private void UpdateWaveUI(int current, int total)
    {
        if (waveText != null)
        {
            waveText.text = $"Wave: {current}/{total}";
        }
    }
    
    private void UpdateScoreUI(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }
} 