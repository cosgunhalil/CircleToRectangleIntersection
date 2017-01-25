using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rectangle : MonoBehaviour {

    private Transform _transform;
    private float _height;
    private float _width;

    public Rectangle(Vector2 position , float height , float width)
    {
        InitRectangle();
        _transform.position = position;
        _height = height;
        _width = width;    
    }

    private void InitRectangle()
    {
        _transform = GetComponent<Transform>();
    }

    public Vector2 GetRectPosition()
    {
        return _transform.position;
    }

    public void SetRectPosition(Vector2 position)
    {
        _transform.position = position;
    }
}
