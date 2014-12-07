using System.Configuration;
using TreeEditor;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class Follower : MonoBehaviour {

    public enum State
    {
        Leaving,
        Dazed,
        Dead,
        Praying,
        Returning,
        Fleeing,
        GettingBlown
    }

    public State FollowerState
    {
        get { return _state; }
        set
        {
            if (_state != value)
            {
                _previousState = _state;
                _state = value;
                _currentStateTime = 0.0f;
            }
        }
    }

    [SerializeField]
    private State _state;

    [SerializeField]
    private SkinnedMeshRenderer _meshRenderer;

    [SerializeField]
    private Material _transparentMat;

    [SerializeField] 
    private AudioSource _audioSource;

    [SerializeField] 
    private NavMeshAgent _navAgent;

    [SerializeField] 
    private DamageScript _damageScript;

    [SerializeField] 
    private Animation _animation;

    private float _currentStateTime;
    private Vector3[] _exitPoints;
    private Vector3[] _prayPoints;
    private Vector3 _destination;
    private bool _hasDestination;
    private State _previousState = State.Leaving;
    private float _speed;
    private GameObject sourceOfFear;
    private Idol _idol;


    public void SetIdol(Idol idolScript)
    {
        _idol = idolScript;
        FollowerState = State.Praying;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (FollowerState == State.GettingBlown && _damageScript != null)
        {
            _damageScript.applyDamage(DamageScript.DamageType.PHYSICAL, Mathf.Clamp(rigidbody.velocity.magnitude * 20.0f, 0.0f, 80.0f));
            FollowerState = _previousState;
        }
    }

	// Use this for initialization
	void Start ()
	{
        _animation.wrapMode = WrapMode.Loop;
	    _speed = _navAgent.speed;
	    GameObject exits = GameObject.Find("Exits");
	    if (exits != null && exits.transform.childCount > 0)
	    {
            _exitPoints = new Vector3[exits.transform.childCount];
	        for (int i = 0; i < exits.transform.childCount; i++)
	        {
	            _exitPoints[i] = exits.transform.GetChild(i).position;
	        }
	    }

        GameObject prayZones = GameObject.Find("PrayZones");
        if (prayZones != null && prayZones.transform.childCount > 0)
        {
            _prayPoints = new Vector3[prayZones.transform.childCount];
            for (int i = 0; i < prayZones.transform.childCount; i++)
            {
                _prayPoints[i] = prayZones.transform.GetChild(i).position;
            }
        }

	    if (_damageScript != null)
	    {
	        _damageScript.addDamageListener((damage, health) =>
	                                        {
	                                            if (health <= 0)
	                                            {
	                                                _state = State.Dead;
	                                            }
	                                        });
	    }
	}
	
	// Update is called once per frame
	void Update () {
	    switch (FollowerState)
	    {
	        case State.Leaving:
	            if (_currentStateTime == 0.0f)
	            {
                    _animation.CrossFade("Walk");
                    if (!_hasDestination)
                    {
                        if (_exitPoints != null)
                        {
                            _destination = _exitPoints[Random.Range(0, _exitPoints.Length)];
                            _navAgent.SetDestination(_destination);
                            _hasDestination = true;
                            
                        }
                    }
	            }
                if (!_navAgent.pathPending)
                {
                    if (_navAgent.remainingDistance <= _navAgent.stoppingDistance)
                    {
                        if (!_navAgent.hasPath || _navAgent.velocity.sqrMagnitude == 0.0f)
                        {
                            FollowerState = State.Dead;
                        }
                    }
                }
	            break;
            case State.Dazed:
	            if (_currentStateTime == 0.0f)
	            {
	                _navAgent.speed = 0.0f;
	            }
	            if (_currentStateTime >= 4.0f)
	            {
	                _navAgent.speed = _speed;
                    FollowerState = _previousState;
	            }
	            break;
            case State.Fleeing:
	            if (_currentStateTime == 0.0f)
	            {
                    _animation.CrossFade("Run");
	                if (sourceOfFear != null)
	                {
	                    Vector3 runDirection = sourceOfFear.transform.position - transform.position;
	                    _navAgent.SetDestination(transform.position + (runDirection.normalized * 50));
	                    _navAgent.speed = _speed*1.5f;
	                    _hasDestination = true;
	                }
	                else
	                {
                        FollowerState = _previousState;
	                }    
	            }
	            if (_currentStateTime >= 4.0f)
	            {
                    _hasDestination = false;
                    _navAgent.speed = _speed;
                    FollowerState = _previousState;
	            }
	            break;
            case State.Returning:
                if (_currentStateTime == 0.0f)
	            {
                    print("go");
                    _animation.CrossFade("Walk");
                    if (!_hasDestination)
                    {
                        if (_prayPoints != null)
                        {
                            _destination = _prayPoints[Random.Range(0, _prayPoints.Length)];
                            _navAgent.SetDestination(_destination);
                            _hasDestination = true;
                        }
                    }
	            }
                break;
            case State.GettingBlown:
	            if (_currentStateTime == 0.0f)
	            {
	                //animation
	            }
	            break;
            case State.Praying:
                if (_currentStateTime == 0.0f)
                {
                    _animation.CrossFade("Walk");
	                if (_idol != null)
	                {
	                    _hasDestination = true;
	                    _destination = _idol.ClaimPositionOfPraise();
	                    _navAgent.SetDestination(_destination);
	                }
	                else
	                {
	                    FollowerState = _previousState;
	                }
	            }
                if (!_navAgent.pathPending)
                {
                    if (_navAgent.remainingDistance <= _navAgent.stoppingDistance)
                    {
                        if (!_navAgent.hasPath || _navAgent.velocity.sqrMagnitude == 0.0f)
                        {
                            _animation.CrossFade("StartPray");
                            _animation.PlayQueued("PrayRepeat");
                        }
                    }
                }
	            break;
            case State.Dead:
                if (_currentStateTime == 0.0f)
                {
                    Destroy(gameObject);
                    _audioSource.clip = null;
                    //_audioSource.Play();
                }
                if (_currentStateTime > 8.0f)
                {
                    if (_transparentMat != null)
                    {
                        _meshRenderer.material = _transparentMat;
                        _transparentMat = null;
                    }
                    Color colour = _meshRenderer.renderer.material.color;
                    colour.a -= 0.5f * Time.deltaTime;
                    _meshRenderer.renderer.material.color = colour;
                }
                if (_currentStateTime > 10.0f)
                {
                    Destroy(gameObject);
                }
                break;
	    }
        _currentStateTime += Time.deltaTime;
	}
}
