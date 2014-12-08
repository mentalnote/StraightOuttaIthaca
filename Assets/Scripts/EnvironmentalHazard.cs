using UnityEngine;
using System.Collections;

public class EnvironmentalHazard : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        var damageScript = other.gameObject.GetComponent<DamageScript>();
        Follower followerScript = other.gameObject.GetComponent<Follower>();
        if (damageScript != null && followerScript != null && followerScript.FollowerState == Follower.State.GettingBlown)
        {
            damageScript.applyDamage(DamageScript.DamageType.PHYSICAL, 100);
        }
        
        
    }

}
