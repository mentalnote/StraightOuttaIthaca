using Assets.Scripts.God;
using UnityEngine;
using System.Collections;

public class CastScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _godPowerVisual;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //left mouse button clicked
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && _godPowerVisual != null)
            {
                var godPowerInstance = Instantiate(_godPowerVisual, hit.point, Quaternion.LookRotation(new Vector3(hit.point.x, hit.point.y+100, hit.point.z) - hit.point, Camera.main.transform.up));
                //lol, get the god particle
                var godParticles = _godPowerVisual.GetComponent<ParticleSystem>();
                if (godParticles != null)
                {
                    //destory the god particle after it duration has passed
                   Destroy(godPowerInstance, godParticles.duration + godParticles.startDelay + 1); 
                }
                
                
            }
        }

        
	}
}
