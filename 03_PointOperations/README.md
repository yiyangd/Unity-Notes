## Unity 03 - Draw Points in Unity
> This note documents how to draw points, lines between two points, and grid graphs in a Unity 2D environment by writing the `Coords` class and `DrawLines`, `DrawGridGraph` scripts. These scripts utilize Unity's **LineRenderer component**, demonstrating *how to draw basic geometric shapes through coordinate positioning, loop logic, and color configuration*.
### 1. Class Definition: `Coords`
Right Click in the assets, create a C# script and rename it `Coords`
- The class represents a coordinate in a 3D space and includes methods to draw points and lines at these coordinates. 

#### 1.1. Float Variables xyz

Create Private float variables to store the x, y, and z coordinates.

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coords
{
    // Private fields to store the x, y, and z coordinates
    float x;
    float y;
    float z;
    // Constructor for the Coords class
```

#### 1.2. Constructor
`Coords(float _X, float _Y):`
- **Parameters:** `_X` and `_Y` are floats representing the *x* and *y* coordinates.
- *Initializes x* and *y* with the given values and *sets z to -1*. 
  - This implies that the `Coords` object is primarily for 2D coordinates, with a *default z-value of -1*.


```csharp
    // Constructor for the Coords class
    public Coords(float _X, float _Y)
    {
        x = _X;  // Set the x coordinate
        y = _Y;  // Set the y coordinate
        z = -1;  // Default z coordinate set to -1
    }
    
    // Override the ToString method to return coordinates in a readable format
```

#### 1.3. Method: `ToString`

- *Overrides* the default `ToString()` method.
- *Returns a string* representation of the coordinate in the format *"(x,y,z)"*.

```csharp
    // Override the ToString method to return coordinates in a readable format
    public override string ToString()
    {
        return "(" + x + "," + y + "," + z + ")";
    }
    // Static method to draw a point in the Unity scene
```

#### 1.4. Static Method: `DrawPoint()`

- **Parameters:** `position` (a *Coords* object), `width` (size of the point), and `colour`.
- **Functionality:**
  - Creates a `GameObject` named *"Point_"* followed by the *position's string* representation.
    - e.g. *Point_(0,0,-1)*
  - Adds a `LineRenderer` component to this object, which is used to render lines in Unity.
  - Sets the `material and color` of the line.
  - Configures the line to have two points, essentially *drawing a very short line (interpreted as a point)*.
  - The positions of these two points are calculated based on the *width* parameter, creating the appearance of a point.


```csharp
    // Static method to draw a point in the Unity scene
    static public void DrawPoint(Coords position, float width, Color colour)
    {
        // Create a new GameObject for the point
        GameObject line = new GameObject("Point_" + position.ToString());

        // Add a LineRenderer component to the GameObject
        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();

        // Set the material of the LineRenderer and assign the color
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = colour;

        // Set the number of positions (points) in the line
        lineRenderer.positionCount = 2;

        // Set the positions for the line (creating a short line segment to represent a point)
        lineRenderer.SetPosition(0, new Vector3(position.x - width / 3.0f, position.y - width / 3.0f, position.z));
        lineRenderer.SetPosition(1, new Vector3(position.x + width / 3.0f, position.y + width / 3.0f, position.z));

        // Set the width of the start and end of the line
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }

    // Static method to draw a line between two points in the Unity scene
```

#### 1.5. Static Method: `DrawLine()`

- **Parameters:** `startPos` and `endPos` (both *Coords* objects),  `width`, and `colour`.
- **Functionality:**
  - Similar to `DrawPoint`, this method creates a `GameObject` named *"Line_"* followed by the *string representations of startPos and endPos*.
    - e.g. **x-axis:**
      - *Line_(-160,0,-1)_(160,0,-1)*
  - Adds a `LineRenderer` and sets up its material and color.
  - The line is drawn between `startPos` and `endPos` coordinates with the *specified width*.


```csharp
    // Static method to draw a line between two points in the Unity scene
    static public void DrawLine(Coords startPos, Coords endPos, float width, Color colour)
    {
        // Create a new GameObject for the line
        GameObject line = new GameObject("Line_" + startPos.ToString() + "_" + endPos.ToString());

        // Add a LineRenderer component to the GameObject
        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();

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
```

- This class does NOT include any Unity-specific methods like `Start()` or `Update()`, suggesting it's meant to *be used in conjunction with other scripts* that **handle game logic or rendering loops**.

### 2. Class Definition: `DrawLines` 
Right Click in the assets, create a C# script and rename it `DrawLines` that *inherits from* `MonoBehaviour`, which is the base class from which every Unity script derives.

```csharp
public class DrawLines : MonoBehaviour
{
    Coords point = new Coords(10, 20);
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(point.ToString()); 
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

```
In `Hierarchy` create an Empty GameObject and call it `PointManager`
- Drag `DrawLines` and drop onto `PointManager` and Click `Play`


![](https://files.mdnice.com/user/1474/ead1302a-7f3e-435e-9578-96f7ffd07559.png)

#### 2.1. Fields of Coords Variables
These are instances of the `Coords` class, representing *various points in a 2D space*. 
- These points are used to *define the start and end points of lines* and individual points to be drawn. 
- The array *leo* contains *multiple Coords objects*.

```csharp
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
```

#### 2.2. Method: `Start()`
Inside `Start()` method, *lines and points are drawn* using the *Coords.DrawLine()* and *Coords.DrawPoint()* static methods from the `Coords` class.
- `Coords.DrawLine()` is used to *draw lines representing the X and Y axes* of a graph, and also to *connect the points in the `leo` array in a specific order*.
- `Coords.DrawPoint()` is used to draw points at the coordinates specified in the leo array.
- The points and lines are given specific colors and widths.

```csharp
    // Start is called before the first frame update
    void Start()
    {
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
```

![](https://files.mdnice.com/user/1474/60941ec5-b3c5-40c2-888c-8fd9095cd460.png)


### 3. Class Definition: `DrawGridGraph`

Create a C# script `DrawGridGraph` to create a grid graph in the Unity.

#### 3.1. Public Fields

- `size`: Represents the size of each
- `xmax` and `ymax`: Define the maximum extents of the grid along the x and y axes, respectively.

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGridGraph : MonoBehaviour
{
    public int size = 28; // Size of each grid cell
    public int xmax = 200; // Maximum extent of the grid along the x-axis
    public int ymax = 200; // Maximum extent of the grid along the y-axis
    
    // Coords objects for the start and end points of the X and Y axes
```

#### 3.2. Private Fields
Define Instances of the `Coords` class for representing the start and end points of the X and Y axes.
```csharp
    // Coords objects for the start and end points of the X and Y axes
    Coords startPointXAxis;
    Coords endPointXAxis;
    Coords startPointYAxis;
    Coords endPointYAxis;

    // Start is called before the first frame update
```
#### 3.3. Method: `Start()`
- Initializes the `Coords` objects for the axes using the *xmax* and *ymax* values. 
  - This sets up the X-axis from -xmax to xmax and the Y-axis from -ymax to ymax.
  
```csharp
   // Start is called before the first frame update
    void Start()
    {
        // Initialize the start and end points of the axes
        startPointXAxis = new Coords(-xmax, 0);
        endPointXAxis = new Coords(xmax, 0);
        startPointYAxis = new Coords(0, -ymax);
        endPointYAxis = new Coords(0, ymax);

        // Draw the X and Y axes in red and green colors, respectively
```
  
- Draws the X and Y axes using `Coords.DrawLine()`, colored red and green, respectively.

```csharp
       // Draw the X and Y axes in red and green colors, respectively
        Coords.DrawLine(startPointXAxis, endPointXAxis, 1, Color.red);
        Coords.DrawLine(startPointYAxis, endPointYAxis, 1, Color.green);

        // Calculate the horizontal offset for grid lines based on the grid size
```

- Calculates *xoffset* and *yoffset* as the distances between the grid lines along the X and Y axes.

```csharp
        // Calculate the horizontal offset for grid lines based on the grid size
        int xoffset = (int)(xmax / (float)size);

        // Calculate the vertical offset for grid lines based on the grid size
        int yoffset = (int)(ymax / (float)size);

        // Draw vertical grid lines at intervals defined by the grid size
```


- Uses two `for` loops to *draw vertical and horizontal* grid lines:
  - The first loop iterates over the x-axis, drawing vertical lines at intervals defined by `size`.
  - The second loop does the same along the y-axis, drawing horizontal lines.
  
```csharp
        // Draw vertical grid lines at intervals defined by the grid size
        for (int x = -xoffset * size; x <= xoffset * size; x += size){
            Coords.DrawLine(new Coords(x, -ymax), new Coords(x, ymax), 0.5f, Color.white);
        }

        // Draw horizontal grid lines at intervals defined by the grid size
        for (int y = -yoffset * size; y <= yoffset * size; y += size){
            Coords.DrawLine(new Coords(-xmax, y), new Coords(xmax, y), 0.5f, Color.white);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Empty - can be used for updates that need to occur every frame
    }
}
```

![](https://files.mdnice.com/user/1474/9e6d6308-db63-494a-8166-8f10b68105e0.png)


#### Resources:
[1] https://www.udemy.com/course/games_mathematics/
