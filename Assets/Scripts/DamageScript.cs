using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageScript : MonoBehaviour {

    public enum DamageType {
        PHYSICAL, FIRE, POISON
    }

    public delegate void DamageCallback(float damage, float health);

    private DamageCallback damageCallbacks;
    private Dictionary<DamageType, float> resistanceValues;
    private Dictionary<string, float> bodyModValues;
    public float health;
    public Resistance[] resistances;
    public BodyPartModifier[] bodyModifiers;

    [System.Serializable]
    public class Resistance {
        public DamageType type;
        public float reduction;
    }

    [System.Serializable]
    public class BodyPartModifier {
        public string part;
        public float reduction;
    }

    public void addDamageListener(DamageCallback callback){
        if(damageCallbacks == null){
            damageCallbacks = callback;
        } else {
            damageCallbacks += callback;
        }
    }

    public void applyDamage(DamageType type, float damage){
        float damagea = resistanceValues.ContainsKey(type) ? damage-(damage*resistanceValues[type]) : damage;
        health -= damagea;  
        if(damageCallbacks != null){
            damageCallbacks.Invoke(damagea, health);
        }
    }

    public void applyDamage(DamageType type, float damage, string bodyPart){
        float damagea = damage;
        if(bodyModValues.ContainsKey(bodyPart)){
            damagea = damage-(damage*bodyModValues[bodyPart]);    
        }
        damagea = resistanceValues.ContainsKey(type) ? damagea-(damagea*resistanceValues[type]) : damagea;
        health -= damagea;
        if(damageCallbacks != null){
            damageCallbacks.Invoke(damagea, health);
        }
        Debug.Log(gameObject.name + " was dealt " + damagea + " " + type.ToString() + " damage.");
    }

	// Use this for initialization
	void Start () {
        resistanceValues = new Dictionary<DamageType, float>();
        foreach(Resistance resistance in resistances){
            resistanceValues.Add(resistance.type, resistance.reduction);
            }
        bodyModValues = new Dictionary<string, float>();
        foreach(BodyPartModifier bMod in bodyModifiers){
            bodyModValues.Add(bMod.part, bMod.reduction);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
