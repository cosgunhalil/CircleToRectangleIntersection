using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour {

    private Transform _transform;
    public float Radius;

    public void CreateCircle(Vector2 position , float r)
    {
        InitCircle();
        _transform.position = position;
        Radius = r;
    }

    private void InitCircle()
    {
        _transform = GetComponent<Transform>();
    }

    public Vector2 GetCirclePosition()
    {
        return _transform.position;
    }

    public void SetCirclePosition(Vector2 position)
    {
        _transform.position = position;
    }
}
