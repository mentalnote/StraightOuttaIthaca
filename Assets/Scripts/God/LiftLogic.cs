using UnityEngine;

public class LiftLogic : MonoBehaviour 
{
    private SphereCollider sphereCollider;
    private Transform selectedFollower = null;
    private float liftHeight = 52;  //max height of terrain

    private void Start()
    {
        sphereCollider = collider as SphereCollider;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sphereCollider.radius);

        float smallestDistance = 0;

        for (int i = 0; i < hitColliders.Length; ++i)
        {
            if (hitColliders[i].name.StartsWith("Follower"))
            {
                Transform follower = hitColliders[i].gameObject.transform;
                float distanceFromLiftPt = (follower.position - sphereCollider.transform.position).magnitude;
                if (selectedFollower == null || distanceFromLiftPt < smallestDistance)
                {
                    selectedFollower = follower;
                }
            }
        }

        Debug.Log("follower = " + selectedFollower);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePt = camera.ScreenToWorldPoint(Input.mousePosition);

            selectedFollower.position = new Vector3(mousePt.x, liftHeight, mousePt.z);
        }

        else if(Input.GetMouseButtonUp(0))
        {
            Vector3 mousePt = camera.ScreenToWorldPoint(Input.mousePosition);

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //if (Physics.Raycast(ray, out hit))
            //{

            selectedFollower.position = new Vector3(mousePt.x, liftHeight, mousePt.z);
        }

        //else if (Input.GetMouseButtonDown(0))
        //{
        //    LiftFollower();
        //}
    }

    private void LiftFollower()
    {
        
    }
}
