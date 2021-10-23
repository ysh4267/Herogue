using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCondition : MonoBehaviour
{
    public RoomObjectController roomObjectController;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            CameraController.cameraPosition = new Vector3((GetComponent<RoomCordinate>().x - 5) * 48, 0, (GetComponent<RoomCordinate>().y - 5) * 32);
            CameraController.playerRoomType = GetComponent<RoomCordinate>().roomType;
        }
    }
}
