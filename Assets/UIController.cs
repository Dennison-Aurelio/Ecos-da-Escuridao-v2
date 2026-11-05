using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Referências")]
    public Slider cooldownSlider;
    public PulseCooldown playerCooldown;

    void Update()
    {
        if (cooldownSlider != null && playerCooldown != null)
        {
            // Atualiza a posição da barra com a porcentagem calculada (0.0 a 1.0)
            cooldownSlider.value = playerCooldown.GetCooldownProgress();
        }
    }
}