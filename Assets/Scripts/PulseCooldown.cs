using UnityEngine;

public class PulseCooldown : MonoBehaviour
{
    [Header("Configurações de Tempo")]
    public float cooldownTime = 5.0f; // Tempo total de espera em segundos
    private float currentCooldown = 0.0f;

    private UIController uiController;

    void Start()
    {
        // Encontra o UIController na cena
        uiController = FindObjectOfType<UIController>();

        // Garante que o pulso começa 100% pronto/carregado
        currentCooldown = cooldownTime;
        UpdateUI();
    }

    void Update()
    {
        // Se a recarga ainda não terminou, incrementa o tempo
        if (currentCooldown < cooldownTime)
        {
            currentCooldown += Time.deltaTime;

            if (currentCooldown > cooldownTime)
            {
                currentCooldown = cooldownTime;
            }

            UpdateUI();
        }
    }

    // Chamado pelo SoundPulse ao apertar Espaço
    public void ResetCooldown()
    {
        currentCooldown = 0.0f;
        UpdateUI();
    }

    // Verifica se o pulso pode ser usado
    public bool CanUsePulse()
    {
        return currentCooldown >= cooldownTime;
    }

    // Envia o valor atual para o Slider da UI
    private void UpdateUI()
    {
        if (uiController != null)
        {
            uiController.UpdateCooldownBar(currentCooldown, cooldownTime);
        }
    }
}