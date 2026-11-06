using UnityEngine;

public class GoalDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se quem entrou na área de saída foi o jogador
        if (other.CompareTag("Player"))
        {
            Debug.Log("🏆 Vitória! O jogador alcançou o objetivo.");

            // Procura o UIController na cena para exibir a tela de vitória
            UIController ui = FindObjectOfType<UIController>();
            if (ui != null)
            {
                ui.ShowVictoryScreen();
            }
        }
    }
}