﻿using UnityEngine;
using System.Collections;

public class Idol : MonoBehaviour
{
    [SerializeField] private float _offsetDistance;
    [SerializeField] private float _spacing;
    [SerializeField] private int _columns;

    private int _currentWorshippers;
    private Vector3 _offsetPosition;
    private float _breadthOffset;

    void OnTriggerEnter(Collider collider)
    {
        Follower followerScript = collider.gameObject.GetComponent<Follower>();
        if (followerScript != null)
        {
           followerScript.SetIdol(this);
        }
    }

    void Start()
    {
        _offsetPosition = transform.position + (transform.forward*_offsetDistance);
        _breadthOffset = _columns%2 == 0 ? -_spacing/2.0f : 0;
    }

    public Vector3 ClaimPositionOfPraise()
    {
        int row = _currentWorshippers == 0 ? 0 : _currentWorshippers / _columns;
        int column = _currentWorshippers == 0 ? 0 : _currentWorshippers % _columns;
        _currentWorshippers++;
        int spacingCoefficient = (column+1)/2;
        return _currentWorshippers%2 == 0
            ? _offsetPosition + new Vector3(spacingCoefficient * _spacing + _breadthOffset, 0, row * _spacing)
            : _offsetPosition + new Vector3(-spacingCoefficient * _spacing + _breadthOffset, 0, row * _spacing);

    }
}