using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//오브젝트에서 추적하는 방의 정보
public class RoomCondition : MonoBehaviour {
    public List<GameObject> monsterListInRoom = new List<GameObject>(); //해당 방에 존재하는 몬스터 리스트
    public RoomObjectController roomObjectController;
    public bool playerInThisRoom = false;

    //문이 한번이라도 열렸었는가
    public bool isOpened = false;
    //클리어 했는가
    public bool isClear = false;

    void Update() {
        if (monsterListInRoom.Count < 1) {
            isClear = true;
        } 
        if (!isOpened&&isClear) {
            roomObjectController.OpenDoor();
            isOpened = true;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            //카메라컨트롤러를 스태틱으로 선언해서 사용
            CameraController.cameraPosition = new Vector3((GetComponent<RoomCordinate>().x - 5) * 48, 0, (GetComponent<RoomCordinate>().y - 5) * 32);
            CameraController.playerRoomType = GetComponent<RoomCordinate>().roomType;
        }

        if (other.CompareTag("Monster")) {
            //몬스터가 방에 있는가
            monsterListInRoom.Add(other.gameObject); //콜라이더에 부딪힌 몬스터를 MonsterListInRoom에 추가
        }
    }

    void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			//플레이어가 빠져나감
			playerInThisRoom = false;
		}
        // 몬스터오브젝트를 삭제할때 OnTriggerExit판정이 아니라고 판단하여 직접 Remove로 삭제함
		// if (other.CompareTag("Monster")) {
		// 	//몬스터를 쓰러트림
		// 	monsterListInRoom.Remove(other.gameObject);
		// }
	}
}
