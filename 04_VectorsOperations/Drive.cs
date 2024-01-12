using UnityEngine;
using System.Collections;

public class Drive : MonoBehaviour
{
    float speed = 5; // Speed of movement
    public GameObject fuel; // Reference to the fuel depot GameObject
    Vector3 direction; // Direction towards the fuel depot
    float stoppingDistance = 0.1f; // Minimum distance to stop from the fuel depot
    
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
        /*
        float a = HolisticMath.Angle(new Coords(this.transform.up), new Coords(direction));

        bool clockwise = false;
        if(HolisticMath.Cross(new Coords(this.transform.up), dirNormal).z < 0)
            clockwise = true; 

        Coords newDir = HolisticMath.Rotate(new Coords(this.transform.up), a, clockwise);
        this.transform.up = new Vector3(newDir.x, newDir.y, newDir.z);
        */
    }

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