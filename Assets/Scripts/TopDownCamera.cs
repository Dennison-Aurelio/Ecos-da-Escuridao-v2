using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [Header("Alvo a Ser Seguido")]
    public Transform target;

    [Header("Deslocamento da Câmera (Altura e Distância)")]
    public Vector3 offset = new Vector3(0f, 15f, -2f);

    void LateUpdate()
    {
        if (target != null)
        {
            // Apenas atualiza a posição (mantém a rotação de 90° definida no Transform)
            transform.position = target.position + offset;
        }
    }
}