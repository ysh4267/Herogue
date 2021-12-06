using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float cameraSpeed = 10f;

    //방의 좌표값에 따른 포지션 프리셋값
    [SerializeField] Vector3 presetPosition;
    
    //보스방의 카메라 위치 프리셋값
    [SerializeField] Vector3 presetBossRoomPosition;
    
    //접근의 용이성을 위해 정적변수로 선언
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
