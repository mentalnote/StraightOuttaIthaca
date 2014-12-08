using UnityEngine;
using System.Collections;

public class SceneArea : MonoBehaviour
{

    [SerializeField] private Vector3 dimensions;

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, dimensions);
    }

    public Vector3 GetRandomPos()
    {
        return new Vector3(
            Random.Range(transform.position.x - dimensions.x, transform.position.x + dimensions.x),
            Random.Range(transform.position.y - dimensions.y, transform.position.y + dimensions.y),
            Random.Range(transform.position.z - dimensions.z, transform.position.z + dimensions.z)
            );
    }
}
