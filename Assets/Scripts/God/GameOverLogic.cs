using UnityEngine;
using System.Collections;

public class GameOverLogic : MonoBehaviour
{

    [SerializeField]
    private int _numToLose = 20;

    [SerializeField] private string _level = "GameOver";

    public int EscapeNum
    {
        get { return _escapeNum; }
        set
        {
            _escapeNum = value;
            if (value >= _numToLose)
            {
                Application.LoadLevel(_level);
            }
        }
    }

    private int _escapeNum;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
