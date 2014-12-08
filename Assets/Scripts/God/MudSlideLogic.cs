using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MudSlideLogic : MonoBehaviour
{
    [SerializeField]
    private float radius = 5.0f;

    [SerializeField]
    private AudioClip soundEffect = null;

    private readonly List<Follower> followers = new List<Follower>();

    private void Start()
    {
        transform.position += new Vector3(0.0f, 6.7f, 0.0f);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < colliders.Length; ++i)
        {
            Follower follower = colliders[i].gameObject.GetComponent<Follower>();
            if (follower != null)
            {
                follower.GetComponent<NavMeshAgent>().Stop(true);
                follower.rigidbody.freezeRotation = true;

                followers.Add(follower);
            }
        }
        
        AudioSource.PlayClipAtPoint(soundEffect, transform.position);

        StartCoroutine("EffectFollowers");
    }
    
    private IEnumerator EffectFollowers()
    {
        yield return new WaitForSeconds(3);

        for (int i = 0; i < followers.Count; ++i)
        {
            if (followers[i] != null)
            {
                followers[i].GetComponent<NavMeshAgent>().Resume();
                followers[i].rigidbody.freezeRotation = false;
            }
        }
    }
}
