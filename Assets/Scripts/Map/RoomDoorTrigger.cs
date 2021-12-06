using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//방에 입장하는 트리거와 같이 사용했을때 방에 입장하기 직전에 문이 생기는 문제가 있어
//문을 만드는 타이밍을 방에 완전히 들어왔을때로 따로 지정
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
