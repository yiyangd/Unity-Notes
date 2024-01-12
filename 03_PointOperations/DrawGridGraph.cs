using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGridGraph : MonoBehaviour
{
    public int size = 28; // Size of each grid cell
    public int xmax = 200; // Maximum extent of the grid along the x-axis
    public int ymax = 200; // Maximum extent of the grid along the y-axis

    // Define Coords objects for the start and end points of the X and Y axes
    Coords startPointXAxis;
    Coords endPointXAxis;
    Coords startPointYAxis;
    Coords endPointYAxis;

    // Start is called before the first frame update
    void Start()
    {
        startPointXAxis = new Coords(-xmax,0);
        endPointXAxis = new Coords(xmax,0);
        startPointYAxis = new Coords(0,-ymax);
        endPointYAxis = new Coords(0,ymax);

        // Draw lines to represent the X and Y axes
        Coords.DrawLine(startPointXAxis, endPointXAxis, 1, Color.red);
        Coords.DrawLine(startPointYAxis, endPointYAxis, 1, Color.green);

        int xoffset = (int)(xmax/(float)size);

        for (int x = -xoffset*size; x <= xoffset*size; x+=size){
            Coords.DrawLine(new Coords(x, -ymax), new Coords(x, ymax), 0.5f, Color.white);
        }

        int yoffset = (int)(ymax/(float)size);

        for (int y = -yoffset*size; y <= yoffset*size; y+=size){
            Coords.DrawLine(new Coords(-xmax, y), new Coords(xmax, y), 0.5f, Color.white);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
