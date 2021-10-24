using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float cameraSpeed = 10f;
    [SerializeField] Vector3 presetPosition;
    [SerializeField] Vector3 presetBossRoomPosition;
    public static Vector3 cameraPosition;
    public static RoomData.RoomType playerRoomType;

    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate() {
        if (playerRoomType == RoomData.RoomType.BossRoom) {
            transform.position = Vector3.Lerp(transform.position, presetBossRoomPosition + cameraPosition, Time.deltaTime * cameraSpeed);
        }
        else {
            transform.position = Vector3.Lerp(transform.position, presetPosition + cameraPosition, Time.deltaTime * cameraSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
