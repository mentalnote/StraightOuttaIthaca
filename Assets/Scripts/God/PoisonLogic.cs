using UnityEngine;

sealed public class PoisonLogic : MonoBehaviour
{
    [SerializeField]
    private float radius = 5.0f;

    [SerializeField]
    private float damagePerSecond = 1.0f;

    private DamageScript damageScript = null;

    private void Start()
    {
        damageScript = GetComponent<DamageScript>();

        if (damageScript != null)
        {
            particleSystem.Play();
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < colliders.Length; ++i)
        {
            Follower follower = colliders[i].gameObject.GetComponent<Follower>();
            if (follower != null && follower.gameObject.GetComponent<PoisonLogic>() == null)
            {
                follower.gameObject.AddComponent<PoisonLogic>();
            }
        }
    }

    private void Update()
    {
        if (damageScript != null || (damageScript = GetComponent<DamageScript>()) != null)
        {
            damageScript.applyDamage(DamageScript.DamageType.POISON, damagePerSecond * Time.deltaTime);
        }
    }
}
