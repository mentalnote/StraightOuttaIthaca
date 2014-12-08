using UnityEngine;

[RequireComponent(typeof(Zeppelin))]
public class Zeppelinicate : MonoBehaviour
{
    [SerializeField]
    private float interval = 10.0f;

    private void Update()
    {
        if (Time.timeSinceLevelLoad % interval <= Time.deltaTime)
        {
            GameObject zeppelin = (GameObject)Instantiate(gameObject, transform.position, transform.rotation);

            zeppelin.GetComponent<Zeppelin>().enabled = true;
            zeppelin.GetComponent<Zeppelinicate>().enabled = false;

            Destroy(zeppelin, 45.0f);
        }
    }
}
