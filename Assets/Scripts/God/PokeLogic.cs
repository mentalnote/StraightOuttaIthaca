using UnityEngine;
using System.Collections;

public class PokeLogic : MonoBehaviour
   {
    [SerializeField] 
    private float _pushForce = 1f;

    [SerializeField] 
    private float _radius = 1f;

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    // Use this for initialization
	void Start ()
	{
	    Collider[] hits = Physics.OverlapSphere(transform.position, _radius, 1 << LayerMask.NameToLayer("Follower"));
        for (int i = 0; i < hits.Length; i++)
	    {

	        GameObject follower = hits[i].gameObject;
	        Follower fScript = follower.GetComponent<Follower>();
	        if (fScript != null)
	        {
	            fScript.BlowAwayFrom(transform.position, _pushForce, _radius);
	        }
	    }
        Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
