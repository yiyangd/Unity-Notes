## Unity 05 - Lines and Planes in 3D

### 1. Animating a Sphere Between Two Cubes by Line Interpolation(Lerp)


#### 1.1. Scene Setup and Objective
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
        return new Coords(-v.y, v.x);
    }
```

#### 2.2. Add a Method: `IntersectsAt()` in `Line`
```csharp

```
#### Create a Script: `CreateLines`

Create an empty `GameObject` - *LineCreator* and drag the script to it


### 2. Use 3 Cubes to create a Plane

Place Three `Cube` objects *at distinct positions* within the scene. 
- create a `Material` and change the color to *red*, drag it to three cubes


![](https://files.mdnice.com/user/1474/fe93ace8-89b1-4eb2-9ffa-ba779139bafc.png)



#### 2.1. Create a Script: `Plane`
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


#### 2.2. Create a Script: `CreatePlane`
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



### Reflections and Ball Bounce (49)
Reflection is a very common calculation in physics and lighting systems
- according to the laws of physics, the angle of incidence equals the angle of reflection
  - these angles measure between the incoming and outgoing vectors and the normal to the surface
  
![](https://files.mdnice.com/user/1474/48bb28ea-3bef-422c-b7c4-a2ce4d517db9.png)

#### Add `GetNormal` in Coords
#### 2.1. Overload `+` and `-` operator in `Coords`
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
```

#### Reflection in `Line`

![](https://files.mdnice.com/user/1474/30bb08fd-ddc8-4feb-8be0-991bf89544a6.gif)


#### Resources:
[1] https://docs.unity3d.com/ScriptReference/Vector3.Lerp.html
