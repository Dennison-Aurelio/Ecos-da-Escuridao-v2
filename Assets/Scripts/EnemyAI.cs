using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Configurações do Inimigo")]
    public Transform player;
    public Transform[] waypoints;
    public float hearingRange = 18.0f; // Ajustado para responder à distância

    [Header("Tempos de Espera (Segundos)")]
    public float waypointWaitTime = 3.0f;
    public float searchWaitTime = 3.0f;

    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private bool isInvestigating = false;
    private bool isWaiting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        GoToNextWaypoint();
    }

    void Update()
    {
        if (agent == null || isWaiting) return;

        // 1. INVESTIGAÇÃO DE SOM
        if (isInvestigating)
        {
            if (!agent.pathPending && agent.remainingDistance <= 0.6f)
            {
                StartCoroutine(SearchRoutine());
            }
        }
        // 2. PATRULHA NORMAL
        else
        {
            if (!agent.pathPending && agent.remainingDistance <= 0.6f)
            {
                StartCoroutine(WaitAtWaypointRoutine());
            }
        }
    }

    // Chamado pelo SoundPulse (Escuta por Raio Limite Direto)
    [Header("Detecção de Obstáculos")]
    public LayerMask obstacleMask; // Selecione a Layer das Paredes/Cenário no Inspector

    // Chamado pelo SoundPulse ao emitir a onda de som
    public void ListenToPulse(Vector3 pulsePosition)
    {
        float distanceToPulse = Vector3.Distance(transform.position, pulsePosition);

        // 1. Checa se o pulso ocorreu dentro do raio de audição
        if (distanceToPulse <= hearingRange)
        {
            Vector3 directionToPulse = (pulsePosition - transform.position).normalized;

            // Inicia o raio ligeiramente à frente do inimigo para não colidir com a própria cápsula dele
            Vector3 rayOrigin = transform.position + (directionToPulse * 0.5f);
            float rayDistance = distanceToPulse - 0.5f;

            // 2. Dispara um raio em direção à origem do pulso para ver se bate em alguma parede
            if (!Physics.Raycast(rayOrigin, directionToPulse, out RaycastHit hit, rayDistance, obstacleMask))
            {
                // Nenhum obstáculo no caminho: o inimigo escuta o som!
                StopAllCoroutines();
                isWaiting = false;
                isInvestigating = true;

                if (agent != null)
                {
                    agent.SetDestination(pulsePosition);
                }
            }
            else
            {
                Debug.Log($"🔊 O som do pulso foi bloqueado pela parede/obstáculo: {hit.collider.name}");
            }
        }
    }

    IEnumerator WaitAtWaypointRoutine()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waypointWaitTime); // Pausa de 3s no waypoint

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        GoToNextWaypoint();
        isWaiting = false;
    }

    IEnumerator SearchRoutine()
    {
        isWaiting = true;
        yield return new WaitForSeconds(searchWaitTime); // Procura por 3s no local do som

        isInvestigating = false;
        GoToNextWaypoint();
        isWaiting = false;
    }

    void GoToNextWaypoint()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    // --- DETECÇÃO DE GAME OVER (MANTIDA DO DIA 4) ---
    private void OnTriggerEnter(Collider other)
    {
        CheckGameOver(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckGameOver(collision.gameObject);
    }

    private void CheckGameOver(GameObject target)
    {
        if (target.CompareTag("Player"))
        {
            Debug.Log("💀 Game Over! O inimigo pegou o jogador.");

            UIController ui = FindObjectOfType<UIController>();
            if (ui != null)
            {
                ui.ShowGameOverScreen();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hearingRange);
    }
}