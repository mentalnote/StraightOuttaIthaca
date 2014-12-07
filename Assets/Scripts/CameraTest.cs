using UnityEngine;
using System.Collections;

public class CameraTest : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 100.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 translationVec = new Vector3();
	    if (Input.GetKey(KeyCode.UpArrow))
	    {
	        translationVec.y += moveSpeed*Time.deltaTime;
	    }
        if (Input.GetKey(KeyCode.DownArrow))
	    {
	        translationVec.y -= moveSpeed*Time.deltaTime;
	    }
        if (Input.GetKey(KeyCode.LeftArrow))
	    {
	        translationVec.x -= moveSpeed*Time.deltaTime;
	    }
        if (Input.GetKey(KeyCode.RightArrow))
	    {
	        translationVec.x += moveSpeed*Time.deltaTime;
	    }
	    Camera.main.transform.Translate(translationVec);
	}
}
