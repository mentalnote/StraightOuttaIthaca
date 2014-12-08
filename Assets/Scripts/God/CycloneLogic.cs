using UnityEngine;
using System.Collections;

public class CycloneLogic : MonoBehaviour
{
    [SerializeField]
    private float force = 1;
    
    private SphereCollider sphereCollider;

    private void Start()
    {
        sphereCollider = collider as SphereCollider;

        StartCoroutine("EffectFollowers");
    }

    private IEnumerator EffectFollowers()
    {
        yield return new WaitForSeconds(1);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sphereCollider.radius);

        for (int i = 0; i < hitColliders.Length; ++i)
        {
            if (hitColliders[i].name.StartsWith("Follower"))
            {
                GameObject followerObject = hitColliders[i].gameObject;
                Follower follower = hitColliders[i].GetComponent<Follower>();
                follower.FollowerState = Follower.State.GettingBlown;
                BlowFollowerAway(followerObject.transform);
            }
        }
    }

    private void BlowFollowerAway(Transform follower)
    {
        Vector3 direction = new Vector3(follower.position.x - sphereCollider.transform.position.x, 0f, follower.position.z - sphereCollider.transform.position.z);

        direction.Normalize();

        follower.rigidbody.AddForce(direction * force);
    }
}
