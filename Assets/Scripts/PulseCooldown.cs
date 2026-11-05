using UnityEngine;

public class PulseCooldown : MonoBehaviour
{
    [Header("Configurações do Cooldown")]
    public float cooldownTime = 3.0f; // Tempo total de espera em segundos

    private float nextPulseTime = 0.0f; // Momento (segundo exato) em que estará liberado

    // Checa se o pulso pode ser usado
    public bool CanUsePulse()
    {
        return Time.time >= nextPulseTime;
    }

    // Reinicia o tempo de espera ao disparar o pulso
    public void ResetCooldown()
    {
        nextPulseTime = Time.time + cooldownTime;
    }

    // Retorna a porcentagem de recarga de 0 a 1 (para atualizar a barra na UI)
    public float GetCooldownProgress()
    {
        if (CanUsePulse()) return 1.0f; // Se já passou o tempo, a barra está 100% cheia

        float timeRemaining = nextPulseTime - Time.time;
        return 1.0f - (timeRemaining / cooldownTime);
    }
}