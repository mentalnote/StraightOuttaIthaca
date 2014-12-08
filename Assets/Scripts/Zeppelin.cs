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
	private GameObject people;
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

	void Start () {
		step = speed * Time.deltaTime;
		state = delivery_state.delivering;
		targetPosition = dropLocation;
		int count = 0;
		numberOfPeople = 4;
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
						print ("yo");
						Vector3 position = new Vector3(transform.position.x,transform.position.y-20,transform.position.z);
						Instantiate(people,position,Quaternion.identity);
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

	}






	//Move towards location, drop, 


}
