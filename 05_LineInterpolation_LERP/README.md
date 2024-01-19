## GameDev 05 - Line-Plane Hits and Reflections in 3D
> 此篇笔记是**『GameDev笔记』**系列的第5篇，记录了`Line`, `Plane`等脚本的定义和创建，在Unity3D中实现球体沿着直线线、线段或射线与平面之间通过线性插值（Lerp）进行平滑移动，碰撞与反弹的动画效果。

### 1. Animate a Sphere Between Two Cubes by Line Interpolation(Lerp)

#### 1.0. Scene Setup and Objective
Begin by creating a new project in Unity3D, 
- Place two `Cube` objects and a `Sphere` object *at distinct positions* within the scene. 

![](https://files.mdnice.com/user/1474/4ba04d58-8de2-4a7b-adce-0264c128a3ab.png)

- our **objective** is to develop scripts to *smoothly transition the sphere* from the position of the first cube to the second *when the play button is pressed*.


#### 1.1. Create a Script: `Line`

![](https://files.mdnice.com/user/1474/c311e7c9-2aad-4450-a333-c2d5d498d4b0.png)
Lines, Line Segments and Rays are essential concepts to grasp in the areas of collisions, animations, and physics.
- An enumeration (`LINETYPE`) in Class Definition is to specify the type of line
```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line
{
    // Start and End Points defining the line
    Coords A;
    Coords B;
    // Direction vector from A to B
    Coords v;

    // Enumeration to define the type of the line
    public enum LINETYPE {LINE, SEGMENT, RAY};
    LINETYPE type;

    // Constructor for a line defined by two points and a type (line, segment, ray)
    public Line(Coords _A, Coords _B, LINETYPE _type)
    {
        A = _A;
        B = _B;
        type = _type;
        // Calculate the direction vector from A to B
        v = new Coords(B.x - A.x, B.y - A.y, B.z - A.z); 
    }

    // Constructor for a line defined by a starting point and a direction vector
    public Line(Coords _A, Coords _v)
    {
        A = _A;
        // Calculate the end point B using the direction vector
        B = _A + _v;
        v = _v;
        // Default type for this constructor is a segment
        type = LINETYPE.SEGMENT;
    }

    // Method for linear interpolation along the line
```


- **Moving the sphere** between two cubes using time `t` and a parametric equation is called **Linear Interpolation** (Lerp[1])

```csharp
   // Method for linear interpolation along the line
    public Coords Lerp(float t)
    {
        // Clamp the interpolation parameter for a line segment
        if (type == LINETYPE.SEGMENT)
            t = Mathf.Clamp(t, 0, 1);
        // For a ray, ensure t is not negative
        else if (type == LINETYPE.RAY && t < 0)
            t = 0;
        // Calculate the interpolated point
        float xt = A.x + v.x * t;
        float yt = A.y + v.y * t;
        float zt = A.z + v.z * t;
        // Return the new Coords instance of the interpolated point
        return new Coords(xt, yt, zt);
    }
```
#### 1.2. Create a Script: `Move`
- Define public variables to assign the *start and end positions* in the Unity Editor.

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{

    public Transform start;
    public Transform end;
    Line line;
    // Start is called before the first frame update
```
- Inside `Start()`, a new `Line` object is instantiated *using the positions of the start and end transforms*. 

```csharp
   // Start is called before the first frame update
    void Start()
    {
        line = new Line(new Coords(start.position),
                        new Coords(end.position), 
                        Line.LINETYPE.SEGMENT);
    }

    // Update is called once per frame
```

- Use `Line.Lerp()` in `Update()` to smoothly transition the sphere from the position of the first cube to the second when the play button is pressed.
  - `Time.time` gives the time since the start of the game, and multiplying it by `0.5f` slows down the movement. 
  
```csharp
    // Update is called once per frame
    void Update()
    {
        this.transform.position = line.Lerp(Time.time * 0.5f).ToVector();
    }
}
```

- Drag `Move` Script to `Sphere`, and Drag two `Cube` objects to `Start` and `End` field in `Move` Script

![](https://files.mdnice.com/user/1474/96a8931b-3253-42a3-9fed-a9b2ac343346.png)

- Press the **Play** Button

![](https://files.mdnice.com/user/1474/530fed79-aa59-42aa-b336-6371bd288827.gif)


### 2. Find Line-Line Intersection



Given two Lines:
$$L(t) = A + \vec{v} t$$
$$L(s) = B + \vec{u}s$$

These lines will intersect where two equations are equal
$$L(t) = L(s)$$
$$A + \vec{v}t = B + \vec{u}s$$
$$\vec{v}t = B - A + \vec{u}s$$

Let $\vec{c} = B - A$, so
$$\vec{v}t = \vec{c} + \vec{u}s$$

Multiply both sides of the euqation with $\vec{u}^{\perp}$ and $\vec{v}^{\perp}$ to get t:
$$\vec{u}^{\perp} \cdot \vec{v}t = \vec{u}^{\perp} \cdot \vec{c} + \color{red}\vec{u}^{\perp} \cdot \vec{u}\color{black}s$$
$$\color{red}\vec{v}^{\perp} \cdot \vec{v}\color{black}t = \vec{v}^{\perp} \cdot \vec{c} + \vec{v}^{\perp} \cdot \vec{u}s$$

Since $\color{red} \vec{v}^{\perp} \cdot \vec{v} = \vec{u}^{\perp} \cdot \vec{u} = 0$, then

$$\vec{u}^{\perp} \cdot \vec{v}t = \vec{u}^{\perp} \cdot \vec{c} + 0$$
$$- \vec{v}^{\perp} \cdot \vec{u}s = \vec{v}^{\perp} \cdot \vec{c} - 0 $$
So
$$t = \frac{\vec{u}^{\perp} \cdot \vec{c}}{\vec{u}^{\perp} \cdot \vec{v}}$$
$$s = \frac{\vec{v}^{\perp} \cdot \vec{c}}{\color{red}- \vec{v}^{\perp} \cdot \vec{u}}
    = \frac{\vec{v}^{\perp} \cdot \vec{c}}{\color{red}\vec{u}^{\perp} \cdot \vec{v}}
$$

![](https://files.mdnice.com/user/1474/7fc4d90d-8e81-477a-8855-ec5c0b8ee155.png)
- $A = (1,2), \vec{v} = (1,1), B = (2,7), \vec{u} = (1,-1)$
- $\vec{c} = B - A = (1,5), \vec{v}^{\perp} = (-1,1), \vec{u}^{\perp} = (1, 1)$

$$t = \frac{\vec{u}^{\perp} \cdot \vec{c}}{\vec{u}^{\perp} \cdot \vec{v}}
    = \frac{(1, 1) \cdot (1,5)}{(1, 1) \cdot (1,1)} = \frac{6}{2} = 3
$$
$$s = \frac{\vec{v}^{\perp} \cdot \vec{c}}{\vec{u}^{\perp} \cdot \vec{v}}
    = \frac{(-1,1) \cdot (1,5)}{(1, 1) \cdot (1,1)} = \frac{4}{2} = 2
$$

$$A + \vec{v}t = B + \vec{u}s = (4,5)$$
#### 2.1. Add a Method: `Prep()` in `Coords`
```csharp
    static public Coords Perp(Coords v)
    {
        // (v.x, v.y)·(-v.y, v.x) = 0
        return new Coords(-v.y, v.x);
    }
```

#### 2.2. Add a Method: `IntersectsAt()` in `Line`
```csharp
    public float IntersectsAt(Line l)
    {
        // Check if the lines are parallel (dot product of perpendicular vectors is 0)
        if(HolisticMath.Dot(Coords.Perp(l.v),v) == 0) 
        {
            // Return NaN to indicate NO intersection (lines are parallel)
            return float.NaN; 
        }
        // Calculate the intersection parameter 't'
        Coords c = l.A - this.A;
        float t = HolisticMath.Dot(Coords.Perp(l.v), c)/HolisticMath.Dot(Coords.Perp(l.v), v);
        // Check if 't' is outside the valid range for a line segment
        if((t < 0 || t > 1) && type == LINETYPE.SEGMENT) 
        {
            return float.NaN;
        }

        return t;
    }
```
#### 2.3. Create a Script: `CreateLines`
```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLines : MonoBehaviour
{
    Line L1;
    Line L2;
    // Start is called before the first frame update
    void Start()
    {
        L1 = new Line(new Coords(-100,0,0), new Coords(200, 150, 0));
        L1.Draw(1, Color.green);

        L2 = new Line(new Coords(0, -100, 0), new Coords(0, 200, 0));
        L2.Draw(1, Color.red);
        // Calculate the intersection points of L1 with L2 and L2 with L1
        float intersectT = L1.IntersectsAt(L2);
        float intersectS = L2.IntersectsAt(L1);
        // Check if the intersection points are valid (not NaN)
        if (intersectT == intersectT && intersectS == intersectS)
        {
            // Create a sphere at the intersection point
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = L1.Lerp(intersectT).ToVector();
        }
    }
    // Update is called once per frame
    void Update()
    {     
    }
}
```

- Create an empty `GameObject` - *`LineCreator`* and drag the script to it
- Press `Play`


![](https://files.mdnice.com/user/1474/05e09a5b-5790-42cf-8448-da554f3672a1.png)

- Another example:
  - `L1 = new Line(new Coords(-100,0,0), new Coords(20, 50, 0));`
  - `L2 = new Line(new Coords(-100, 10, 0), new Coords(0, 50, 0));`
  
![](https://files.mdnice.com/user/1474/a23a4960-ce2a-4270-bdb7-303bf3350652.png)

- if we ONLY check  `intersectT == intersectT`, without `intersectS == intersectS`:


![](https://files.mdnice.com/user/1474/44942394-9e19-4939-898f-56ad73df8436.png)

- the intersection sphere is at `t=0` point of `L1`, but `s = NaN` in this case

### 3. Ball to Wall Animation
The goal is to use **`Lerp()`** from Section 1 and **`IntersectsAt()`** from Section 2 to animate a tennis ball starting from the beginning of a line and moving along this `trajectory` until it reaches a `wall`, where it will stop.

#### 3.1. Scene Setup
Create a Empty GameObject and its Component Script `HitWall`:

```csharp
public class HitWall : MonoBehaviour
{
    // Lines to represent the wall and the path of the ball
    Line wall;
    Line ballPath;
    // The GameObject for the tennis ball
    public GameObject ball;
    // Line to represent the ball's trajectory towards the wall
    Line trajectory;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the wall line and draw it in blue color
        wall = new Line(new Coords(5, -2, 0), new Coords(0, 5, 0));
        wall.Draw(1, Color.blue);
        // Initialize the ball's path line and draw it in yellow color
        ballPath = new Line(new Coords(-6, 0, 0), new Coords(100, 0, 0));
        ballPath.Draw(0.1f, Color.yellow);
        // Set the initial position of the ball to the start of the ball's path
        ball.transform.position = ballPath.A.ToVector();
        // Calculate the intersection point of the ball's path with the wall
```

Drag a Tennis Object to Scene and `Ball` field in `HitWall`, press `Play`


![](https://files.mdnice.com/user/1474/252aae07-77ef-47e0-85ac-85b4df122d42.png)

#### 3.2. Calculate the Hit Point and Trajectory

```csharp
        // Calculate the intersection point of the ball's path with the wall
        float t = ballPath.IntersectsAt(wall);
        float s = wall.IntersectsAt(ballPath);
        // Check if the intersection is valid (not NaN)
        if(t == t && s == s)
        {
            // Create a trajectory from the ball's starting point to the intersection point
            trajectory = new Line(ballPath.A, ballPath.Lerp(t), Line.LINETYPE.SEGMENT);
        }
    }
    // Update is called once per frame
```

#### 3.3. Update the Position of Ball to Hit Wall
```csharp
    // Update is called once per frame
    void Update()
    {
        // Update the ball's position along the trajectory based on time
        // This creates the animation effect of the ball moving
        ball.transform.position = trajectory.Lerp(Time.time).ToVector();
    }
}
```

![](https://files.mdnice.com/user/1474/297ae259-0920-4948-8ac5-cc03cc09e5bb.gif)


### 4. Use 3 Cubes to create a Plane

Place Three `Cube` objects *at distinct positions* within the scene. 
- create a `Material` and change the color to *red*, drag it to three cubes


![](https://files.mdnice.com/user/1474/fe93ace8-89b1-4eb2-9ffa-ba779139bafc.png)



#### 4.1. Create a Script: `Plane`
**Review:** Two Independent Vectors span a Plane (Linear Algebra Note 02)

![](https://files.mdnice.com/user/1474/93cdf8a6-4c8a-43cf-b44a-45b8a26a37f8.gif)

Take **two vectors ( $\vec{u}, \vec{v}$ ), in 3-D space**, that are not pointing in the same direction, the **set of all possible vectors whose tips sit on a 2D plane** is the **span of those two vectors**  
- the **span of most pairs of 2-D vectors ( $\vec{u}, \vec{v}$ )**   is **ALL vectors (points) of 2-D Plane**,   

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane
{
    Coords A;
    Coords B;
    Coords C;
    Coords u;
    Coords v;

    public Plane(Coords _A, Coords _B, Coords _C)
    {
        A = _A;
        B = _B;
        C = _C;
        v = B - A;
        u = C - A;
    }

    public Plane(Coords _A, Vector3 V, Vector3 U)
    {
        A = _A;
        v = new Coords(V.x, V.y, V.z);
        u = new Coords(U.x, U.y, U.z);
    }
    // Method for linear interpolation on the plane
    public Coords Lerp(float s, float t)
    {
        // Calculate the interpolated position on the plane
        // It's based on scaling vectors v and u by s and t, then adding to point A
        float xst = A.x + v.x * s + u.x * t;
        float yst = A.y + v.y * s + u.y * t;
        float zst = A.z + v.z * s + u.z * t;

        return new Coords(xst, yst, zst);
    }
}
```


#### 4.2. Create a Script: `CreatePlane`
This script uses the `Plane` class to **create a grid of spheres on a plane** defined by *three cubes*. 

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlane : MonoBehaviour
{
    public Transform A;
    public Transform B;
    public Transform C;
    
    Plane plane;
    // Start is called before the first frame update
    void Start()
    {
        plane = new Plane(new Coords(A.position),
                          new Coords(B.position),
                          new Coords(C.position));

        // Nested loops to iterate over the plane within a range of [0, 1] for both s and t
        for(float s = 0; s < 1; s += 0.1f)
        {
            for(float t = 0; t < 1; t += 0.1f)
            {
                // Creating a sphere at each iteration
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                // Setting the position of the sphere based on the interpolated point on the plane
                // Lerp function is used to find the point at (s, t) on the plane
                sphere.transform.position = plane.Lerp(s,t).ToVector();
            }
        }
    }
    void Update()
    {
        // No actions are performed in each frame update in this script
    }
}
```

- Drag the Script to `PlaneCreator` and Drag three cubes to `A`, `B` and `C`

![](https://files.mdnice.com/user/1474/429c67ad-deb4-440e-a65a-a1e2197670d5.png)

- Press the **Play** Button

![](https://files.mdnice.com/user/1474/669eed6f-381d-4930-8d78-4d11a281c993.gif)

### 5. Find Line-Plane Intersection
Given a Line and a Plane:
$$L(t) = A + \vec{w} t$$
$$ P(a,e) = B + \vec{v} a+ \vec{u}e$$

with the **Normalized Normal Vector** of Plane:

$$\vec{n} = \frac{\vec{v} \times \vec{u}}{\|\vec{v} \times \vec{u}\|}$$

Let the Hit Point in Plane be $H$, we have

$$\vec{n} \cdot (H - B) = 0$$
$$H = A + \vec{w} \color{red}t$$

Then,

$$\vec{n} \cdot (A + \vec{w}\color{red}t\\- B) = 0$$
$$\vec{n} \cdot (A - B) +  \vec{n} \cdot \vec{w}\color{red}t\\ = 0$$
$$\color{red}t\\ = \frac{-\vec{n} \cdot (A - B)}{ \vec{n} \cdot \vec{w}}$$


![](https://files.mdnice.com/user/1474/6188c5eb-ce1e-4606-a1ba-db26096ac606.png)

$$\color{red}t\\ = \frac{-(0,1,0) \cdot (5,25,35)}{ (0,1,0) \cdot (0,-57,1)} = \frac{25}{57} = 0.44$$

$$H = A + \vec{w} \color{red}t\\= (100, 25, 35) + 0.44(0, -57, 1)$$
$$ = (100,0,35.44)$$

#### 5.1. Add another `IntersectsAt()`  in `Line`

```csharp
    public float IntersectsAt(Plane p)
    {
        // Calculate the normal vector of the plane by crossing its two direction vectors
        Coords normal = HolisticMath.Cross(p.u, p.v);
        // Check if the line is parallel to the plane (dot product is zero)
        if(HolisticMath.Dot(normal,v) == 0)
            return float.NaN; // Return NaN (Not a Number) indicating NO intersection
        // Calculate the intersection parameter 't' using the plane's normal and the line's direction
        float t = HolisticMath.Dot(normal, p.A - A) / HolisticMath.Dot(normal, v);
        return t; // Return the intersection parameter 't'
    }
```

#### 5.2. Create a Script: `CreatePlaneHit`
```csharp
public class CreatePlaneHit : MonoBehaviour
{
    // Public Transforms to define the points of the plane (ABC)
    // and the ray (DE) in 3D space
    public Transform A, B, C, D, E;

    // Private fields for the plane and the ray
    Plane plane;
    Line L1;
    
    // Start is called before the first frame update
    void Start()
    {
        // Initialize the plane with three points (A, B, C)
        plane = new Plane(new Coords(A.position), 
                          new Coords(B.position),
                          new Coords(C.position));
        // Initialize the ray (L1) with two points (D, E) and set it as a ray type
        L1 = new Line(new Coords(D.position), new Coords(E.position), Line.LINETYPE.RAY);
        // Draw the ray in green color
        L1.Draw(1, Color.green);
        // Create a grid of spheres to visualize the plane
        for(float s = 0; s <= 1; s += 0.1f)
        {
            for(float t = 0; t <= 1; t += 0.1f)
            {
                // Create a sphere at each interpolated point on the plane
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.position = plane.Lerp(s, t).ToVector();
            }

        }
        // Calculate the intersection point of the ray with the plane
        float interceptT = L1.IntersectsAt(plane);
        // Check if the intersection is valid (not NaN)
        if(interceptT == interceptT)
        {
            // Create a cube at the intersection point
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = L1.Lerp(interceptT).ToVector();
            // Log the intersection parameter and position
            Debug.Log("t:"+interceptT + " " + cube.transform.position);
        }
    }
}
```

- Create 5 `Cube` Objects and an Empty GameObject - `CreatePlaneHit`, 
- drag the script to `CreatePlaneHit`, and 5 `Cube` to the corresponding fields


![](https://files.mdnice.com/user/1474/83c5f7f9-129d-4e6c-91a6-45fff9a0b7a1.png)

- Press `Play`

![](https://files.mdnice.com/user/1474/c3818ba2-ecfc-442f-ae6c-fa2e37deb402.gif)


![](https://files.mdnice.com/user/1474/f77c429c-89cf-4074-b5da-73115042b035.png)


### 6. Line-Line Reflections and Ball Bounce 
Reflection is a very common calculation in physics and lighting systems
- according to the laws of physics, the *angle of incidence* equals *the angle of reflection*
  - these angles measure between the incoming and outgoing vectors and the normal to the surface
  
![](https://files.mdnice.com/user/1474/48bb28ea-3bef-422c-b7c4-a2ce4d517db9.png) 

- Let $\vec{v}$ be the incoming vector , $\vec{n}$  be normal vector to the plane  and $\vec{r}$ be the outgoing vector that we need to calculate by

$$\vec{r} = \vec{v} - 2(\vec{v}\cdot \vec{n})\vec{n}$$



#### 6.1. Add a `Reflect` method in `Line`
```csharp
    public Coords Reflect(Coords normal)
    {
        // Incoming Vector 
        Coords vnorm = v.GetNormal(); 
        // Normal Vector
        Coords n = normal.GetNormal();

        // Calculate the Dot Product
        float vn = HolisticMath.Dot(vnorm, n);
        // incoming and normal vectors are parallel
        // outgoing vector = incoming vector
        if(vn == 0) return v; 

        // Calculate Outgoing Vector
        Coords r = vnorm -  2 * vn * n;
        return r;
    }
```

#### 6.2. Add `GetNormal` in `Coords`
```csharp
    public Coords GetNormal()
    {
        float magnitude = HolisticMath.Distance(new Coords(0,0,0), new Coords(x, y, z));
        return new Coords(x/magnitude, y/magnitude, z/magnitude);
    }
```
#### 6.3. Overload *`+-*/`* operator in `Coords`
```csharp
    static public Coords operator+ (Coords a, Coords b)
    {
        Coords c = new Coords(a.x + b.x, a.y + b.y, a.z + b.z);
        return c;
    }

    static public Coords operator- (Coords a, Coords b)
    {
        Coords c = new Coords(a.x - b.x, a.y - b.y, a.z - b.z);
        return c;
    }
    static public Coords operator* (float b, Coords a)
    {
        Coords c = new Coords(a.x * b, a.y * b, a.z * b);
        return c;
    }
    static public Coords operator/ (Coords a, float b)
    {
        Coords c = new Coords(a.x / b, a.y - b, a.z - b);
        return c;
    }
```

#### 6.4. Update `WallHit` Script (from 3.3)
```csharp
    void Update()
    {
        if(Time.time <= 1) // ball hits the wall when Time.time is 1
            ball.transform.position = trajectory.Lerp(Time.time).ToVector();
        else
        {
            ball.transform.position += trajectory.Reflect(Coords.Perp(wall.v)).ToVector() * Time.deltaTime * 5;
        }
    }
```

![](https://files.mdnice.com/user/1474/8b818723-6ce3-4d75-a0d0-2c67c7e12653.gif)

- Note: In Section 6, `Wall` is a `Line`

### 7. Line-Plane Reflections
```csharp
public class CreateSolidWall : MonoBehaviour
{
    public Transform A,B,C,D,E;
    public GameObject ball;

    Plane wall;
    Line ballPath;
    Line trajectory;
    // Start is called before the first frame update
    void Start()
    {
        wall = new Plane(new Coords(A.position),
                        new Coords(B.position),
                        new Coords(C.position));
        for(float s = 0; s <= 1; s += 0.05f)
        {
            for(float t = 0; t <= 1; t += 0.05f)
            {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.position = wall.Lerp(s, t).ToVector();
            }
        }

        ballPath = new Line(new Coords(D.position), new Coords(E.position), Line.LINETYPE.RAY);
        ballPath.Draw(0.1f, Color.green);
        ball.transform.position = ballPath.A.ToVector();

        //ball path wall intersection
        float interceptT = ballPath.IntersectsAt(wall);
        if(interceptT == interceptT)
        {
            trajectory = new Line(ballPath.A, ballPath.Lerp(interceptT), Line.LINETYPE.SEGMENT);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time <= 1) // ball hits the wall when Time.time is 1
            ball.transform.position = trajectory.Lerp(Time.time).ToVector();
        else
        {
            ball.transform.position += trajectory.Reflect(HolisticMath.Cross(wall.v,wall.u)).ToVector() * Time.deltaTime * 5;
        }
    }
}
```

![](https://files.mdnice.com/user/1474/aaa9d947-1a6e-40db-8a35-a742293fd65f.gif)


#### Resources:
[1] https://www.udemy.com/course/games_mathematics/  
[2] https://docs.unity3d.com/ScriptReference/Vector3.Lerp.html
