## Unity 04 - Vectors Manipulation

> This note documents how to implement vector normalization, distance calculation, dot product, angle, cross product, and operations for vector rotation and translation in C#, as well as how to apply these vector manipulation methods to control the rotation and movement of objects in Unity.

### 1. Vector Operation
#### Create a Script: `HolisticMath`
This script provides a set of **static mathematical methods** for **vector manipulation** in a 2D or 3D space, 
- which are particularly useful for *controlling objects in Unity* 

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolisticMath
{   
    // Calculates the Euclidean distance between two points
```




#### 1.1. Method: `GetDistance()`
- *Purpose:* Calculates the distance between two points.
- *Implementation:* Uses the Pythagorean theorem to find the distance in 3D space.
- *Usage:* Determines the *distance* between the *tank* and the *fuel depot* to decide when to stop moving
$$\text{point 1} = (x_1, \ y_1, \ z_1)$$
$$\text{point 2} = (x_2, \ y_2, \ z_2)$$
$$Dist = \sqrt{(x_2-x_1)^2+(y_2-y_1)^2+(z_2-z_1)^2}$$

```csharp
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
```

#### 1.2. Method: `GetNormal()`
- *Purpose:* Normalizes a vector (makes its length 1).
- *Implementation:* Divides each component of the vector by its length.
- *Usage:* Used to *normalize the direction vector* from the *tank* to the *fuel* depot, ensuring the tank *moves at a consistent speed regardless of distance*.

$$\vec{v} = (v_x, \ v_y)$$
$$\hat{v} = (\frac{v_x}{\|v\|},  \ \frac{v_y}{\|v\|})$$

```csharp
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
```

#### 1.3. Method: `Dot()`
- *Purpose:* Computes the dot product of two vectors.
- *Implementation:* Multiplies corresponding components of the vectors and sums them up.

$$\vec{v}_1 = (x_1, \ y_1, \ z_1)$$
$$\vec{v}_2 = (x_2, \ y_2, \ z_2)$$
$$\vec{v}_1 \cdot \vec{v}_2 = x_1 \cdot x_2 + y_1 \cdot y_2 + z_1 \cdot z_2 $$


```csharp
    // Computes the dot product of two vectors
    static public float Dot(Coords vector1, Coords vector2)
    {
        // Multiply corresponding components and sum them
        return(vector1.x * vector2.x + vector1.y * vector2.y + vector1.z * vector2.z);
    }

    // Calculates the angle between two vectors (result in radians)
```

#### 1.4. Method: `Angle()`
- *Purpose:* Calculates the angle between two vectors (result in radians)
- *Implementation:* Uses the *dot product* and the *magnitudes of the vectors* to find the angle using the **arccosine function**. Returns the angle in radians.rotate the tank’s 'up' direction towards the fuel depot.

$$\vec{v}_1 \cdot \vec{v}_2=\|\vec{v}_1\|\|\vec{v}_2\|\cos\theta$$
$$\theta = \arccos{(\frac{\vec{v}_1 \cdot \vec{v}_2}{\|\vec{v}_1\| \cdot \|\vec{v}_2\|})}$$

```csharp
    // Calculates the angle between two vectors (result in radians)
    static public float Angle(Coords vector1, Coords vector2)
    {
        // Calculate the dot product divided by the product of the magnitudes of the vectors
        float dotDivide = Dot(vector1, vector2) 
                            / (Distance(new Coords(0,0,0), vector1) 
                               * Distance(new Coords(0,0,0), vector2));
        // Return the arccosine of the result
        return Mathf.Acos(dotDivide); // For degrees, multiply by 180/Mathf.PI
    }
    
    
    // Rotates a vector by a specified angle (in radians)
```

#### 1.5. Method: `Rotate()`
- *Purpose:* Rotates a vector by a given angle in radians, either clockwise or counter-clockwise (determined based on the cross product of two vectors).
- *Usage:* Rotate the tank’s `up` direction towards the fuel depot.

$$\text{Vector}\ \vec{v} = \begin{bmatrix}
x  \\
y
\end{bmatrix}
$$

$$\text{Rotated Vector} = \begin{bmatrix}
x \cdot \cos\theta - y \cdot \sin\theta \\
x \cdot \sin\theta + y \cdot \cos\theta 
\end{bmatrix}
$$

```csharp
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
```


![](https://files.mdnice.com/user/1474/b9a289cd-4bd8-4bc7-93ea-2da3fb953abf.png)
- Unity uses Left-Handed Coordinates System, so the positive rotations are going to go counter-clockwise around z-axis

![](https://files.mdnice.com/user/1474/2ed500ad-fa32-41e2-8923-d54d46a929fb.png)


#### 1.6. Method: `Cross()`
- *Purpose:* Computes the cross product of two vectors.

$$\vec{v}_1 = (x_1, \ y_1, \ z_1)$$
$$\vec{v}_2 = (x_2, \ y_2, \ z_2)$$
$$\vec{v}_1 \times \vec{v}_2 =
\begin{bmatrix}
y_1 \cdot z_2 - z_1 \cdot y_2 \\
z_1 \cdot x_2 - x_1 \cdot z_2\\
x_1 \cdot y_2 - y_1 \cdot x_2
\end{bmatrix}
$$

```csharp

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

```

- *Usage:* determine the direction of rotation (clockwise or counter-clockwise) for the `tank` to face the `fuel` depot.
  - if the **Fuel is to the Right of the Tank**, `z` component of cross product is **negative**
    - This implies that the shortest rotation to face the fuel is **clockwise**.
  - if the **Fuel is to the Left of the Tank**, `z` component of cross product is **positive**
    - This indicates that the shortest rotation to face the fuel is **counter-clockwise**.
    

![](https://files.mdnice.com/user/1474/eb95e4eb-dfce-4fb9-b63a-311a8aa38cd4.png)
    
    
#### 1.7. Method: `LookAt2D()`
- *Purpose:* Rotates the *forwardVector* so it points towards *focusPoint* from *position*
- *Usage:* Directly aligns the tank's facing direction towards the fuel depot by using `Angle()`, `Cross()` and `Rotate()` methods
```csharp
    // Adjusts a vector to point towards a specific target point
    static public Coords LookAt2D(Coords forwardVector, Coords position, Coords focusPoint)
    {
        // Calculate the direction from the position to the focus point
        Coords direction = new Coords(focusPoint.x - position.x, focusPoint.y - position.y, position.z);
        // Determine the angle to rotate the forward vector to align with the direction
        float angle = HolisticMath.Angle(forwardVector, direction);

        // Determine the direction of rotation (clockwise or counter-clockwise)
        // counter-clockwise if cross product is positive
        bool clockwise = false; 
        if(HolisticMath.Cross(forwardVector, direction).z < 0)
            clockwise = true;
        
        
        // Perform the rotation
        Coords newDir = HolisticMath.Rotate(forwardVector, angle, clockwise);
        return newDir;
    }
}
```

#### 1.8. Create a Script: `Autopilot`
The script's purpose is to make the tank **automatically rotate and move** towards a fuel depot when the game starts.
```csharp
using UnityEngine;
using System.Collections;

