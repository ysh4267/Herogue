using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDoorTrigger : MonoBehaviour {
    [SerializeField] RoomCondition roomCondition = null;
    [SerializeField] RoomObjectController roomObjectController = null;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            roomCondition.playerInThisRoom = true;
            if (!roomCondition.isClear) {
                roomObjectController.CloseDoor();
            }
        }
    }
}
