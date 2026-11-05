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
        // Garante que as telas começam ocultas
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        Time.timeScale = 1f; // Garante que o jogo roda na velocidade normal
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
            Time.timeScale = 0f; // Pausa a física/jogo
        }
    }

    public void ShowGameOverScreen()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f; // Pausa a física/jogo
        }
    }

    // Método chamado pelos botões da UI para reiniciar a partida
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}