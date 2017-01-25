using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionDetector : MonoBehaviour {

    public Rectangle Rectangle;
    public Circle Circle;
    public bool isInterSect;

    private Vector2 _circleDistance;
    
    void Start()
    {
        Circle.CreateCircle(new Vector2(0,0), 2);
        Rectangle.CreateRect(new Vector2(5, 5), 2, 2);
    }

	
	// Update is called once per frame
	void Update ()
    {
        isInterSect = CheckIntersection();
	}

    private bool CheckIntersection()
    {
        _circleDistance.x = Math.Abs(Circle.GetCirclePosition().x - Rectangle.GetRectPosition().x);
        _circleDistance.y = Math.Abs(Circle.GetCirclePosition().y - Rectangle.GetRectPosition().y);

        if (_circleDistance.x > (Rectangle.Width / 2 + Circle.Radius))
        {
            return false;
        }
        if (_circleDistance.y > (Rectangle.Height / 2 + Circle.Radius))
        {
            return false;
        }

        if (_circleDistance.x <= (Rectangle.Width / 2))
        {
            return true;
        }
        if (_circleDistance.y <= (Rectangle.Height / 2))
        {
            return true;
        }

        var cornerDistance_sq = Math.Pow((_circleDistance.x - Rectangle.Width / 2) , 2) + 
                                Math.Pow(_circleDistance.y - Rectangle.Height / 2 , 2);

        return (cornerDistance_sq <= (Math.Pow(Circle.Radius,2)));
    }
}
