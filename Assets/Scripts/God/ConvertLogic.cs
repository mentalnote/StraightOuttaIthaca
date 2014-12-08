using UnityEngine;
using System.Collections;

public class ConvertLogic : MonoBehaviour
{
    [SerializeField]
    private float radius = 5.0f;
    
    [SerializeField]
    private float faith = 10.0f;

    [SerializeField]
    private AudioClip soundEffect = null;
    
    private void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < colliders.Length; ++i)
        {
            Follower follower = colliders[i].gameObject.GetComponent<Follower>();
            if (follower != null)
            {
                follower.FaithTracker.Faith += faith;
            }
        }
        
        AudioSource.PlayClipAtPoint(soundEffect, transform.position);
    }
}
