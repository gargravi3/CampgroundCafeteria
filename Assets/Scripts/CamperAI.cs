using UnityEngine;
using UnityEngine.AI;
using CityPeople;

public class CamperAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private CityPeopleScript _rangerModel;
    [SerializeField] private GameObject _bearObject;
    private AlertBroadcaster _alertBroadcaster;

    [SerializeField] private Material[] _outfits;

    public enum AI_State
    {
        Idle,
        Wander,
        Scared
    };

    private AI_State _aiState;

    [Header("Idle Properties")]
    private float _idleTime = 0f;

    [Header("Wandering Properties")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _wanderDistance;

    [Header("Alerted Properties")]
    [SerializeField] private float _sightDistance;
    [SerializeField] private float _alertedTimer;
    private float _alertTime = 0f;
    private RaycastHit _rayHit;

    [Header("Chasing Properties")]
    private float _agentBaseSpeed;
    [SerializeField] private float _runSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _alertBroadcaster = GameObject.Find("AlertManager").GetComponent<AlertBroadcaster>();
        _rangerModel.SetPalette(_outfits[Random.Range(0, _outfits.Length - 1)]);

        _aiState = AI_State.Wander;
        _agent.speed = _walkSpeed;

        WanderAround();
    }

    // Update is called once per frame
    void Update()
    {
        if (_agent.remainingDistance < 0.1f && !_agent.pathPending)
        {
            switch (_aiState)
            {
                case AI_State.Idle:
                    _idleTime -= Time.deltaTime;
                    if (_idleTime <= 0f)
                    {
                        _aiState = AI_State.Wander;
                        _agent.speed = _walkSpeed;
                    }
                    break;

                case AI_State.Wander:
                    WanderAround();
                    break;

                case AI_State.Scared:
                    // scared
                    break;

                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Using for physics interactions ie collision or raycast
    /// </summary>
    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out _rayHit, _sightDistance))
        {
            if (_rayHit.collider.gameObject.tag == "Player")
            {
                Debug.DrawRay(transform.position, transform.forward * _sightDistance, Color.red);

                if (_aiState != AI_State.Scared)
                {
                    _agent.SetDestination(transform.position); // Removes the current path so any state can trigger a chase
                }

                _aiState = AI_State.Scared;
                _alertBroadcaster.AlertAtLocation(transform.position);
                _alertTime = _alertedTimer;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * _sightDistance, Color.white);

            if (_aiState == AI_State.Scared)
            {
                _alertTime -= Time.deltaTime;

                if (_alertTime <= 0)
                {
                    WanderAround();
                }
            }
        }

        _animator.SetFloat("velocityX", transform.InverseTransformVector(_agent.velocity).x);
        _animator.SetFloat("velocityZ", transform.InverseTransformVector(_agent.velocity).z);
    }

    private void WanderAround()
    {
        Vector3 newPosition = Random.insideUnitCircle * _wanderDistance;
        newPosition += transform.position;

        _agent.SetDestination(newPosition);
        WaitAMoment(0.5f, 2.0f);
    }

    private void WaitAMoment(float minTime, float maxTime)
    {
        _idleTime = Random.Range(minTime, maxTime);
        _aiState = AI_State.Idle;
    }

    public void ExecuteFootstep()
    {
        AudioSource audio = GetComponent<AudioSource>();

        //if(!audio.isPlaying) audio.Play();
    }
}
