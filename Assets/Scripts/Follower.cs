using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
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
                _state = value;
                _currentStateTime = 0.0f;
            }
        }
    }

    [SerializeField]
    private State _state;

    [SerializeField]
    private MeshRenderer _meshRenderer;

    [SerializeField]
    private Material _transparentMat;

    [SerializeField] 
    private AudioSource _audioSource;

    [SerializeField] 
    private NavMeshAgent _navAgent;

    private float _currentStateTime;
    private Vector3[] exitPoints;
    private Vector3 destination;

	// Use this for initialization
	void Start ()
	{
	    GameObject exits = GameObject.Find("Exits");
	    if (exits != null && exits.transform.childCount > 0)
	    {
            exitPoints = new Vector3[exits.transform.childCount];
	        for (int i = 0; i < exits.transform.childCount; i++)
	        {
	            exitPoints[i] = exits.transform.GetChild(i).position;
	        }
	    }
	}
	
	// Update is called once per frame
	void Update () {
	    switch (FollowerState)
	    {
	        case State.Leaving:
	            if (_currentStateTime == 0.0f)
	            {
	            }
	            if (_currentStateTime >= 3f && _currentStateTime - Time.deltaTime <= 3f)
	            {
	            }
	            if (_currentStateTime > 10.0f)
	            {
	                FollowerState = State.Dazed;
	                break;
	            }
	            _currentStateTime += Time.deltaTime;
	            break;
            case State.Dead:
                if (_currentStateTime == 0.0f)
                {
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
	}
}
