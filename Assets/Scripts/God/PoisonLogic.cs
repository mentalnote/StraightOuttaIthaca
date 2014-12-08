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
        particleSystem.Play();

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < colliders.Length; ++i)
        {
            Follower follower = colliders[i].gameObject.GetComponent<Follower>();
            if (follower != null && follower.gameObject.GetComponentsInChildren<PoisonLogic>().Length == 0)
            {
                PoisonLogic poisonLogic = (PoisonLogic)Instantiate(this, follower.transform.position, Quaternion.identity);

                poisonLogic.transform.parent = follower.transform;
                poisonLogic.damageScript = follower.gameObject.GetComponent<DamageScript>();
            }
        }
    }

    private void Update()
    {
        if (damageScript != null)
        {
            damageScript.applyDamage(DamageScript.DamageType.POISON, damagePerSecond * Time.deltaTime);
        }
    }
}
