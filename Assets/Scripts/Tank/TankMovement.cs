using UnityEngine;
using UnityEngine.AI;

public class TankMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;         
    public float m_Speed = 12f;            
    public float m_TurnSpeed = 180f;       
    public AudioSource m_MovementAudio;    
    public AudioClip m_EngineIdling;       
    public AudioClip m_EngineDriving;      
    public float m_PitchRange = 0.2f;

    [Space(5)]
    [Header("AI Settings")]
    [Range(1.0f, 15.0f)] public float wanderRadius = 10;
    [Range(1.0f, 15.0f)] public float wanderDistance = 10;
    [Range(1.0f, 5.0f)] public float wanderJitter = 0.5f;
    private Collider floor;
    NavMeshAgent agent;
    [HideInInspector] public GameObject target;
    [HideInInspector] public GameObject patrolTarget;

    private Rigidbody m_Rigidbody;         
    private float m_OriginalPitch;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void OnEnable ()
    {
        m_Rigidbody.isKinematic = false;
    }


    private void OnDisable ()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        m_OriginalPitch = m_MovementAudio.pitch;
        agent = this.GetComponent<NavMeshAgent>();
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Ground");
        foreach (GameObject c in objects)
        {
            if (c.GetComponent<MeshCollider>())
            {
                floor = c.GetComponent<MeshCollider>();
                break;
            }
        }
        patrolTarget = GameObject.FindGameObjectWithTag("Patroller");
    }

    private void Update()
    {
        EngineAudio();
    }
    private void EngineAudio()
    {
        if(agent.velocity.magnitude >= 0.0f)
		{
            if (m_MovementAudio.clip == m_EngineDriving)
			{
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
			}
		}
        else
		{
            if (m_MovementAudio.clip == m_EngineIdling)
			{
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }

    Vector3 wanderTarget = Vector3.zero;
    public void Wander()
    {
        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
                                        0,
                                        Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);

        if (!floor.bounds.Contains(targetWorld))
        {
            targetWorld = -transform.position * 0.1f;

        };

        Seek(targetWorld);
    }

    public void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    public void Pursue(GameObject target)
    {
        Vector3 targetDir = target.transform.position - this.transform.position;

        float relativeHeading = Vector3.Angle(this.transform.forward, this.transform.TransformVector(target.transform.forward));

        float toTarget = Vector3.Angle(this.transform.forward, this.transform.TransformVector(targetDir));

        //        if ((toTarget > 90 && relativeHeading < 20) || ds.currentSpeed < 0.01f)
        if ((toTarget > 90 && relativeHeading < 20) || targetDir.magnitude >= 60.0f)
        {
            Seek(target.transform.position);
            return;
        }

        //        float lookAhead = targetDir.magnitude / (agent.speed + ds.currentSpeed);
        float lookAhead = targetDir.magnitude / (agent.speed);
        Seek(target.transform.position + target.transform.forward * lookAhead);
    }
}