public class Autopilot : MonoBehaviour
{
    float speed = 5; // Speed of movement
    public GameObject fuel; // Reference to the fuel depot GameObject
    Vector3 direction; // Direction towards the fuel depot
    float stoppingDistance = 0.1f; // Minimum distance to stop from the fuel depot
    
    // Start is called before the first frame update
```

- `Start()` is called before the first frame update to *calculate the initial direction from the tank to the fuel depot* and *set the tank's 'up' direction to face the fuel depot* using `LookAt2D()`

```csharp
    // Start is called before the first frame update
    void Start()
    {
        // Calculate initial direction from the tank to the fuel depot
        direction = fuel.transform.position - this.transform.position;

        // Normalize the direction vector
        Coords dirNormal = HolisticMath.GetNormal(new Coords(direction));
        direction = dirNormal.ToVector();
        
        // Rotate the tank to face the fuel depot
        this.transform.up = HolisticMath.LookAt2D(new Coords(this.transform.up),
                                                  new Coords(this.transform.position),
                                                  new Coords(fuel.transform.position)).ToVector();
    }

    // Update is called once per frame
```

- `Update()` is called once per frame to move tank closer to the fuel depot. 
  - This is done by *adding the normalized direction vector*, scaled by the *speed* and *Time.deltaTime* (to *ensure smooth movement over time*) 
  -  It *stops* moving once it's *within the defined `stoppingDistance` from the depot*.
  
```csharp
    // Update is called once per frame
    void Update()
    {
        // Check if the distance to the fuel depot is greater than the stopping distance
        if(HolisticMath.Distance(new Coords(this.transform.position), new Coords(fuel.transform.position)) > stoppingDistance)
        {
            // Move the tank towards the fuel depot at the specified speed
            this.transform.position += direction * speed * Time.deltaTime;    
        }
    }
}
```
- Left Click `Tank` in `Hierarchy` and drag `Fuel` from `Hierarchy` to the `Fuel` field in `Autopilot(Script)`
### 2. Rotate and Translate the Tank by KeyBoard
Create a C# Script - `Drive` to allow the `Tank` to *move forward and backward* and to *rotate* based on **player input**:
- vertical input (e.g., `Up↑/Down↓ arrow` keys or `W/S` keys) determines the vehicle's forward and backward movement.
- horizontal input (e.g., `Left←/Right→ arrow` keys or `A/D` keys determines the vehicle's rotation in counter-clockwise and clockwise) 

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;

    void Update()
    {
        // Get vertical input (forward/backward) 
        // and calculate the distance to move
        float translation = Input.GetAxis("Vertical") * speed;
        
        // Get horizontal input (left/right)  
        // and calculate the rotation amount
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        // Make movement and rotation smooth and frame rate independent
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        
        // Update the vehicle's upward direction using custom rotation method
```
#### 2.1. Rotation
- The vehicle's upward direction (`transform.up`) is rotated using the `HolisticMath.Rotate` method. 
- This rotation is based on player input and is converted to radians (`Mathf.Deg2Rad`). 
- The `true` flag indicates clockwise rotation.
```csharp
        // Update the vehicle's upward direction using custom rotation method
        // This rotates the vehicle left or right based on player input
        // The rotation is converted to radians and performed clockwise
        transform.up = HolisticMath.Rotate(new Coords(transform.up), 
                                           rotation * Mathf.Deg2Rad, 
                                           true).ToVector();
        // Unity Built-in Rotation
        //transform.Rotate(0, 0, -rotation); 
        
        // Update the vehicle's position using custom translation method
```

