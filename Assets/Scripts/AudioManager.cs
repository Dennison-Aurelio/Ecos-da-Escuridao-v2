using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Fontes de Áudio")]
    public AudioSource sfxSource;      // Para efeitos sonoros curtos (pulso, alerta, etc.)
    public AudioSource ambientSource;  // Exclusivo para o som ambiente da fase em loop

    [Header("Som Ambiente")]
    public AudioClip ambientMusic;

    [Header("Efeitos Sonoros (Clips)")]
    public AudioClip pulseSFX;
    public AudioClip enemyAlertSFX;
    public AudioClip gameOverSFX;
    public AudioClip victorySFX;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Inicia o som ambiente automaticamente se o clipe estiver atribuído
        if (ambientMusic != null && ambientSource != null)
        {
            ambientSource.clip = ambientMusic;
            ambientSource.loop = true; // Garante que fica repetindo infinitamente
            ambientSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayPulseSound() => PlaySFX(pulseSFX);
    public void PlayEnemyAlertSound() => PlaySFX(enemyAlertSFX);
    public void PlayGameOverSound() => PlaySFX(gameOverSFX);
    public void PlayVictorySound() => PlaySFX(victorySFX);
}