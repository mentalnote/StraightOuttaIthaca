using System;
using System.Linq;
using Assets.Scripts.God;
using UnityEngine;
using System.Collections;

public class CastScript : MonoBehaviour
{


    [System.Serializable]
    public class GodPowerEntry
    {
        public GodPowerType PowerType;
        public GameObject Visual;
    }

   
    [SerializeField] 
    private GodPowerEntry[] _godPowerEntries;

    //Your selected god power
    [SerializeField] 
    private GodPowerType _selectedGodPower = GodPowerType.None;
    public void SetSelectedGodPower(String godPowerName)
    {
        var godPowerType = (GodPowerType) Enum.Parse(typeof (GodPowerType), godPowerName);
        _selectedGodPower = godPowerType;
    }


    /**
     * Select the god power visual from the list of all the god powers
     */
    private GameObject GetGodPowerVisual(GodPowerType godPowerType)
    {
        return (from godPowerEntry in _godPowerEntries where godPowerEntry.PowerType.Equals(godPowerType) select godPowerEntry.Visual).FirstOrDefault();
    }

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //left mouse button clicked
        if (_selectedGodPower != GodPowerType.None && Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var godPowerVisual = GetGodPowerVisual(_selectedGodPower);
                if (godPowerVisual != null)
                {
                    var godPowerInstance = Instantiate(godPowerVisual, hit.point, Quaternion.LookRotation(new Vector3(hit.point.x, hit.point.y + 100, hit.point.z) - hit.point, Camera.main.transform.up));
                    //lol, get the god particle
                    var godParticles = godPowerVisual.GetComponent<ParticleSystem>();
                    if (godParticles != null)
                    {
                        //destory the god particle after it duration has passed
                        Destroy(godPowerInstance, godParticles.duration + godParticles.startDelay + 1);
                    }
                    _selectedGodPower = GodPowerType.None;
                }
            }
        }

        
	}
}
