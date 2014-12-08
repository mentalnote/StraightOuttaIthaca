using UnityEngine;

sealed public class OrbitCamera : MonoBehaviour
{
    [SerializeField]
    private Transform pivotTransform = null;

    [SerializeField]
    private float viewDistance = 100.0f;

    [SerializeField]
    private float orbitSpeed = 45.0f;

    private void Update()
    {
        transform.position = pivotTransform.position;

        if (Input.mousePosition.x <= 0)
        {
            transform.Rotate(new Vector3(0.0f, orbitSpeed * Time.deltaTime, 0.0f), Space.World);
        }

        if (Input.mousePosition.x >= Screen.width - 1)
        {
            transform.Rotate(new Vector3(0.0f, -orbitSpeed * Time.deltaTime, 0.0f), Space.World);
        }

        transform.position -= transform.forward * viewDistance;
    }
}
