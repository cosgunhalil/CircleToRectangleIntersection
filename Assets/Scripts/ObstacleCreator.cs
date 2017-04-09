using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCreator : MonoBehaviour {

    public int ObjectCount;
    public float ObstacleHeight;
    public float ObstacleWidth;

    public List<Rectangle> Rects;
    private List<GameObject> _rectPool;
    public List<MovingObject> MovingObjects;

    private Transform _transform;
    private EDirection _direction;

    public void Init()
    {
        _transform = GetComponent<Transform>();
        InitDirection();
        Rects = new List<Rectangle>();
        _rectPool = new List<GameObject>();
        MovingObjects = new List<MovingObject>();
        InitRects();
        InitMovingObjects();
    }

    private void InitDirection()
    {
        if (_transform.position.x < 0)
        {
            _direction = EDirection.right;
        }
        else
        {
            _direction = EDirection.left;
        }
    }

    private void InitRects()
    {
        for (int i = 0; i < ObjectCount; i++)
        {
            Rectangle r = GenerateRect().GetComponent<Rectangle>();
            r.CreateRect(_transform.position, ObstacleHeight, ObstacleWidth);
            Rects.Add(r);
            MovingObjects.Add(r.GetComponent<MovingObject>());
        }
    }

    private GameObject GenerateRect()
    {
        GameObject rect;
        if (_rectPool.Count < 1)
        {
            rect = Instantiate(Resources.Load("Rectangle", typeof(GameObject))) as GameObject;
            _rectPool.Add(rect);
        }

        rect = _rectPool[_rectPool.Count - 1];
        _rectPool.Remove(rect);

        rect.SetActive(true);
        return rect;
    }

    private void InitMovingObjects()
    {
        float leftLimit;
        float rightLimit;

        for (int i = 0; i < MovingObjects.Count; i++)
        {
            
            if (_transform.position.x < 0)
            {
                leftLimit = _transform.position.x;
                rightLimit = -leftLimit;
            }
            else
            {
                rightLimit = _transform.position.x;
                leftLimit = -rightLimit;
            }
            MovingObjects[i].InitMovingObstacleObject(leftLimit - i,rightLimit + i, 2);
        }
    }

    public Vector2 GetPosition()
    {
        return new Vector2(transform.position.x , transform.position.y);
    }

    public void MoveRects()
    {

        for (int i = 0; i < MovingObjects.Count; i++)
        {
            MovingObjects[i].MoveLinear(_direction);
        }

    }

    public void AddObjectToPool(GameObject go)
    {
        go.SetActive(false);
        _rectPool.Add(go);
    }

    public void RePoolObjects()
    {
        //foreach (var item in _movingObjects)
        //{
        //    item.gameObject.SetActive(false);
        //    _movingObjects.Remove(item);
        //    _rectPool.Add(item.gameObject);
        //    _movingObjects = new List<MovingObject>();
        //}
    }


}
