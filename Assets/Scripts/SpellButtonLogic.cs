using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpellButtonLogic : MonoBehaviour
{

    public float TimePassed
    {
        get { return _timePassed; }
        set { _timePassed = value; }
    }

    [SerializeField] private float _chargeTime = 10.0f;
    [SerializeField] private int _startingCharges;
    [SerializeField] private Button _spellButton;
    [SerializeField] private Slider _chargeProgress;
    [SerializeField] private Color _indicatorColor;
    [SerializeField] private Image[] _chargeIndicators;

    private float _timePassed;
    private int _currentCharges;

    public bool ExpendCharge()
    {
        if (_currentCharges > 0)
        {
            _currentCharges--;
            _chargeIndicators[_currentCharges].color = Color.white;
            return true;
        }
        return false;
    }

	// Use this for initialization
	void Start ()
	{
	    for (int i = 0; i < _startingCharges; i++)
	    {
	        _chargeIndicators[i].color = _indicatorColor;
	    }
	    _currentCharges = _startingCharges;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (_spellButton != null && _spellButton.interactable)
	    {
            _timePassed += Time.deltaTime;    
	    }
	    if (_timePassed >= _chargeTime)
	    {
	        if (_currentCharges == _chargeIndicators.Length)
	        {
	            _timePassed = _chargeTime;
	        }
	        else
	        {
	            _chargeIndicators[_currentCharges].color = _indicatorColor;
	            _currentCharges++;
	            _timePassed = 0.0f;
	        }
	    }
	    _chargeProgress.value = _timePassed/_chargeTime;
	}
}
