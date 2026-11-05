using UnityEngine;

public class GoalDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se quem entrou na zona de saída é o Player
        if (other.CompareTag("Player"))
        {
            Debug.Log("🏆 Vitória! O jogador alcançou a saída.");

            // Chama o gerenciador de UI para exibir a tela de vitória
            UIController ui = FindObjectOfType<UIController>();
            if (ui != null)
            {
                ui.ShowVictoryScreen();
            }
        }
    }
}