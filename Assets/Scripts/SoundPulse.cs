using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoundPulse : MonoBehaviour
{
    [Header("Configurações de Luz")]
    public Light pulseLight;
    public float maxRange = 5f;
    public float maxIntensity = 2f;
    public float pulseDuration = 2f;

    private bool isPulsing = false;
    private PulseCooldown cooldownScript;

    void Start()
    {
        cooldownScript = GetComponent<PulseCooldown>();

        if (pulseLight != null)
        {
            pulseLight.range = 0f;
            pulseLight.intensity = 0f;
        }
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && !isPulsing)
        {
            if (cooldownScript != null && !cooldownScript.CanUsePulse())
            {
                Debug.Log("⏳ Pulso em tempo de recarga!");
                return;
            }

            if (pulseLight != null)
            {
                // --- CHAMADA DO ÁUDIO DO PULSO ---
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.PlayPulseSound();
                }

                StartCoroutine(EmitPulse());

                if (cooldownScript != null)
                {
                    cooldownScript.ResetCooldown();
                }

                EnemyAI[] allEnemies = FindObjectsOfType<EnemyAI>();
                foreach (EnemyAI enemy in allEnemies)
                {
                    if (enemy != null)
                    {
                        enemy.ListenToPulse(transform.position);
                    }
                }
            }
        }
    }

    IEnumerator EmitPulse()
    {
        isPulsing = true;
        float elapsed = 0f;

        if (pulseLight != null)
        {
            pulseLight.enabled = true;
        }

        while (elapsed < pulseDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / pulseDuration;

            if (t <= 0.5f)
            {
                float factor = t * 2f;
                pulseLight.range = Mathf.Lerp(0f, maxRange, factor);
                pulseLight.intensity = Mathf.Lerp(0f, maxIntensity, factor);
            }
            else
            {
                float factor = (t - 0.5f) * 2f;
                pulseLight.range = Mathf.Lerp(maxRange, 0f, factor);
                pulseLight.intensity = Mathf.Lerp(maxIntensity, 0f, factor);
            }

            yield return null;
        }

        if (pulseLight != null)
        {
            pulseLight.range = 0f;
            pulseLight.intensity = 0f;
        }

        isPulsing = false;
    }
}