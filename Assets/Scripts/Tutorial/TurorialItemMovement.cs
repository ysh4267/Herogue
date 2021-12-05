using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurorialItemMovement : MonoBehaviour
{
    float yPos = 4.0f;
    int temp = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (yPos < 3.5) temp = 1;
        else if (yPos > 4.5) temp = -1;
        yPos += 0.001f * temp;
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        transform.Rotate(new Vector3(0, 0.1f, 0));
    }
}
