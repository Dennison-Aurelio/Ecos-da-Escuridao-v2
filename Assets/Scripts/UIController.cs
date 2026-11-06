using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Componentes do Cooldown")]
    public Slider cooldownSlider;

    [Header("Painéis de Fim de Jogo")]
    public GameObject victoryPanel;
    public GameObject gameOverPanel;

    void Start()
    {
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void UpdateCooldownBar(float currentCooldown, float maxCooldown)
    {
        if (cooldownSlider != null)
        {
            cooldownSlider.maxValue = maxCooldown;
            cooldownSlider.value = currentCooldown;
        }
    }

    public void ShowVictoryScreen()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            Time.timeScale = 0f;
        }

        // --- CHAMADA DO ÁUDIO DE VITÓRIA ---
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayVictorySound();
        }
    }

    public void ShowGameOverScreen()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }

        // --- CHAMADA DO ÁUDIO DE GAME OVER ---
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayGameOverSound();
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}