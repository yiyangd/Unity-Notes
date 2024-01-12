using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLines : MonoBehaviour
{

    // Define Coords objects for the start and end points of the X and Y axes
    Coords startPointXAxis = new Coords(-160,0);
    Coords endPointXAxis = new Coords(160,0);
    Coords startPointYAxis = new Coords(0,100);
    Coords endPointYAxis = new Coords(0,-100);

    // Define an array of Coords to represent specific points (possibly a pattern or constellation)
    Coords[] leo = {
        new Coords(0,20),
        new Coords(20,30),
        new Coords(80,30),
        new Coords(30,50),
        new Coords(80,50),
        new Coords(70,60),
        new Coords(70,80),
        new Coords(80,90),
        new Coords(95,80)
    };

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(point.ToString());
        //Coords.DrawPoint(new Coords(0,0), 2, Color.red);
        //Coords.DrawPoint(point, 2, Color.green);
        //Coords.DrawPoint(point2, 10, Color.blue);

        // Draw lines to represent the X and Y axes
        Coords.DrawLine(startPointXAxis, endPointXAxis, 0.5f, Color.yellow);
        Coords.DrawLine(startPointYAxis, endPointYAxis, 0.5f, Color.green);

        // Draw points at each coordinate in the 'leo' array
        foreach(Coords c in leo)
        {
            Coords.DrawPoint(c, 2, Color.yellow);
        }
        // Connect the points in the 'leo' array with lines
        Coords.DrawLine(leo[0], leo[1], 0.4f, Color.white);
        Coords.DrawLine(leo[1], leo[2], 0.4f, Color.white);
        Coords.DrawLine(leo[0], leo[3], 0.4f, Color.white);
        Coords.DrawLine(leo[3], leo[5], 0.4f, Color.white);
        Coords.DrawLine(leo[2], leo[4], 0.4f, Color.white);
        Coords.DrawLine(leo[4], leo[5], 0.4f, Color.white);
        Coords.DrawLine(leo[5], leo[6], 0.4f, Color.white);
        Coords.DrawLine(leo[6], leo[7], 0.4f, Color.white);
        Coords.DrawLine(leo[7], leo[8], 0.4f, Color.white);
    }



    // Update is called once per frame
    void Update()
    {
         // Currently empty - can be used for frame-by-frame updates or interactions
    }
}
