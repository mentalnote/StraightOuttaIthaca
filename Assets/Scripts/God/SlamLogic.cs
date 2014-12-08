using UnityEngine;

sealed public class SlamLogic : MonoBehaviour
{
	[SerializeField]
    private float radius = 5.0f;

    [SerializeField]
	private float force = 1.0f;

    [SerializeField]
    private AudioClip soundEffect = null;

	private void Start()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
		for (int i = 0; i < colliders.Length; ++i)
		{
			Follower follower = colliders[i].gameObject.GetComponent<Follower>();
			if (follower != null && follower.rigidbody != null)
			{
				follower.BlowAwayFrom(transform.position, force, radius);
			}
        }
        
        AudioSource.PlayClipAtPoint(soundEffect, transform.position);
	}
}
