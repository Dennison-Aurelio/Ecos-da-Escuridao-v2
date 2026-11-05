using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    [Header("Configurações de Patrulha")]
    public Transform[] waypoints; // Pontos do setor para o inimigo patrulhar
    private int currentWaypointIndex = 0;
    public float waitTimeAtWaypoint = 2.0f;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    [Header("Configurações de Audição")]
    public float maxHearingDistance = 18.0f; // Distância máxima que o eco alcança
    public LayerMask obstacleMask; // Camada das Paredes

    [Header("Configurações de Investigação")]
    public float searchTimeAtPulse = 3.0f; // Tempo (segundos) que fica procurando no local do eco
    private float searchTimer = 0f;

    private NavMeshAgent agent;
    private bool isInvestigating = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Começa a patrulhar se houver pontos definidos
        if (waypoints != null && waypoints.Length > 0)
        {
            SetNextWaypoint();
        }
    }

    void Update()
    {
        // 1. Se estiver investigando um som que ouviu
        if (isInvestigating)
        {
            // Checa se o inimigo já chegou até o ponto onde o eco foi disparado
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                searchTimer += Time.deltaTime;

                // Espera o tempo configurado (ex: 3 segundos) procurando no local
                if (searchTimer >= searchTimeAtPulse)
                {
                    isInvestigating = false; // Desiste da busca
                    searchTimer = 0f;
                    SetNextWaypoint(); // Retoma a ronda do setor
                    Debug.Log("🔍 Inimigo não encontrou o jogador e voltou a patrulhar.");
                }
            }
        }
        // 2. Se não estiver investigando, mantém a ronda pelo setor
        else if (waypoints != null && waypoints.Length > 0)
        {
            PatrolLogic();
        }
    }

    void PatrolLogic()
    {
        // Verifica se chegou perto do ponto atual da ronda
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!isWaiting)
            {
                isWaiting = true;
                waitTimer = waitTimeAtWaypoint;
            }

            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                SetNextWaypoint();
            }
        }
    }

    void SetNextWaypoint()
    {
        if (waypoints.Length == 0) return;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    // Chamado pelo Player ao emitir o pulso
    public void ListenToPulse(Vector3 soundPosition)
    {
        float distanceToSound = Vector3.Distance(transform.position, soundPosition);

        // 1. Checa se o som está dentro do alcance máximo de audição
        if (distanceToSound <= maxHearingDistance)
        {
            // 2. Checa se há paredes bloqueando o som
            Vector3 directionToSound = (soundPosition - transform.position).normalized;
            if (!Physics.Raycast(transform.position, directionToSound, distanceToSound, obstacleMask))
            {
                // Som não foi bloqueado por paredes! O inimigo vai investigar
                isInvestigating = true;
                isWaiting = false;
                searchTimer = 0f; // Reseta o cronômetro de busca
                agent.SetDestination(soundPosition);
                Debug.Log("⚠️ Inimigo ouviu o eco no setor!");
            }
            else
            {
                Debug.Log("🔊 O som foi abafado pelas paredes.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("💀 O INIMIGO TE PEGOU! REINICIANDO...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}