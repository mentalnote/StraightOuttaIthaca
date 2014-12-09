using UnityEngine;
using System.Collections;

public class GameObjectArray : MonoBehaviour
{

    [SerializeField] private GameObject[] _gameObjects;

    public Vector3[] getPositions()
    {
        if (_gameObjects != null)
        {
            Vector3[] vectors = new Vector3[_gameObjects.Length];
            for (int i = 0; i < _gameObjects.Length; i++)
            {
                vectors[i] = _gameObjects[i].transform.position;
            }
            return vectors;
        }
        return null;
    }
}
