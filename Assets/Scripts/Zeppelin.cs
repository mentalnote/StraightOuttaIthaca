using UnityEngine;
using System.Collections;

public class Zeppelin : MonoBehaviour {

	public enum delivery_state
	{
		delivering,
		dropping,
		dropped
	}

	[SerializeField]
	private Follower people;
	[SerializeField]
	private Vector3 dropLocation;
	[SerializeField]
	private Vector3 endLocation;
	[SerializeField]
	private float speed;
	[SerializeField]
	private int droppingDuration;
	private Vector3 targetPosition;
	private delivery_state state;
	private float step;
	private int count;
	private int numberOfPeople;

    [SerializeField]
    private Vector3 lookRotationOffset = Vector3.zero;

	void Start () {
		step = speed * Time.deltaTime;
		state = delivery_state.delivering;
		targetPosition = dropLocation;
		int count = 0;
		numberOfPeople = 4 + (int)(Time.timeSinceLevelLoad / 60.0f);
	}	
	void setNumberOfPeople(int number){
		numberOfPeople = number;
	}

	void setDroppingDuration(int duration){
		droppingDuration = duration;
	}

	void Update () {
		switch (state){
			case  delivery_state.delivering:
				if (transform.position == dropLocation){
					state = delivery_state.dropping;
				}
				break;
			case delivery_state.dropping:
				if (count < droppingDuration){
					int dropIntervals = droppingDuration/numberOfPeople;
					if (count % dropIntervals == 0){
                        NavMeshHit hit;
						
                        if (NavMesh.SamplePosition(transform.position + new Vector3(Random.Range(-15.0f, 15.0f), 0.0f, Random.Range(-15.0f, 15.0f)), out hit, 500.0f, 1))
                        {
                            Instantiate(people,hit.position,Quaternion.identity);
                        }
					}
					count++;
					
				} else {
					state = delivery_state.dropped;
					targetPosition = endLocation;
				}
				break;

			case delivery_state.dropped:
				if (transform.position == targetPosition){
					Destroy(gameObject);
				}
				break;
		}
		transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        if (targetPosition != transform.position)
        {
            transform.rotation = Quaternion.LookRotation(targetPosition - transform.position) * Quaternion.Euler(lookRotationOffset);
        }
	}
}