#### 2.2. Translation
```csharp
        // Update the vehicle's position using custom translation method
        // This moves the vehicle forward or backward based on player input
        transform.position = HolisticMath.Translate(new Coords(transform.position), 
                                                    new Coords(transform.up),
                                                    new Coords(0, translation, 0)).ToVector();
        // transform.Translate(0, translation, 0);
    }
}
```

Define the `Translate()` method in the `HolisticMath` class that is designed to move an object **in a specific direction**, taking into account its **current facing direction**. 
- `position`: The *current position* of the object.
- `facing`: The *direction the object is facing*.
- `transVector`: The *direction and magnitude of the desired movement*.

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolisticMath
{
    // ...
    static public Coords Translate(Coords position, Coords facing, Coords transVector)
    {
        // Check if the movement vector is effectively zero (no movement)
        if(HolisticMath.Distance(new Coords(0,0,0), transVector) <= 0) 
            return position; // Return current position if no movement is required
        
        // Calculate the angle between the desired movement direction and the object's facing direction

```
- Calculates the angle between the *object's current facing direction* and the *desired movement direction*.

```csharp
        // Calculate the angle between the desired movement direction and the object's facing direction
        float angle = HolisticMath.Angle(transVector, facing);

        // Calculate the angle between the movement vector and a world reference vector (like 'up' in 2D)
```

- Calculates the angle between the *movement vector* and a *world reference vector* 
  - here,  (0,1,0), which could represent "north" or "up" in a 2D plane.

```csharp
        // Calculate the angle between the movement vector and a world reference vector (like 'up' in 2D)
        float worldAngle = HolisticMath.Angle(transVector, new Coords(0,1,0));

        // Determine whether the rotation to align the facing direction with the movement vector is clockwise
```

- Rotates the `transVector` by *the sum of `angle` and `worldAngle`*, in the direction determined by cross product (clockwise or counter-clockwise). 
- This *aligns the movement direction with the object's facing direction*.

```csharp
        // Determine whether the rotation to align the facing direction with the movement vector is clockwise
        bool clockwise = false;
        if(HolisticMath.Cross(transVector,facing).z < 0)
            clockwise = true;
        // Rotate the movement vector to align it with the object's facing direction
        transVector = HolisticMath.Rotate(transVector, angle + worldAngle, clockwise);
```

- Adds the *rotated movement vector* to the *current position* to get the new position.

```csharp
        // Add the rotated movement vector to the current position to get the new position
        float xVal = position.x + transVector.x;
        float yVal = position.y + transVector.y;
        float zVal = position.z + transVector.z;
        // Return the new position after applying the translation
        return new Coords(xVal, yVal, zVal);
    }
```

#### Resources
[1] https://en.wikipedia.org/wiki/Rotation_matrix  

[2] https://www.udemy.com/course/games_mathematics/
