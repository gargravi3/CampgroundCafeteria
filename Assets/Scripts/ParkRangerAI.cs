using UnityEngine;
using UnityEngine.AI;
using CityPeople;

public class ParkRangerAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private CityPeopleScript _rangerModel;
    [SerializeField] private GameObject _bearObject;

    [SerializeField] private Material[] _outfits;

    public enum AI_State
    {
        Idle,
        Wander,
        Alerted,
        Chasing,
        Shooting
    };

    private AI_State _aiState;

    [Header("Idle Properties")]
    private float _idleTime = 0f;

    [Header("Wandering Properties")]
    private int _wanderPosition;
    public GameObject[] wanderPath;
    [SerializeField] private float _walkSpeed;

    [Header("Alerted Properties")]
    [SerializeField] private float _alertDistance;
    [SerializeField] private float _sightDistance;
    [SerializeField] private float _alertedTimer;
    [SerializeField] private float _alertedJoggingSpeed;
    private float _alertTime = 0f;
    private RaycastHit _rayHit;

    [Header("Chasing Properties")]
    [SerializeField] private float _runSpeed;

    [Header("Shooting Properties")]
    [SerializeField] private GameObject _weapon; // will update with weapon script
    [SerializeField] private float _shootDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rangerModel.SetPalette(_outfits[Random.Range(0, _outfits.Length-1)]);

        _aiState = AI_State.Wander;
        _wanderPosition = 0;
        _agent.speed = _walkSpeed;
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

                case AI_State.Alerted:
                    _alertTime = _alertedTimer;
                    break;

                case AI_State.Chasing:
                    _agent.SetDestination(_bearObject.transform.position); // TODO change this to intercept, makes it more balanced
                    _agent.speed = _runSpeed;
                    break;

                case AI_State.Shooting:
                    transform.LookAt(_bearObject.transform);
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
        if(Physics.Raycast(transform.position, transform.forward, out _rayHit, _sightDistance))
        {
            if (_rayHit.collider.gameObject.tag == "Player")
            {
                Debug.DrawRay(transform.position, transform.forward * _shootDistance, Color.red);
                
                // if within shooting distance shoot, if not chase
                if(Vector3.Distance(transform.position, _rayHit.point) < _shootDistance)
                {
                    // Shoot at bear
                    Shoot();
                }
                else
                {
                    if (_aiState != AI_State.Chasing)
                    {
                        _agent.SetDestination(transform.position); // Removes the current path so any state can trigger a chase
                    }

                    _aiState = AI_State.Chasing;   
                }

                _alertTime = _alertedTimer;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * _sightDistance, Color.white);

            if (_aiState != AI_State.Idle)
            {
                _alertTime -= Time.deltaTime;

                if (_alertTime <= 0)
                {
                    WaitAMoment(5.0f, 5.5f);
                }
            }
        }

        // Velocity is in world space so must use the inverse to translate to the ranger's local space
        if (_aiState != AI_State.Shooting)
        {
            _animator.SetFloat("velocityX", transform.InverseTransformVector(_agent.velocity).x);
            _animator.SetFloat("velocityZ", transform.InverseTransformVector(_agent.velocity).z);
        }
    }

    private void WanderAround()
    {
        if(_wanderPosition >= wanderPath.Length)
        {
            _wanderPosition = 0;
        }

        _agent.SetDestination(wanderPath[_wanderPosition++].transform.position);
        WaitAMoment(0.5f, 5.0f);
    }

    private void Shoot()
    {
        _aiState = AI_State.Shooting;
        _agent.SetDestination(transform.position);
        _weapon.SetActive(true);
        _animator.SetBool("isShooting", true);
    }

    private void WaitAMoment(float minTime, float maxTime)
    {
        _idleTime = Random.Range(minTime, maxTime);
        _aiState = AI_State.Idle;
    }

    public void AlertToPosition(Vector3 alertPosition)
    {
        _agent.SetDestination(alertPosition);
        _aiState = AI_State.Alerted;
        _agent.speed = _alertedJoggingSpeed;
    }

    public void FireWeapon()
    {
        _weapon.GetComponent<Tranquilizer>().Fire(_bearObject.transform.position);
    }

    public void WeaponHolstered()
    {
        _animator.SetBool("isShooting", false);
        _weapon.SetActive(false);
        _aiState = AI_State.Chasing;
    }
}
