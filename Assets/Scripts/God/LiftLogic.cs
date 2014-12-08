using UnityEngine;

public class LiftLogic : MonoBehaviour 
{
    private SphereCollider sphereCollider;
    private Transform selectedFollower = null;
    private float liftHeight = 52;  //max height of terrain
    private bool lifting = false;

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

        if (selectedFollower != null)
        {
            selectedFollower.rigidbody.isKinematic = true;
            lifting = true;

            Debug.Log("follower = " + selectedFollower);
        }

    }

    private void Update()
    {
        if(lifting)
        {
            //USE FOR CORRECT RAYCASTING
            //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;
            //if (Physics.Raycast(ray, out hit, 1 << LayerMask.NameToLayer("Terrain")))
            //{
            //    if (hit.collider != null)
            //    {
            //        Vector3 worldPos = hit.point;
            //        Vector3 newPos = new Vector3(worldPos.x, worldPos.y + 10, worldPos.y);
            //        Debug.Log("newPos = " + newPos);

            //        selectedFollower.position = newPos;
            //    }
            //}




            //Vector3 mousePt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log(mousePt);
            //Debug.Log(Input.mousePosition);

            //selectedFollower.position = new Vector3(mousePt.x, liftHeight, mousePt.z);
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectedFollower.rigidbody.isKinematic = false;
            lifting = false;
        }
    }

    private void LiftFollower()
    {
        
    }
}
