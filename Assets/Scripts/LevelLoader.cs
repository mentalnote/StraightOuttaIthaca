using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour
{
    public void LoadNextLevel(string levelName)
    {
        Application.LoadLevel(levelName);
    }
}
