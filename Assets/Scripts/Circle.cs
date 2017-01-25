using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour {

    private Transform _transform;
    private float _radius;

    public Circle(Vector2 position , float r)
    {
        InitCircle();
        _transform.position = position;
        _radius = r;
    }

    private void InitCircle()
    {
        _transform = GetComponent<Transform>();
    }

    public Vector2 GetCirclePosition()
    {
        return _transform.position;
    }

    public void SetPosition(Vector2 position)
    {
        _transform.position = position;
    }
}
