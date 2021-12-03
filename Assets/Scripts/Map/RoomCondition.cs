using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCondition : MonoBehaviour {
    List<GameObject> MonsterListInRoom = new List<GameObject>(); //해당 방에 존재하는 몬스터 리스트
    public RoomObjectController roomObjectController;
    public bool playerInThisRoom = false;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            CameraController.cameraPosition = new Vector3((GetComponent<RoomCordinate>().x - 5) * 48, 0, (GetComponent<RoomCordinate>().y - 5) * 32);
            CameraController.playerRoomType = GetComponent<RoomCordinate>().roomType;
        }

        if (other.gameObject.CompareTag("Player")) {
            playerInThisRoom = true;
        }

        if (other.CompareTag("Monster")) {
            //몬스터가 방에 있는가
            MonsterListInRoom.Add(other.gameObject); //콜라이더에 부딪힌 몬스터를 MonsterListInRoom에 추가
            Debug.Log(other.gameObject.name);
        }
    }

    void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			//플레이어가 빠져나감
			playerInThisRoom = false;
		}
		if (other.CompareTag("Monster")) {
			//몬스터를 쓰러트림
			MonsterListInRoom.Remove(other.gameObject);
		}
	}
}