using UnityEngine;
using System.Collections;

public class FollowerSpawner : MonoBehaviour
{

    [SerializeField] private float _spawnPerMinute;
    [SerializeField] private GameObject _prefab;

    private bool _stop;
    private float _timeSinceLastSpawn = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (!_stop && _prefab != null)
	    {
	        if (_timeSinceLastSpawn > (_spawnPerMinute/60.0f))
	        {
	            GameObject follower = (GameObject)Instantiate(_prefab, transform.position, transform.rotation);
	            Follower followerScript = follower.GetComponent<Follower>();
	            if (followerScript != null && Random.Range(0,2) == 0)
	            {
	                followerScript.FollowerState = Follower.State.Returning;
	            }
	            _timeSinceLastSpawn = 0;
	        }
	        _timeSinceLastSpawn += Time.deltaTime;
	    }

	    if (Input.GetKeyDown(KeyCode.Space))
	    {
	        _stop = !_stop;
	    }

	}
}
