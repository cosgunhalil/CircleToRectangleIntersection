using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public bool IsMoving;

    private Transform _transform;
    private float _upperLimit;
    private Vector2 _startPosition;
    private float _speed;

    public void InitPlayer(float upperLimit, Vector2 startPosition, float speed)
    {
        _transform = GetComponent<Transform>();
        _upperLimit = upperLimit;
        _startPosition = startPosition;
        _speed = speed;
    }

    public void Move()
    {
        if (IsMoving == true)
        {
            _transform.Translate(Vector3.up * _speed * Time.deltaTime);
            if (_transform.position.y > _upperLimit)
            {
                IsMoving = false;
                ResetPlayerPosition();
                NextLevel();
            }
        }
    }

    private void NextLevel()
    {
        IntersectionDetector.Instance.LoadLevel();
    }

    private void ResetPlayerPosition()
    {
        _transform.position = _startPosition;
    }
}
