using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float cameraSpeed = 10f;
    [SerializeField] Vector3 presetPosition;
    public static Vector3 cameraPosition;
    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate() {
        transform.position = Vector3.Lerp(transform.position, presetPosition + cameraPosition, Time.deltaTime * cameraSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
