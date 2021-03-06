﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour {

    private Transform _transform;
    private float _leftLimit;
    private float _rightLimit;
    private float _upperLimit;
    private float _downLimit;
    private float _speed;
    private Rectangle _rectangle;

    public enum Direction
    {
        right,
        left
    }

    public void InitMovingObject(float leftLimit, float upperLimit, float speed)
    {
        _transform = GetComponent<Transform>();
        _rectangle = GetComponent<Rectangle>();
        if (_transform.position.x < 0)
        {
            _leftLimit = _transform.position.x;

        }
        else
        {
            _leftLimit = -_transform.position.x;
        }

        _rightLimit = -_leftLimit;
        _upperLimit = upperLimit;
        _downLimit = -upperLimit;
        _speed = speed;
    }

    public void InitMovingObstacleObject(float leftLimit, float rightLimit, float speed)
    {
        _transform = GetComponent<Transform>();
        _rectangle = GetComponent<Rectangle>();

        _leftLimit = leftLimit;
        _rightLimit = rightLimit;
        _speed = speed;
    }

    public void Move()
    {
        if (_transform.position.x > _rightLimit || _transform.position.x < _leftLimit)
        {
            _speed *= -1;
            _rectangle.SetColor(Color.white);
        }

        _transform.Translate(Vector3.right * _speed * Time.deltaTime);
    }

    internal void MoveLinear(EDirection _direction)
    {
        if (_direction == EDirection.left)
        {
            if (_speed > 0)
            {
                _speed *= -1;
            }

            if (_transform.position.x < _leftLimit)
            {
                _transform.position = new Vector3(_rightLimit, _transform.position.y, _transform.position.z);
                _rectangle.SetColor(Color.white);
            }
        }
        else
        {
            
            if (_transform.position.x > _rightLimit)
            {
                _transform.position = new Vector3(_leftLimit, _transform.position.y, _transform.position.z);
                _rectangle.SetColor(Color.white);
            }
        }
        _transform.Translate(Vector3.right * _speed * Time.deltaTime);
    }
}

public enum EDirection
{
    right,
    left
}
