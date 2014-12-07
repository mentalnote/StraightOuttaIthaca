using UnityEngine;
using System.Collections;

public class MeteorLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnParticlesCollision(GameObject collision)
    {
        //meteors actually have collision on their particles
        //might work...easier?
        print("luck (prefab_testing)");
    }

}
