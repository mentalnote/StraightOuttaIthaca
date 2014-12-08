using UnityEngine;
using System.Collections;

public class MeteorLogic : MonoBehaviour
{
    [SerializeField]
    private float height = 20.0f;

    [SerializeField]
    private float radius = 5.0f;
    
    [SerializeField]
    private float force = 1.0f;

    private void Start()
    {
        transform.position += new Vector3(0.0f, height, 0.0f);
    }

    void OnParticleCollision(GameObject collision)
    {
        Follower follower = collision.gameObject.GetComponent<Follower>();
        if (follower != null)
        {
            follower.BlowAwayFrom(transform.position, force, radius);
        }
    }

}
