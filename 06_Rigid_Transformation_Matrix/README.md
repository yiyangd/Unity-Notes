## Note 06 - Rigid Transformation Matrix



### 1. Matrix
Create a new Unity Project, import `Coords`, `HolisticMath`, `Line` and `Plane` Scripts

#### 1.1. Create a Script: `Matrix`

#### 1.2. Create a Script: `CreateMatrix`

In Unity Hierarchy, create an Empty GameObject - `MatrixManager`, drag the script to it.

#### 1.3. Scene Setup


Create 3 `Cube` Objects - `X`,`Y`,`Z` at the position `(0,0,0)`, and modify their scales:
- `X`: `(10,0.1,0.1)`
- `Y`: `(0.1,10,0.1)`
- `Z`: `(0.1,0.1,10)`

![](https://files.mdnice.com/user/1474/ac42cb32-6570-4ce3-9571-990beaa68831.png)

Create 8 `Sphere` Objects with the position `(0,0,0)`, `(0,0,1)`, `(0,1,0)`, `(0,1,1)`, `(1,0,0)`, `(1,0,1)`, `(1,1,0)`, `(1,1,1)`


![](https://files.mdnice.com/user/1474/3e939a23-1c25-4a9d-8c56-971cce2eb3b8.png)

### 2. Translation

#### 2.1. Add Method - `Translation` in `HolisticMath`
The general form of a translation matrix T, which translates a point by a vector $\vec{v}$ along the X, Y, and Z axes respectively, is:

$$
T = \begin{bmatrix}
1 & 0 & 0 & vx \\
0 & 1 & 0 & vy \\
0 & 0 & 1 & vz \\
0 & 0 & 0 & 1 
\end{bmatrix}
$$

- In this matrix, the *last column $(vx, vy, vz, 1)$* defines the **translation vector**, and the *diagonal elements $(1, 1, 1)$* represent the *scale factor* for each axis (**unchanged** in the case of **pure translation**).
- To apply this translation to a point $P = (Px, Py, Pz)$ in homogeneous coordinates (which is represented as $P = (Px, Py, Pz, 1)$, we multiply the translation matrix T by the point's column vector to get the translated point:

$$
P' = TP = \begin{bmatrix}
1 & 0 & 0 & vx \\
0 & 1 & 0 & vy \\
0 & 0 & 1 & vz \\
0 & 0 & 0 & 1 
\end{bmatrix}
\begin{bmatrix}
Px \\
Py \\
Pz \\
1 
\end{bmatrix}
$$

```csharp
static public Coords Translate(Coords position, Coords vector)
{
    float[] translateValues = {1, 0, 0, vector.x, 
                               0, 1, 0, vector.y,
                               0, 0, 1, vector.z,
                               0, 0, 0, 1};
    Matrix translateMatrix = new Matrix(4, 4, translateValues);
    Matrix pos = new Matrix(4, 1, position.AsFloats());

    Matrix result = translateMatrix * pos;
    return result.AsCoords();
}
```

#### 2.2. Create a new Script - `Transformation`
```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformation : MonoBehaviour
{

    public GameObject[] points;
    public Vector3 translation;
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject p in points)
        {
            Coords position = new Coords(p.transform.position, 1);

            p.transform.position = HolisticMath.Translate(position, 
                                                          new Coords(new Vector3(translation.x, translation.y, translation.z),0)
                                                          ).ToVector();


        }
        
    }
    void Update()
    {
    }
}
```

Create an Empty GameObject - `TransformationManager`, drag the script to it, input the `translation vector = (3, -1, -1)` and press play:

![](https://files.mdnice.com/user/1474/7c1299ab-502d-4f75-8f8a-421b90414870.gif)

### 2. Scaling

#### 2.1.

Scaling matrix scales a point by different factors  $S_x$, $S_y$, and $S_z$ along the X, Y, and Z axes::

$$
S = \begin{bmatrix}
S_x & 0 & 0 & 0 \\
0 & S_y & 0 & 0 \\
0 & 0 & S_z & 0 \\
0 & 0 & 0 & 1 
\end{bmatrix}
$$

- To apply this scaling to a point $P = (Px, Py, Pz, 1)$ in homogeneous coordinates, we multiply the scaling matrix $S$ by the point's column vector:

$$
P' = S \times P 
= \begin{bmatrix}
S_x & 0 & 0 & 0 \\
0 & S_y & 0 & 0 \\
0 & 0 & S_z & 0 \\
0 & 0 & 0 & 1 
\end{bmatrix}
\begin{bmatrix}
Px \\
Py \\
Pz \\
1 
\end{bmatrix}
$$

```csharp
static public Coords Scale(Coords position, float scaleX, float scaleY, float scaleZ)
{
    float[] scaleValues = {scaleX, 0, 0, 0, 
                               0, scaleY, 0, 0,
                               0, 0, scaleZ, 0,
                               0, 0, 0, 1};
    Matrix scaleMatrix = new Matrix(4, 4, scaleValues);
    Matrix pos = new Matrix(4, 1, position.AsFloats());

    Matrix result = scaleMatrix * pos;
    return result.AsCoords();
}
```

#### 2.2. Update `Transformation` Script

```csharp
public class Transformation : MonoBehaviour
{

    public GameObject[] points;
    public float angle;
    public Vector3 translation;
    public Vector3 scaling;
    public GameObject centre;
    //Start is called before the first frame update
    void Start()
    {
        Vector3 centrePoint = new Vector3(centre.transform.position.x, 
                                            centre.transform.position.y, 
                                            centre.transform.position.z);
        
        foreach(GameObject p in points)
        {

            
            Coords position = new Coords(p.transform.position, 1);

            // p.transform.position = HolisticMath.Translate(position, 
            //                                               new Coords(new Vector3(translation.x, translation.y, translation.z),0)
            //                                              ).ToVector();

            // translate centre to origin
            position = HolisticMath.Translate(position,
                                              new Coords(new Vector3(-centrePoint.x, -centrePoint.y, -centrePoint.z), 0));
            position = HolisticMath.Scale(position, scaling.x, scaling.y, scaling.z);
            p.transform.position = HolisticMath.Translate(position,
                                                          new Coords(new Vector3(centrePoint.x, centrePoint.y, centrePoint.z), 0)).ToVector();

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
```

![](https://files.mdnice.com/user/1474/ba40382e-c704-42f9-b37a-c54ea683d746.gif)



### 3. Rotation
#### 3.1. Add a `Rotate()` Method in `HolisticMath`
Rotating a point in 3D space around the X, Y, and Z axes by an angle $\theta$ are given:
   
$$
   R_x(\theta) = \begin{bmatrix}
   1 & 0 & 0 & 0 \\
   0 & \cos(\theta) & -\sin(\theta) & 0 \\
   0 & \sin(\theta) & \cos(\theta) & 0 \\
   0 & 0 & 0 & 1 
   \end{bmatrix}
$$

$$
   R_y(\theta) = \begin{bmatrix}
   \cos(\theta) & 0 & \sin(\theta) & 0 \\
   0 & 1 & 0 & 0 \\
   -\sin(\theta) & 0 & \cos(\theta) & 0 \\
   0 & 0 & 0 & 1 
   \end{bmatrix}
$$

$$
   R_z(\theta) = \begin{bmatrix}
   \cos(\theta) & -\sin(\theta) & 0 & 0 \\
   \sin(\theta) & \cos(\theta) & 0 & 0 \\
   0 & 0 & 1 & 0 \\
   0 & 0 & 0 & 1 
   \end{bmatrix}
$$


```csharp
static public Coords Rotate(Coords position, float angleX, bool clockwiseX,
                                             float angleY, bool clockwiseY,
                                             float angleZ, bool clockwiseZ)
{
    if (clockwiseX)
        angleX = 2 * Mathf.PI - angleX;
    if (clockwiseY)
        angleY = 2 * Mathf.PI - angleY;
    if (clockwiseZ)
        angleZ = 2 * Mathf.PI - angleZ;
    // Rx
    float[] xrollValues = {1,0,0,0,
                           0,Mathf.Cos(angleX),-Mathf.Sin(angleX),0,
                           0,Mathf.Sin(angleX), Mathf.Cos(angleX),0,
                           0,0,0,1};
    Matrix XRoll = new Matrix(4, 4, xrollValues);
    // Ry
    float[] yrollValues = {Mathf.Cos(angleY),0,Mathf.Sin(angleY),0,
                           0,1,0,0,
                           -Mathf.Sin(angleY),0,Mathf.Cos(angleY),0,
                           0,0,0,1};
    Matrix YRoll = new Matrix(4, 4, yrollValues);
    // Rz
    float[] zrollValues = {Mathf.Cos(angleZ),-Mathf.Sin(angleZ),0,0,
                           Mathf.Sin(angleZ),Mathf.Cos(angleZ),0,0,
                           0,0,1,0,
                           0,0,0,1};
    Matrix ZRoll = new Matrix(4, 4, zrollValues);

    Matrix pos = new Matrix(4, 1, position.AsFloats());

    Matrix result = ZRoll * YRoll * XRoll * pos;

    return result.AsCoords();
    }
```
