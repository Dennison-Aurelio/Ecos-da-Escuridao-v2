using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Configurações do Inimigo")]
    public Transform player;
    public Transform[] waypoints;
    public float hearingRange = 18.0f;

    [Header("Tempos de Espera (Segundos)")]
    public float waypointWaitTime = 3.0f;
    public float searchWaitTime = 3.0f;

    [Header("Detecção de Obstáculos")]
    public LayerMask obstacleMask;

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

        if (isInvestigating)
        {
            if (!agent.pathPending && agent.remainingDistance <= 0.6f)
            {
                StartCoroutine(SearchRoutine());
            }
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance <= 0.6f)
            {
                StartCoroutine(WaitAtWaypointRoutine());
            }
        }
    }

    public void ListenToPulse(Vector3 pulsePosition)
    {
        float distanceToPulse = Vector3.Distance(transform.position, pulsePosition);

        if (distanceToPulse <= hearingRange)
        {
            Vector3 directionToPulse = (pulsePosition - transform.position).normalized;
            Vector3 rayOrigin = transform.position + (directionToPulse * 0.5f);
            float rayDistance = distanceToPulse - 0.5f;

            if (!Physics.Raycast(rayOrigin, directionToPulse, out RaycastHit hit, rayDistance, obstacleMask))
            {
                // --- CHAMADA DO ÁUDIO DE ALERTA DO INIMIGO ---
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.PlayEnemyAlertSound();
                }

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
        yield return new WaitForSeconds(waypointWaitTime);

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        GoToNextWaypoint();
        isWaiting = false;
    }

    IEnumerator SearchRoutine()
    {
        isWaiting = true;
        yield return new WaitForSeconds(searchWaitTime);

        isInvestigating = false;
        GoToNextWaypoint();
        isWaiting = false;
    }

    void GoToNextWaypoint()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

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