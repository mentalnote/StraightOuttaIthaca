using UnityEngine;
using System.Collections;

public class PokeLogic : MonoBehaviour
{

    [SerializeField] 
    private float _pushForce = 1f;

    [SerializeField] 
    private float _radius = 1f;

    // Use this for initialization
	void Start ()
	{
	    RaycastHit[] hits = Physics.SphereCastAll(transform.position, _radius, new Vector3(), 1 >> LayerMask.NameToLayer("Follower"));
	    for (int i = 0; i < hits.Length; i++)
	    {
	        GameObject follower = hits[i].transform.gameObject;
            print(follower);
	        Follower fScript = follower.GetComponent<Follower>();
	        if (fScript != null)
	        {
	            Rigidbody followerRB = follower.GetComponent<Rigidbody>();
	            if (followerRB != null)
	            {
	                followerRB.AddExplosionForce(_pushForce, new Vector3(transform.position.x, follower.transform.position.y, transform.position.z), _radius);
	            }
                fScript.FollowerState = Follower.State.GettingBlown;
	        }
	    }
        Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
