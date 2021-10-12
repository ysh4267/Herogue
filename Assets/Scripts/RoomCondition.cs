using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCondition : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            CameraController.cameraPosition = new Vector3((GetComponent<MapGenData>().x - 5) * 48, 0, (GetComponent<MapGenData>().y - 5) * 32);
        }
    }
}
