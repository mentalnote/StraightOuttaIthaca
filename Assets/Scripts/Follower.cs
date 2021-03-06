﻿using System;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class Follower : MonoBehaviour {

    public enum State
    {
        Leaving,
        Dazed,
        Dead,
        Praying,
        Returning,
        Fleeing,
        GettingBlown,
        Wandering,
        Dropping
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
                _stateChanged = true;

                if (_state == State.Dead)
                {
                    AudioSource.PlayClipAtPoint(deathSoundEffect, transform.position);
                }

                _currentStateTime = 0.0f;
            }
        }
    }

    public FaithTracker FaithTracker
    {
        get { return _faithtracker; }
    }

    [SerializeField]
    private State _state;

    [SerializeField]
    private float _faithRenewalRadius = 5.0f;

    [SerializeField]
    private Color _faithfulColor = Color.blue;

    [SerializeField]
    private Color _nonFaithfulColor = Color.red;

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
    private FaithTracker _faithtracker;

    [SerializeField] 
    private Animation _animation;

    [SerializeField]
    private AudioClip praySoundEffect = null;

    [SerializeField]
    private AudioClip deathSoundEffect = null;

    private float _currentStateTime;
    private GameObject[] _exitPoints;
    private Vector3[] _prayPoints;
    private Vector3 _destination;
    private Queue<Vector3> _destinations;
    private bool _hasDestination;
    private State _previousState = State.Leaving;
    private float _speed;
    private GameObject sourceOfFear;
    private Idol _idol;
    private SceneArea _villageArea;
    private SpellButtonLogic _convertButtonLogic;
    private bool _stateChanged;

    public void BlowAwayFrom(Vector3 position, float force, float radius)
    {
        if (FollowerState != State.GettingBlown)
        {
            Vector3 displacement = transform.position - position;
            float displacementMagnitude = displacement.magnitude;

            Vector3 displacementNormalized = new Vector3(displacement.x + Random.Range(-0.5f, 0.5f), 0.0f, displacement.z + Random.Range(-0.5f, 0.5f)).normalized;
            displacementNormalized.y = 10.0f;

            rigidbody.AddForce(displacementNormalized * (force * (1.0f - Mathf.Clamp01(displacementMagnitude / radius))));

            Destroy(_navAgent);
            FollowerState = State.Dead;
        }
    }

    public void Die()
    {
        FollowerState = State.Dead;
    }

    private void EmitFaith()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _faithRenewalRadius, 1 << LayerMask.NameToLayer("Follower"));
        for (int i = 0; i < hits.Length; i++)
        {
            FaithTracker faithTracker = hits[i].GetComponent<FaithTracker>();
            if (faithTracker != null)
            {
                faithTracker.InstillFaith(50.0f);
            }
        }
    }

    private void UpdateColour()
    {
        if (_faithtracker != null && FollowerState != State.Praying)
        {
            Color colour = new Color(
            Mathf.Lerp(_nonFaithfulColor.r, _faithfulColor.r, _faithtracker.Faith / 100f),
            Mathf.Lerp(_nonFaithfulColor.g, _faithfulColor.g, _faithtracker.Faith / 100f),
            Mathf.Lerp(_nonFaithfulColor.b, _faithfulColor.b, _faithtracker.Faith / 100f),
            Mathf.Lerp(_nonFaithfulColor.a, _faithfulColor.a, _faithtracker.Faith / 100f)
            );
            _meshRenderer.material.color = colour;    
        }
    }

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
        if (FollowerState == State.Dropping)
        {
            FollowerState = State.Wandering;
        }
    }

	// Use this for initialization
	void Start ()
	{
        _convertButtonLogic = GameObject.Find("ConvertBtn").GetComponent<SpellButtonLogic>();
	    _animation["Walk"].speed = 2f;
        _animation.wrapMode = WrapMode.Loop;
	    _speed = _navAgent.speed;
	    GameObject exits = GameObject.Find("Exits");
	    if (exits != null && exits.transform.childCount > 0)
	    {
            _exitPoints = new GameObject[exits.transform.childCount];
	        for (int i = 0; i < exits.transform.childCount; i++)
	        {
	            _exitPoints[i] = exits.transform.GetChild(i).gameObject;
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

	    _villageArea = GameObject.Find("VillageArea").GetComponent<SceneArea>();

	    if (_damageScript != null)
	    {
	        _damageScript.addDamageListener((damage, health) =>
	                                        {
	                                            if (health <= 0)
	                                            {
	                                                FollowerState = State.Dead;
	                                            }
	                                        });
	    }
	}
	
	// Update is called once per frame
	void Update() {
        UpdateColour();
        switch (FollowerState)
	    {
	        case State.Leaving:
                _navAgent.speed = _speed;
	            if (_faithtracker != null && _faithtracker.Converted)
	            {
	                _hasDestination = false;
                    FollowerState = State.Returning;
	                break;
	            }
	            if (_currentStateTime == 0.0f)
	            {
                    _animation.Play("Walk");
                    if (!_hasDestination)
                    {
                        if (_exitPoints != null)
                        {
                            GameObject exit = _exitPoints[Random.Range(0, _exitPoints.Length)];
                            Vector3[] vecs = exit.GetComponent<GameObjectArray>().getPositions();
                            _destinations = new Queue<Vector3>();
                            for (int i = 0; i < vecs.Length; i++)
                            {
                                _destinations.Enqueue(vecs[i]);
                            }
                            _navAgent.SetDestination(_destinations.Dequeue());
                            _hasDestination = true;
                        }
                    }
	            }
                if (!_navAgent.pathPending)
                {
                    if (_navAgent.remainingDistance - Math.Abs(transform.position
                        .y - _navAgent.destination.y) <= _navAgent.stoppingDistance + 10.0f)
                    {
                        //if (!_navAgent.hasPath || _navAgent.velocity.sqrMagnitude == 0.0f)
                        //{
                            if (_destinations.Count > 0)
                            {
                                
                                _navAgent.SetDestination(_destinations.Dequeue());
                            }
                            else
                            {
                                GameObject.Find("God").GetComponent<GameOverLogic>().EscapeNum++;
                                Destroy(gameObject);    
                            }
                            //FollowerState = State.Dead;
                        //}
                    }
                }
	            break;
            case State.Wandering:
                if (_faithtracker != null && _faithtracker.Converted)
	            {
	                _hasDestination = false;
                    FollowerState = State.Returning;
                    break;
	            }
                if (_faithtracker != null && _faithtracker.Faithless)
                {
                    _hasDestination = false;
                    FollowerState = State.Leaving;
                    break;
                }
	            if (_currentStateTime == 0.0f)
	            {
                    _animation.Play("Walk");
                    if (!_hasDestination)
                    {
                        if (_exitPoints != null && _villageArea != null)
                        {
                            _destination = _villageArea.GetRandomPos();
                            _navAgent.SetDestination(_destination);
                            _hasDestination = true;
                        }
                    }
	            }
                if (!_navAgent.pathPending)
                {
                    if (_navAgent.remainingDistance - Math.Abs(transform.position
                        .y - _navAgent.destination.y) <= _navAgent.stoppingDistance + 2.0f)
                    {
                        //if (!_navAgent.hasPath || _navAgent.velocity.sqrMagnitude == 0.0f)
                        //{
                        if (_exitPoints != null && _villageArea != null)
                        {
                            _destination = _villageArea.GetRandomPos();
                            _navAgent.SetDestination(_destination);
                            if (Random.Range(0, 2) == 0)
                            {
                                FollowerState = State.Dazed;
                                break;
                            }
                        }
                        else
                        {
                            Destroy(gameObject);
                        }
                        //FollowerState = State.Dead;
                        //}
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
                    break;
	            }
	            break;
            case State.Dropping:
	            if (_currentStateTime == 0.0f)
	            {
	                _animation.CrossFade("Flail");
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
                        break;
	                }    
	            }
	            if (_currentStateTime >= 4.0f)
	            {
                    _hasDestination = false;
                    _navAgent.speed = _speed;
                    FollowerState = _previousState;
                    break;
	            }
	            break;
            case State.Returning:
	            _navAgent.speed = _speed;
                if (_faithtracker != null && _faithtracker.Faithless)
                {
                    _hasDestination = false;
                    FollowerState = State.Leaving;
                    break;
                }
                if (_currentStateTime == 0.0f)
                {
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
                    animation.wrapMode = WrapMode.Loop;
    	            animation.CrossFade("Flail");
                }
            
                if (_currentStateTime > 5.0f && rigidbody.velocity.magnitude < 1.0f)
                {
                    FollowerState = _previousState;
                    break;
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
	                    rigidbody.constraints = RigidbodyConstraints.FreezeAll;
	                    _navAgent.SetDestination(_destination);
	                    _currentStateTime = 0.01f;
	                }
	                else
	                {
	                    FollowerState = _previousState;
                        break;
	                }
	            }
                if (!_navAgent.pathPending && _hasDestination)
                {
                    if (_navAgent.remainingDistance - Math.Abs(transform.position
                        .y - _navAgent.destination.y) <= _navAgent.stoppingDistance + 4.0f)
                    {
                        _animation["PrayStart"].wrapMode = WrapMode.Once;
                        _animation.CrossFade("PrayStart");
                        _animation.PlayQueued("PrayRepeat");
                        _hasDestination = false;
                        _currentStateTime = 10.0f;
                        _navAgent.speed = 0.0f;
                        collider.enabled = false;
						
						AudioSource.PlayClipAtPoint(praySoundEffect, transform.position);
                        //FollowerState = State.Dead;
                        //}
                    }
                }
	            if (!_hasDestination && _currentStateTime <= 11.0f)
	            {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_idol.transform.position - transform.position), (_currentStateTime - 10.0f) / 1.0f);
                }
	            break;
            case State.Dead:
                if (_currentStateTime == 0.0f)
                {
                    _animation.CrossFade("Flail");
                    animation.wrapMode = WrapMode.Once;
                    _audioSource.clip = null;
                    EmitFaith();
                    _convertButtonLogic.TimePassed += 1000;
                    //_audioSource.Play();
                }
                if (_currentStateTime > 4.0f)
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
                if (_currentStateTime > 5.0f)
                {
                    Destroy(gameObject);
                }
                break;
	    }
	    if (_stateChanged)
	    {
	        _stateChanged = false;
	    }
	    else
	    {
            _currentStateTime += Time.deltaTime;
	    }
	}
}
