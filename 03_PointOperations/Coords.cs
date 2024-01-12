using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coords
{
    float x;
    float y;
    float z;

    public Coords(float _X, float _Y)
    {
        x = _X;
        y = _Y;
        z = -1;
    }

    public override string ToString()
    {
        return "(" + x + "," + y + "," + z + ")";
    }

    // Static method to draw a point in the Unity scene
    static public void DrawPoint(Coords position, float width, Color colour)
    {
        // Create a new GameObject for the point
        GameObject line = new GameObject("Point_" + position.ToString());
        // Add a LineRenderer component to the GameObject
        LineRenderer lineRenderer =  line.AddComponent<LineRenderer>();
        // Set the material of the LineRenderer and assign the color
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = colour;
        // Set the number of positions (points) in the line
        lineRenderer.positionCount = 2;
        // Set the positions for the line (creating a short line segment to represent a point)
        lineRenderer.SetPosition(0, new Vector3(position.x - width/3.0f, position.y - width/3.0f, position.z));
        lineRenderer.SetPosition(1, new Vector3(position.x + width/3.0f, position.y + width/3.0f, position.z));
        // Set the width of the start and end of the line
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }

    // Static method to draw a line between two points in the Unity scene
    static public void DrawLine(Coords startPos, Coords endPos, float width, Color colour)
    {
        // Create a new GameObject for the line
        GameObject line = new GameObject("Line_" + startPos.ToString() + "_" + endPos.ToString());
        // Add a LineRenderer component to the GameObject
        LineRenderer lineRenderer =  line.AddComponent<LineRenderer>();
        // Set the material of the LineRenderer and assign the color
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = colour;
        // Set the number of positions (points) in the line
        lineRenderer.positionCount = 2;
        // Set the start and end positions of the line
        lineRenderer.SetPosition(0, new Vector3(startPos.x, startPos.y, startPos.z));
        lineRenderer.SetPosition(1, new Vector3(endPos.x, endPos.y, endPos.z));
        // Set the width of the start and end of the line
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }

}
