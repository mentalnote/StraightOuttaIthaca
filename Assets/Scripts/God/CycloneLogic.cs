using UnityEngine;
using System.Collections;

public class CycloneLogic : MonoBehaviour
{
    [SerializeField]
    private float radius = 5.0f;

    [SerializeField]
    private float force = 1;

    [SerializeField]
    private float turnSpeed = 180.0f;

    [SerializeField]
    private AudioClip soundEffect = null;

    private void Start()
    {
        StartCoroutine("EffectFollowers");
        
        AudioSource.PlayClipAtPoint(soundEffect, transform.position);
    }

    private IEnumerator EffectFollowers()
    {
        yield return new WaitForSeconds(1);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        for (int i = 0; i < hitColliders.Length; ++i)
        {
            Follower follower = hitColliders[i].gameObject.GetComponent<Follower>();
            if (follower != null)
            {
                follower.BlowAwayFrom(follower.transform.position - new Vector3(0.0f, -1.0f, 0.0f), force, radius);

                follower.transform.parent = transform;
            }
        }
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0.0f, Time.timeSinceLevelLoad * turnSpeed, 0.0f);
    }
}
