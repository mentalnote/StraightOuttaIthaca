using System;
using System.Linq;
using Assets.Scripts.God;
using UnityEngine;
using System.Collections;

public class CastScript : MonoBehaviour
{




    //Your selected god power
    [SerializeField] 
    private GameObject _selectedGodPower;

    private SpellButtonLogic _spellButtonLogic;

    public void SetSelectedGodPower(GameObject godPower)
    {
        if (godPower != null && godPower.GetComponent<GodPowerStats>() != null)
        {
            
            _selectedGodPower = godPower;
        }
    }

    public void SetSpellButtonLogic(GameObject spellButton)
    {
        _spellButtonLogic = spellButton.GetComponent<SpellButtonLogic>();
    }


    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //left mouse button clicked
        if (_selectedGodPower != null && Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (_selectedGodPower != null && (_spellButtonLogic == null || _spellButtonLogic.ExpendCharge()))
                {
                    var godPowerInstance =
                        (GameObject)
                            (Instantiate(_selectedGodPower, hit.point,
                                Quaternion.LookRotation(
                                    new Vector3(hit.point.x, hit.point.y + 200, hit.point.z) - hit.point,
                                    Camera.main.transform.up)));

                    //lol, get the god particle
                    var godParticles = _selectedGodPower.GetComponent<ParticleSystem>();
                    if (godParticles != null)
                    {
                        //destory the god particle after it duration has passed
                        Destroy(godPowerInstance, godParticles.duration + godParticles.startDelay + 1);
                    }
                    //set it back to null, the user has to "re-select" a new power through the menu
                    _selectedGodPower = null;
                }
                else
                {
                    _selectedGodPower = null;
                }
            }
        }

        
	}
}
