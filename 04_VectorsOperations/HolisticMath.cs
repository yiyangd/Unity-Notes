using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolisticMath
{   

    // Calculates the Euclidean distance between two points
    static public float Distance(Coords point1, Coords point2)
    {
        // Compute the squared difference in each dimension and sum them
        float diffSquared = Square(point1.x - point2.x) 
                          + Square(point1.y - point2.y)
                          + Square(point1.z - point2.z);
        // Take the square root to get the distance
        float squareRoot = Mathf.Sqrt(diffSquared);
        return squareRoot;
    }
    // Returns the square of a value
    static public float Square(float value)
    {
        return value * value;
    }

    // Normalizes a given vector to unit length
    static public Coords GetNormal(Coords vector)
    {
        // Calculate the vector's length
        float length = Distance(new Coords(0,0,0), vector);
        // Divide each component by the length to normalize
        vector.x /= length;
        vector.y /= length;
        vector.z /= length;

        return vector;
    }


    // Computes the dot product of two vectors
    static public float Dot(Coords vector1, Coords vector2)
    {
        // Multiply corresponding components and sum them
        return(vector1.x * vector2.x + vector1.y * vector2.y + vector1.z * vector2.z);
    }

    // Calculates the angle between two vectors (result in radians)
    static public float Angle(Coords vector1, Coords vector2)
    {
        // Calculate the dot product divided by the product of the magnitudes of the vectors
        float dotDivide = Dot(vector1, vector2) 
                            / (Distance(new Coords(0,0,0), vector1) 
                               * Distance(new Coords(0,0,0), vector2));
        // Return the arccosine of the result
        return Mathf.Acos(dotDivide); 
        // For degrees, multiply by 180/Mathf.PI
    }


    // Rotates a vector by a specified angle (in radians)
    static public Coords Rotate(Coords vector, float angle, bool clockwise) // radians
    {
        // If clockwise rotation is needed, adjust the angle accordingly
        if(clockwise)
        {
            angle = 2 * Mathf.PI - angle;
        }
        // Calculate the new coordinates after rotation
        float xVal = vector.x * Mathf.Cos(angle) - vector.y * Mathf.Sin(angle);
        float yVal = vector.x * Mathf.Sin(angle) + vector.y * Mathf.Cos(angle);

        return new Coords(xVal, yVal, 0);
    }

    // Computes the cross product of two vectors
    static public Coords Cross(Coords vector1, Coords vector2)
    {
        // Perform cross product calculation
        float xMult = vector1.y * vector2.z - vector1.z * vector2.y;
        float yMult = vector1.z * vector2.x - vector1.x * vector2.z;
        float zMult = vector1.x * vector2.y - vector1.y * vector2.x;

        // Return the resulting vector
        Coords crossProd = new Coords(xMult, yMult, zMult);
        return crossProd;
    }

    // Adjusts a vector to point towards a specific target point
    static public Coords LookAt2D(Coords forwardVector, Coords position, Coords focusPoint)
    {
        // Calculate the direction from the position to the focus point
        Coords direction = new Coords(focusPoint.x - position.x, 
                                      focusPoint.y - position.y, 
                                      position.z);
        // Determine the angle to rotate the forward vector to align with the direction
        float angle = HolisticMath.Angle(forwardVector, direction);

        // Determine the direction of rotation (clockwise or counter-clockwise)
        bool clockwise = false; // counter-clockwise if cross product is positive
        if(HolisticMath.Cross(forwardVector, direction).z < 0)
            clockwise = true;
        
        // Perform the rotation
        Coords newDir = HolisticMath.Rotate(forwardVector, angle, clockwise);
        return newDir;
    }
}
