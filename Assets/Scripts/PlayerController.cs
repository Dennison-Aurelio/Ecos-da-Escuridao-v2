using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float fixedYPosition = 1.0f; // Mantém flexível no Inspector

    private CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        float x = 0f;
        float z = 0f;

        // Leitura direta do teclado (WASD + Setas)
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) z += 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) z -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) x += 1f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) x -= 1f;

        // Normalização do vetor (evita andar mais rápido na diagonal)
        Vector3 moveInput = new Vector3(x, 0f, z).normalized;

        if (moveInput.sqrMagnitude > 0.01f)
        {
            controller.Move(moveInput * moveSpeed * Time.deltaTime);
        }

        // Correção de altura segura (apenas se houver desvio acidental de física)
        if (!Mathf.Approximately(transform.position.y, fixedYPosition))
        {
            Vector3 currentPos = transform.position;
            transform.position = new Vector3(currentPos.x, fixedYPosition, currentPos.z);
        }
    }
}