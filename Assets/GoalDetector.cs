using UnityEngine;
using UnityEngine.SceneManagement; // Biblioteca para recarregar a cena
using UnityEngine.InputSystem;     // Sistema de entrada da Unity 6
using TMPro;

public class GoalDetector : MonoBehaviour
{
    [Header("Interface de Vitória")]
    public GameObject victoryTextObject;

    private bool gameEnded = false;

    void Update()
    {
        // Se o jogo terminou e o jogador pressionar R, reinicia a fase
        if (gameEnded && Keyboard.current != null)
        {
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null && !gameEnded)
        {
            gameEnded = true;

            Debug.Log("🎉 VOCÊ ENCONTROU A SAÍDA! FASE CONCLUÍDA!");

            // Trava a movimentação do Player
            player.enabled = false;

            // Ativa o texto de vitória na tela
            if (victoryTextObject != null)
            {
                victoryTextObject.SetActive(true);
            }
        }
    }
}