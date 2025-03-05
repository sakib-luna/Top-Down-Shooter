using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
    
    private int score = 0;
    
    public delegate void ScoreChanged(int score);
    public static event ScoreChanged OnScoreChanged;
    
    private void Start()
    {
        Time.timeScale = 1f;
        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
            
        if (victoryPanel != null)
            victoryPanel.SetActive(false);
            
        PlayerController.OnPlayerDeath += GameOver;
        Enemy.OnEnemyDeath += AddScore;
        SpawnManager.OnGameWon += Victory;
    }
    
    private void OnDestroy()
    {
        PlayerController.OnPlayerDeath -= GameOver;
        Enemy.OnEnemyDeath -= AddScore;
        SpawnManager.OnGameWon -= Victory;
    }
    
    private void AddScore(int points)
    {
        score += points;
        
        if (OnScoreChanged != null)
            OnScoreChanged(score);
    }
    
    private void GameOver()
    {
        Time.timeScale = 0f;
        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }
    
    private void Victory()
    {
        Time.timeScale = 0f;
        
        if (victoryPanel != null)
            victoryPanel.SetActive(true);
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene(0); // Assuming main menu is scene 0
    }
} 