using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rectangle : MonoBehaviour {

    private Transform _transform;
    public float Height;
    public float Width;

    public void CreateRect(Vector2 position , float height , float width)
    {
        InitRectangle();
        _transform.position = position;
        Height = height;
        Width = width;    
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
