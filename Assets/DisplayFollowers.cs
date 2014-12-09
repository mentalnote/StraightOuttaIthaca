using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayFollowers : MonoBehaviour
{
	[SerializeField]
    private PrayerTracker tracker = null;

    private void Update ()
    {
        GetComponent<Text>().text = "Followers: " + tracker.FollowerCount;
	}
}
