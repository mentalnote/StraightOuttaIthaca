using UnityEngine;
using System.Collections;

public class FaithTracker : MonoBehaviour
{

    public bool Converted
    {
        get { return _faithfulThreshold <= _faith; }    
    }

    public bool Faithless
    {
        get { return _escapeThreshold >= _faith; }
    }

    public float Faith
    {
        get { return _faith; }
        set
        { 
            _faith = value;

            if (_faith > 100.0f)
            {
                _faith = 100.0f;
            }
        }
    }

    [SerializeField] 
    private float _startFaith = 60.0f;

    [SerializeField] 
    private float _minLossPerSecond = 1.0f;
    
    [SerializeField] 
    private float _maxLossPerSecond = 3.0f;

    [SerializeField] 
    private float _escapeThreshold = 50.0f;

    [SerializeField] 
    private float _faithfulThreshold = 70.0f;

    private float _faith;
    private float _timePassed;


    public void InstillFaith(float amount)
    {
        _faith += amount;
    }

	// Use this for initialization
	void Start ()
	{
	    Faith = _startFaith;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _timePassed += Time.deltaTime;
	    if (_timePassed >= 1.0f)
	    {
	        _timePassed = _timePassed%1.0f;
	        _faith -= Random.Range(_minLossPerSecond, _maxLossPerSecond);
	    }
	}
}
