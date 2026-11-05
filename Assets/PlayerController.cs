using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float moveSpeed = 5.0f;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = gameObject.AddComponent<CharacterController>();
        }
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        float x = 0f;
        float z = 0f;

        // Leitura direta das teclas (Top-Down puro)
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) z += 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) z -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) x += 1f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) x -= 1f;

        // Vetor de movimento relativo à tela (X = Direita/Esquerda, Z = Cima/Baixo)
        Vector3 move = new Vector3(x, 0f, z).normalized;

        if (move.magnitude >= 0.1f)
        {
            // Aplica o movimento travado no chão (sem alterar Y)
            controller.Move(move * moveSpeed * Time.deltaTime);
        }

        // Garante que o Player não suba/pule (trava o Y em relação ao chão)
        Vector3 pos = transform.position;
        if (pos.y != 1f) // Mantém a altura fixa do Player
        {
            transform.position = new Vector3(pos.x, 1f, pos.z);
        }
    }
}